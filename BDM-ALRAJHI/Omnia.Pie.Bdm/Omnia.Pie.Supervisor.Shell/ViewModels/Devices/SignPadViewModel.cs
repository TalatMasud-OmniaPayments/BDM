using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
//using Omnia.Pie.Vtm.Devices.SignpadScanner;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public class SignPadViewModel : DeviceViewModel
	{
		public SignPadViewModel()
		{
			Operations = new IOperationViewModel<DeviceViewModel, object>[] {
				new OperationViewModel<SignPadViewModel, SignPadImage>(this)
				{
					Id = nameof(ISignpadScanner.CaptureSignAsync).ToHumanString(),
					Execute = x => x.device.CaptureSignAsync(),
					CanExecute = x => Connected
				}
			};
		}

		public override IDevice Device => device;
		readonly ISignpadScanner device = ServiceLocator.Instance.Resolve<ISignpadScanner>();
		public override string Id => "Sign Pad";
	}
}
