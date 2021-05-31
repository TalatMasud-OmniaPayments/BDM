namespace Omnia.Pie.Vtm.Devices.PinPad
{
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using AxNXPinXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Extensions;

	public class PinPad : Device, IPinPad
	{
		private bool _startedReading;
		AxNXPinX ax;

		public event EventHandler CancelPressed;
		public event EventHandler ClearPressed;
		public event EventHandler<PinPadDigitPressedEventArgs> DigitPressed;
        public event EventHandler<string> PinPadStatusChanged;
        public event EventHandler EnterPressed;
		internal readonly DeviceOperation<bool> ReadDataCancelOperation;
		internal readonly DeviceOperation<bool> ReadPinCancelOperation;

		public PinPad(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				ReadPinCancelOperation = new DeviceOperation<bool>(nameof(ReadPinCancelOperation), Logger, Journal),
				ReadDataCancelOperation = new DeviceOperation<bool>(nameof(ReadPinCancelOperation), Logger, Journal)
			});

			Logger.Info("PinPad Initialized");
		}

		public void ImportKey(string pLoadKeyName, string pEncKeyName, string pKeyValue, string pUse, out string pStrKeyCheckValue)
		{
			pStrKeyCheckValue = "000000";
			var res = ax.ImportKey(pLoadKeyName, pEncKeyName, pKeyValue, pUse);
			if (res == 0)
			{
				string str = ax.ImportedKey();
				if (str.Length >= 6)
					pStrKeyCheckValue = str.Substring(0, 6);
			}
		}

		#region Overridden Functions

		protected override AxHost CreateAx()
		{
			return ax = new AxNXPinX();
		}

		protected override int OpenSessionSync(int timeout)
		{
			return ax.OpenSessionSync(timeout);
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		protected override void OnInitialized()
		{
			ax.KeyPressed += Ax_KeyPressed;
			ax.DeviceError += Ax_DeviceError;
            ax.DeviceStatusChanged += Ax_DeviceStatusChanged;
			ax.FatalError += Ax_FatalError;
			ax.ResetComplete += Ax_ResetComplete;
			ax.ReadPinCancelled += Ax_ReadPinCancelled;
			ax.ReadDataCancelled += Ax_ReadDataCancelled;
		}

        

        private void Ax_ReadDataCancelled(object sender, EventArgs e)
		{
			Logger.Info("Ax_ReadDataCancelled");

			ReadDataCancelOperation.Stop(true);

			var activeKeys = string.Join(",", PinPadKeys.Numbers, PinPadKeys.Enter, PinPadKeys.Cancel, PinPadKeys.Clear);
			var readPinResult = ax.ReadPin(4, 12, 0, activeKeys, PinPadKeys.Cancel, 0);

			if (readPinResult != DeviceResult.Ok)
			{
				throw new DeviceMalfunctionException("ReadPin", readPinResult);
			}

			Logger.Info($"{nameof(PinPad)} Started pin reading, result={readPinResult}");
		}

		private void Ax_ReadPinCancelled(object sender, EventArgs e)
		{
			Logger.Info("Ax_ReadPinCancelled");

			ReadPinCancelOperation.Stop(true);

			// Resume reading if it has been started
			if (_startedReading)
			{
				StartReading();
			}
		}

		protected override void OnDisposing()
		{
			ax.KeyPressed -= Ax_KeyPressed;
			ax.DeviceError -= Ax_DeviceError;
            ax.DeviceStatusChanged -= Ax_DeviceStatusChanged;
			ax.FatalError -= Ax_FatalError;
			ax.ResetComplete -= Ax_ResetComplete;
			ax.ReadPinCancelled -= Ax_ReadPinCancelled;
			ax.ReadDataCancelled -= Ax_ReadDataCancelled;
		}

		#endregion

		#region Public Functions

		public void StartReading()
		{
			var activeKeys = string.Join(",", PinPadKeys.Numbers, PinPadKeys.Enter, PinPadKeys.Cancel, PinPadKeys.Clear);
			var deviceResult = ax.ReadData(0, 0, activeKeys, string.Empty, 0);

			if (deviceResult != DeviceResult.Ok)
			{
				throw new DeviceMalfunctionException("ReadData", deviceResult);
			}
			_startedReading = true;
			
			Logger.Info($"{nameof(PinPad)} Started reading keys");
		}

		public async Task StartPinReading()
		{
			Logger.Info($"{nameof(PinPad)} Cancelling data reading");

			await ReadDataCancelOperation.StartAsync(() =>
			{
				int cancelResult = ax.CancelReadData();
				if (cancelResult != DeviceResult.Ok)
				{
					throw new DeviceMalfunctionException("CancelReadData", cancelResult);
				}

				return cancelResult;
			});
		}

		public async Task StopPinReading()
		{
			Logger.Info($"{nameof(PinPad)} Stop pin reading task");

			await ReadPinCancelOperation.StartAsync(() =>
			{
               // Logger.Info($"{nameof(PinPad)} Entering ReadPinCancelOperation.StartAsync");
                int cancelResult = 0;
                try
                {
                    //Logger.Info($"{nameof(PinPad)} Calling ax.CancelReadPin()");
                    cancelResult = ax.CancelReadPin();
                    //Logger.Info($"{nameof(PinPad)} Calling ax.CancelReadPin()....Done! Result={cancelResult} (0=OK)");
                    if (cancelResult != DeviceResult.Ok)
                    {
                        Logger.Error($"{nameof(PinPad)} Failed to stop pin reading: {cancelResult}.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"{nameof(PinPad)} Failed to stop pin reading. Error encountered.");
                    Logger.Error($"{nameof(PinPad)} {ex.Message}");
                }

                //Logger.Info($"{nameof(PinPad)} Returning Cancel Result={cancelResult}");
                return cancelResult;
			});

            //Logger.Info($"{nameof(PinPad)} Stop pin reading task...Done!");
            return;
        }

		public async Task<string> BuildPinBlockAsync(Card card)
		{
			try
			{
				Logger.Info($"{nameof(PinPad)} Building pin block");
				string pinBlock = await new BuildPinBlockOperation(ax).ExecuteAsync(card);

				Logger.Info($"{nameof(PinPad)} Completed building pin block");
				return pinBlock;
			}
			catch (Exception ex)
			{
				ex = ex.ToDeviceException();
				Logger.Exception(ex);
				throw ex;
			}
		}

		public async Task<string> BuildPinBlockDieboldAsync(Card card)
		{
			try
			{
				Logger.Info($"{nameof(PinPad)} Building pin block");
				string pinBlock = await new BuildPinBlockOperation(ax).ExecuteDieboldAsync(card);

				Logger.Info($"{nameof(PinPad)} Completed building pin block");
				return pinBlock;
			}
			catch (Exception ex)
			{
				ex = ex.ToDeviceException();
				Logger.Exception(ex);
				throw ex;
			}
		}

		public async Task<string> BuildPinBlockDieboldAsync(string card)
		{
			try
			{
				Logger.Info($"{nameof(PinPad)} Building pin block");
				string pinBlock = await new BuildPinBlockOperation(ax).ExecuteDieboldAsync(card);

				Logger.Info($"{nameof(PinPad)} Completed building pin block");
				return pinBlock;
			}
			catch (Exception ex)
			{
				ex = ex.ToDeviceException();
				Logger.Exception(ex);
				throw ex;
			}
		}

		public async Task<string> BuildPinBlockAsync(string formattedPan)
		{
			try
			{
				Logger.Info($"{nameof(PinPad)} Building pin block");
				string pinBlock = await new BuildPinBlockOperation(ax).ExecuteAsync(formattedPan);

				Logger.Info($"{nameof(PinPad)} Completed building pin block");
				return pinBlock;
			}
			catch (Exception ex)
			{
				ex = ex.ToDeviceException();
				Logger.Exception(ex);
				throw ex;
			}
		}

		public override Task ResetAsync() => ResetOperation.StartAsync(() => ax.Reset());

		#endregion

		#region Private Functions & Events

		private void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);

		private void Ax_FatalError(object sender, _DNXPinXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));

		private void Ax_DeviceError(object sender, _DNXPinXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));

        private void Ax_DeviceStatusChanged(object sender, _DNXPinXEvents_DeviceStatusChangedEvent e)
        {
            Logger.Info($"PinPad status changed{sender.ToString()} AndEvent: {e.value} => Ax_StatusChanged");
            PinPadStatusChanged?.Invoke(sender, e.value);
        }

        private void Ax_KeyPressed(object sender, _DNXPinXEvents_KeyPressedEvent e) => OnEvent(() =>
		{
			if (string.Equals(e.key, PinPadKeys.Backspace, StringComparison.OrdinalIgnoreCase))
			{
				ClearPressed?.Invoke(this, EventArgs.Empty);
			}
			else if (string.Equals(e.key, PinPadKeys.Clear, StringComparison.OrdinalIgnoreCase))
			{
				ClearPressed?.Invoke(this, EventArgs.Empty);
			}
			else if (string.Equals(e.key, PinPadKeys.Cancel, StringComparison.OrdinalIgnoreCase))
			{
				CancelPressed?.Invoke(this, EventArgs.Empty);
			}
			else if (string.Equals(e.key, PinPadKeys.Enter, StringComparison.OrdinalIgnoreCase))
			{
				EnterPressed?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				int digit;
				if (int.TryParse(e.key, out digit))
				{
					DigitPressed?.Invoke(this, new PinPadDigitPressedEventArgs(digit));
				}
			}
		});

		#endregion
	}
}