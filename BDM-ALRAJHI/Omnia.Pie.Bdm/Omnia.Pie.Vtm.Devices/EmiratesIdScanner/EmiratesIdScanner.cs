namespace Omnia.Pie.Vtm.Devices.EmiratesIdScanner
{
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using AxNXIDScannerPrinterXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Enum;

	public class EmiratesIdScanner : Device, IEmiratesIdScanner
	{
		AxNXIDScannerPrinterX ax;
		internal readonly DeviceOperation<ScannedEmiratesId> ScanEmiratesIdOperation;
		internal readonly DeviceOperation<bool> EjectEmiratesIdOperation;
		internal readonly DeviceOperation<bool> CancelScanEmiratesIdOperation;

		public event EventHandler MediaChanged;
		public bool HasMediaInserted { get; private set; }
		public bool IsEmiratesIdInside => HasMediaInserted;

		public EmiratesIdScanner(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal,
			IGuideLights guideLights) : base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				ScanEmiratesIdOperation = new DeviceOperation<ScannedEmiratesId>(nameof(ScanEmiratesIdOperation), logger, journal),
				EjectEmiratesIdOperation = new DeviceOperation<bool>(nameof(EjectEmiratesIdOperation), logger, journal),
				CancelScanEmiratesIdOperation = new DeviceOperation<bool>(nameof(CancelScanEmiratesIdOperation), logger, journal)
			});

			Logger.Info("EmiratesIdScanner Initialized");
		}

		#region Overridden Functions

		protected override IGuideLight GuideLight
		{
			get
			{
				return GuideLights.EmiratesIdScanner;
			}
		}

		protected override AxHost CreateAx()
		{
			return ax = new AxNXIDScannerPrinterX();
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

		protected override int OpenSessionSync(int timeout)
		{
			return ax.OpenSessionSync(timeout);
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		protected override void OnInitialized()
		{
			ax.MediaInserted += Ax_MediaInserted;
			ax.MediaTaken += Ax_MediaTaken;
			ax.Timeout += Ax_Timeout;
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.ReadImageComplete += Ax_ReadImageComplete;
			ax.AcceptCancelled += Ax_AcceptCancelled;
			ax.ControlMediaComplete += Ax_ControlMediaComplete;
			ax.ResetComplete += Ax_ResetComplete;
		}

		protected override void OnDisposing()
		{
			ax.MediaInserted -= Ax_MediaInserted;
			ax.MediaTaken -= Ax_MediaTaken;
			ax.Timeout -= Ax_Timeout;
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.ReadImageComplete -= Ax_ReadImageComplete;
			ax.AcceptCancelled -= Ax_AcceptCancelled;
			ax.ControlMediaComplete -= Ax_ControlMediaComplete;
			ax.ResetComplete -= Ax_ResetComplete;
		}

		#endregion

		#region Public Functions

		public Task<ScannedEmiratesId> ScanEmiratesIdAsync()
		{
			GuideLight.TurnOn();
			return ScanEmiratesIdOperation.StartAsync(() => ax.ReadImage("BACK,FRONT", "COLORFULL", "GRAYSCALE", "CMC7", "BMP", "BMP", null, null, Timeout.Scan));
		}

		public Task EjectEmiratesIdAsync()
		{
			GuideLight.TurnOn();
			return EjectEmiratesIdOperation.StartAsync(() => ax.ControlMedia("EJECT", Timeout.AwaitTaken));
		}

		public Task CancelScan()
		{
			GuideLight.TurnOff();
			return CancelScanEmiratesIdOperation.StartAsync(() => ax.CancelAccept());
		}

		public override Task ResetAsync()
		{
			GuideLight.TurnOff();
			return ResetOperation.StartAsync(() => ax.Reset(PrinterScannerResetAction.RETRACT.ToString(), 1));
		}

		public MediaUnit[] GetMediaInfo() => null;

		public void SetMediaInfo(int[] ids, int[] counts) { }

		#endregion

		#region Private Functions & Events

		private void Ax_FatalError(object sender, _DNXIDScannerPrinterXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXIDScannerPrinterXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_Timeout(object sender, EventArgs e)
		{
			OnError(new DeviceTimeoutException(nameof(EmiratesIdScanner)));
		}

		private void Ax_AcceptCancelled(object sender, EventArgs e)
		{
			CancelScanEmiratesIdOperation.Stop(true);
		}

		private void Ax_ReadImageComplete(object sender, EventArgs e)
		{
			ScanEmiratesIdOperation.Stop(GetScannedEmiratesId());
		}

		private void Ax_MediaInserted(object sender, EventArgs e)
		{
			OnUserAction();
			GuideLight.TurnOff();
			HasMediaInserted = true;
		}

		private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
		}

		private void Ax_ControlMediaComplete(object sender, _DNXIDScannerPrinterXEvents_ControlMediaCompleteEvent e)
		{
			if (!HasMediaInserted)
				EjectEmiratesIdOperation.Stop(true);
		}

		private void Ax_MediaTaken(object sender, EventArgs e)
		{
			OnUserAction();
			GuideLight.TurnOff();
			EjectEmiratesIdOperation.Stop(true);
			HasMediaInserted = false;
		}

		private ScannedEmiratesId GetScannedEmiratesId()
		{
			try
			{
				return new ScannedEmiratesId
				{
					Front = BitmapExtender.BitmapImageFromMemory((dynamic)ax.GetImage("FRONT")),
					Back = BitmapExtender.BitmapImageFromMemory((dynamic)ax.GetImage("BACK"))
				};
			}
			catch (Exception e)
			{
				Logger.Exception(e);
				return null;
			}
		}

		#endregion
	}
}