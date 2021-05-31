using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class SignpadScannerViewModel : DeviceViewModel
	{
		public SignpadScannerViewModel() 
		{
			Capture = new OperationViewModel<SignPadImage>(() => Model.CaptureSignAsync())
			{
				Id = nameof(Model.CaptureSignAsync)
			};
		}

		new ISignpadScanner Model => (ISignpadScanner)base.Model;

		public OperationViewModel Capture { get; private set; }

		public override void Load()
		{
			base.Load();
		}
	}
}
