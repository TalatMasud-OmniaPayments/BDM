namespace Omnia.Pie.Vtm.Devices.SignpadScanner
{
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Windows.Media.Imaging;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;

	public class SignpadScannerStub : Device, ISignpadScanner
	{
		public SignpadScannerStub(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal,
			IGuideLights guideLights) : base(deviceErrorStore, logger, journal, guideLights)
		{
		}

		public Task<SignPadImage> CaptureSignAsync() => Task.FromResult(new SignPadImage { Image = image });
		readonly BitmapSource image = BitmapExtender.LoadImageFromExecutablePath("sign.bmp");

		protected override AxHost CreateAx() => null;
		protected override int CloseSessionSync() => DeviceResult.Ok;
		protected override int OpenSessionSync(int timeout) => DeviceResult.Ok;
		protected override string GetDeviceStatus() => null;
	}
}