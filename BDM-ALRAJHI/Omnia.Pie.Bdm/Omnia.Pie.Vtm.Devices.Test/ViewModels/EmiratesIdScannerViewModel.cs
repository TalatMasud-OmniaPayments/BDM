namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public class EmiratesIdScannerViewModel : DeviceViewModel
	{
		public EmiratesIdScannerViewModel()
		{
			Scan = new OperationViewModel<ScannedEmiratesId>(() => Model.ScanEmiratesIdAsync())
			{
				Id = nameof(Model.ScanEmiratesIdAsync)
			};
			Eject = new OperationViewModel(() => Model.EjectEmiratesIdAsync())
			{
				Id = nameof(Model.EjectEmiratesIdAsync)
			};
		}

		new IEmiratesIdScanner Model => (IEmiratesIdScanner)base.Model;

		public OperationViewModel Scan { get; private set; }
		public OperationViewModel Eject { get; private set; }

		public override void Load()
		{
			base.Load();
		}
	}
}