using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
//using Omnia.Pie.Vtm.Devices.PinPad;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices {
	public class PinPadViewModel : DeviceViewModel {
		public override IDevice Device => model;
		readonly IPinPad model = ServiceLocator.Instance.Resolve<IPinPad>();
		public override string Id => "Pin Pad ";
	}
}
