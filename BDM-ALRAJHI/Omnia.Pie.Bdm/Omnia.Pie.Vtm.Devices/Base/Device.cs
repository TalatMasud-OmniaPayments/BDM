using Microsoft.Win32;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Constants;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Extensions;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessEntities = Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Vtm.Devices
{
    public abstract class Device : IDevice, IDisposable
    {
        protected Device(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights)
        {
            GuideLights = guideLights;
            Logger = logger;
            Journal = journal;
            DeviceErrorStore = deviceErrorStore;
            DeviceObservers = new List<IDeviceObserver>();
            Operations.AddRange(new DeviceOperation[] {
                InitializeOperation = new DeviceOperation<bool>(nameof(InitializeOperation), logger, journal, false),
                DisposeOperation = new DeviceOperation<bool>(nameof(DisposeOperation), logger, journal, false),
                ResetOperation = new DeviceOperation<bool>(nameof(ResetOperation), Logger, Journal),
                TestOperation= new DeviceOperation<bool>(nameof(TestOperation), Logger, Journal)
            });

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            var filePath = Path.Combine(path, "Resources\\Sounds\\click.wav");
            _player = new SoundPlayer(filePath);
        }

        internal readonly DeviceOperation<bool> InitializeOperation;
        internal readonly DeviceOperation<bool> DisposeOperation;
        internal readonly DeviceOperation<bool> ResetOperation;
        internal readonly DeviceOperation<bool> TestOperation;

        private SoundPlayer _player;
        private List<IDeviceObserver> DeviceObservers { get; }

        /// <summary>
        /// Set of guide lights used by devices
        /// </summary>
        /// 
        protected readonly IGuideLights GuideLights;
        protected readonly IJournal Journal;
        protected readonly IDeviceErrorStore DeviceErrorStore;

        protected ILogger Logger { get; private set; }
        //protected readonly IChannelManagementService _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();

        /// <summary>
        /// The guide light used in the concrete device.
        /// Can be overriden in a concrete device to choose a guidelight that is turned on and off during the device activity
        /// </summary>
        /// <remarks>May be refactored  to an array of guidelighs, in case of devices wth many guide lights</remarks>
        protected virtual IGuideLight GuideLight => null;

        // These fields wrap the internal AX control and some of its logic
        // Unable to extract base logic for all AX controls as they do not have any cmmon interfaces.
        /// <summary>
        /// Should be overriden to get the AX control used in a concrete device
        /// </summary>
        /// <remarks>May be refactored further into an array of AX controls in case of devices with many AX controls</remarks>
        public AxHost Ax { get; private set; }
        protected abstract AxHost CreateAx();

        /// <summary>
        /// Should be overriden to point to OpenSessionSync() of a concrete device
        /// Used to open device session in Initialize()
        /// <see cref="Initialize"/>
        /// </summary>
        protected abstract int OpenSessionSync(int timeout);

        /// <summary>
        /// Should be overriden to point to CloseSessionSync() of a concrete device
        /// Used to close device session in Dispose()
        /// <see cref="Dispose"/>
        /// </summary>
        protected abstract int CloseSessionSync();

        public virtual Task ResetAsync() {
                return Task.FromResult(0);
            }
		public virtual Task TestAsync() => Task.FromResult(0);

		/// <summary>
		/// Initializes device usage
		/// Should be called before any activity with the device
		/// When overriden, can be used to initialize any additional resources, attach events.
		/// </summary>
		/// <seealso cref="Dispose"/>
		public void Initialize() => InitializeOperation.Start(() =>
		{
			if (Status == DeviceStatus.Online)
				return DeviceResult.Ok;
			Ax = CreateAx();
			Ax?.CreateControl();
			var result = OpenSessionSync(Timeout.Initialize);
			if (result == DeviceResult.Ok)
			{
				Status = DeviceStatus.Online;
				OnInitialized();
			}
			return result;
		});

		/// <summary>
		/// Should be called after all activity with the device
		/// When overriden, used to release any resources, detach events used during device activity
		/// </summary>
		/// <seealso cref="Initialize"/>
		public void Dispose() => DisposeOperation.Start(() =>
		{
			if (Status != DeviceStatus.Online)
				return DeviceResult.Ok;
			int result = CloseSessionSync();
			if (result == DeviceResult.Ok)
			{
				OnDisposing();
				Status = DeviceStatus.Unknown;
			}
			return result;
		});

		public void AddObserver(IDeviceObserver deviceObserver)
		{
			if (deviceObserver == null)
			{
				throw new ArgumentNullException(nameof(deviceObserver));
			}
			DeviceObservers.Add(deviceObserver);
		}

		public void RemoveObserver(IDeviceObserver deviceObserver)
		{
			DeviceObservers.Remove(deviceObserver);
		}

		protected virtual void OnInitialized() { }
		protected virtual void OnDisposing() { }

		protected abstract string GetDeviceStatus();
		public bool IsAvailable => Status == DeviceStatus.Online;

		DeviceStatus _status = DeviceStatus.Unknown;
		public virtual DeviceStatus Status
		{
			get
			{
				var x = Ax != null ? GetDeviceStatus() : null;
				if (x != null) {

                    //Logger.Info("Device Status:" + x);
                    switch (x.ToUpper())
                    {
                        case "DEVONLINE":
                            _status = DeviceStatus.Online;
                            break;
                        case "DEVOFFLINE":
                            _status = DeviceStatus.Offline;
                            break;
                        case "DEVPOWEROFF":
                            _status = DeviceStatus.PowerOff;
                            break;
                        case "DEVNODEVICE":
                            _status = DeviceStatus.NoDevice;
                            break;
                        case "DEVHWERROR":
                            _status = DeviceStatus.HwError;
                            break;
                        case "DEVUSERERROR":
                            _status = DeviceStatus.UserError;
                            break;
                        case "DEVBUSY":
                            _status = DeviceStatus.Busy;
                            break;
                        default:
                            _status = DeviceStatus.Unknown;
                            break;
                    }

                    return _status;
                }
                return _status;
			}
			set
			{
				_status = value;
			}
		}

		internal readonly List<DeviceOperation> Operations = new List<DeviceOperation>();

		protected void OnError(Exception ex)
		{
			GuideLight?.TurnOff();
            //Logger.Info("OnError called");

			ex = ex.ToDeviceException();
			var runningOperations = Operations.Where(ii => ii.IsRunning).ToArray();
			if (runningOperations.Length != 0)
			{
				foreach (var operation in runningOperations)
				{
					operation.Stop(ex);
				}
			}
			else
			{
				Logger.Exception(ex);
			}

            //Logger.Info("StartDeviceError");
            var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
            var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
            channelService.SendDeviceStatus(devStatus.GetDevicesStatus());
            //Logger.Info("EndDeviceError");
            SaveDeviceError(ex);
		}

		protected void OnEvent(Action f)
		{
            //Logger.Info("OnEvent called");
            try
			{
				f();
			}
			catch (Exception ex)
			{
				ex = ex.ToDeviceException();
				Logger.Exception(ex);
				throw;
			}
		}

		protected void OnUserAction()
		{
            //Logger.Info("OnUserAction called");
            foreach (IDeviceObserver deviceObserver in DeviceObservers)
			{
				deviceObserver.OnUserAction(this);
			}
		}

		private async void SaveDeviceError(Exception exception)
		{
			try
			{
				if (DeviceErrorStore != null)
				{
					await DeviceErrorStore.Save(new DataAccessEntities.DeviceError
					{
						Source = GetType().Name,
						Created = DateTime.Now,
						Message = exception.Message,
						StatusSent = 0,
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);
			}
		}

		public async Task PlayBeepAsync()
		{
			await Task.Delay(1);
			_player?.PlayLooping();
		}

		public async Task StopBeepAsync()
		{
			await Task.Delay(1);
			_player?.Stop();
		}

		public DeviceError GetDeviceError(DeviceShortName name)
		{
			var devError = new DeviceError() { Code = string.Empty, Message = string.Empty };
			int x = 0;
            //Logger.Info("DeviceName: " + name);
            //Logger.Info("RegistryHive.LocalMachine: " + RegistryHive.LocalMachine);
            //Logger.Info("RegistryHive.Registry64: " + RegistryHive.Registry64);

            using (var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
                //Logger.Info("Reg open");
                var i = reg.OpenSubKey(@"SOFTWARE\ATM\ErrorCode\").GetValue(name.ToString());
				var s = i as string;
                //Logger.Info("ErrorCode: " + s);
                //s = "0";
                //var a = int.TryParse(s, out x);

                if (!string.IsNullOrWhiteSpace(s) && int.TryParse(s, out x) && s != "0000000" && x > 0)
				{
					devError.Message = name.ToString();
					devError.Code = s;
					devError.Time = DateTime.Now;
				}

			}
            //Logger.Info("Device out of registry");
            //Logger.Info($"Device ErrorMessage: {devError.Message} andCode: {devError.Code} andTime: {devError.Time}" );
            
            return devError;
		}
	}
}