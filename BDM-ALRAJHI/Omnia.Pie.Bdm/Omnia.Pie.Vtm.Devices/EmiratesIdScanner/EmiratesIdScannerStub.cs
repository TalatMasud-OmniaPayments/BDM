namespace Omnia.Pie.Vtm.Devices.EmiratesIdScanner
{
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Windows.Media.Imaging;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Interface;

	public class EmiratesIdScannerStub : Device, IEmiratesIdScanner
	{
		public bool HasMediaInserted { get; private set; }
		public bool IsEmiratesIdInside => HasMediaInserted;
		public event EventHandler MediaChanged;

		public EmiratesIdScannerStub(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal,
			IGuideLights guideLights) : base(deviceErrorStore, logger, journal, guideLights)
		{
		}

		protected override int CloseSessionSync()
		{
			return DeviceResult.Ok;
		}

		protected override int OpenSessionSync(int timeout)
		{
			return DeviceResult.Ok;
		}

		protected override AxHost CreateAx() => null;
		protected override string GetDeviceStatus() => null;

		readonly BitmapImage frontImage = BitmapExtender.LoadImageFromExecutablePath("eidFront.bmp");
		readonly BitmapImage backmage = BitmapExtender.LoadImageFromExecutablePath("eidBack.bmp");

		public Task<ScannedEmiratesId> ScanEmiratesIdAsync()
		{
			HasMediaInserted = true;
			return Task.FromResult(new ScannedEmiratesId
			{
				Front = frontImage,
				Back = backmage
			});
		}

		public Task CancelScan()
		{
			HasMediaInserted = false;
			return Task.FromResult(0);
		}

		public Task EjectEmiratesIdAsync()
		{
			HasMediaInserted = false;
			return Task.FromResult(0);
		}

		public MediaUnit[] GetMediaInfo() => null;

		public void SetMediaInfo(int[] ids, int[] counts) { }
	}
}