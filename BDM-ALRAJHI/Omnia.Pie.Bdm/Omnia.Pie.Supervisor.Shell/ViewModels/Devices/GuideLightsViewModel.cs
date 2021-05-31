using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices {
	public class GuideLightsViewModel : DeviceViewModel {
		public override IDevice Device => ServiceLocator.Instance.Resolve<IGuideLights>();
		public override string Id => "Guide Lights";
	}
}
