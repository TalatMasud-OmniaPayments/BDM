namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	using Omnia.Pie.Vtm.Devices.Interface;
    using Omnia.Pie.Vtm.Devices.Interface.Entities;

    public class FingerPrintScannerViewModel : DeviceViewModel
	{
		public FingerPrintScannerViewModel()
		{
            FingerPrintScanner = new OperationViewModel<string>(() => Model.CaptureFingerPrintAsync())
			{
				Id = nameof(Model.CaptureFingerPrintAsync)
			};
		}

		public OperationViewModel FingerPrintScanner { get; private set; }

		new IFingerPrintScanner Model => (IFingerPrintScanner)base.Model;

		public override void Load()
		{
			base.Load();
		}
	}
}