namespace Omnia.Pie.Vtm.Devices.SignpadScanner
{
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using AxNXSignpadScannerXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Devices.Interface.Enum;

	public class SignpadScanner : Device, ISignpadScanner
	{
		public SignpadScanner(IDeviceErrorStore deviceErrorStore, IGuideLights guideLights, ILogger logger, IJournal journal)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				CaptureSignOperation = new DeviceOperation<SignPadImage>(nameof(CaptureSignOperation), logger, journal)
			});

			Logger.Info("SignpadScanner Initialized --"); //this is a test change3
        }

		internal readonly DeviceOperation<SignPadImage> CaptureSignOperation;

		AxNXSignpadScannerX ax;
		protected override AxHost CreateAx() => ax = new AxNXSignpadScannerX();
		protected override int CloseSessionSync() => ax.CloseSessionSync();
		protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
		protected override string GetDeviceStatus() => ax.DeviceStatus;

		protected override void OnInitialized()
		{
			ax.Timeout += Ax_Timeout;
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.ReadImageComplete += Ax_ReadImageComplete;
			ax.ResetComplete += Ax_ResetComplete;
		}

		protected override void OnDisposing()
		{
			ax.Timeout -= Ax_Timeout;
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.ReadImageComplete -= Ax_ReadImageComplete;
			ax.ResetComplete -= Ax_ResetComplete;
		}

		void Ax_FatalError(object sender, _DNXSignpadScannerXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_DeviceError(object sender, _DNXSignpadScannerXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_Timeout(object sender, EventArgs e) => OnError(new DeviceTimeoutException(nameof(SignpadScanner)));
		void Ax_ReadImageComplete(object sender, EventArgs e) => CaptureSignOperation.Stop(getSign());
		void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);

		private SignPadImage getSign()
		{
			try
			{
				var image = BitmapExtender.BitmapImageFromMemory((dynamic)ax.GetImage("FRONT"));
				return new SignPadImage
				{
					Image = BitmapExtender.IncreaseContrast(
						BitmapExtender.Resize(image, image.PixelWidth / 4, image.PixelHeight / 4))
				};
			}
			catch (Exception ex)
			{
				Logger.Exception(ex);
				return null;
			}
		}

		public Task<SignPadImage> CaptureSignAsync() => CaptureSignOperation.StartAsync(() =>
			ax.ReadImage("FRONT", "GRAYSCALE", "GRAYSCALE", "CMC7", "BMP", "BMP", null, null, Timeout.Scan));

		public override Task ResetAsync() => ResetOperation.StartAsync(() => ax.Reset(PrinterScannerResetAction.EJECT.ToString(), 0));
	}
}