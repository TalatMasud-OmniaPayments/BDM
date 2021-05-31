using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices {
	public class CashAcceptorViewModel : DeviceViewModel {
		readonly ICashAcceptor model = ServiceLocator.Instance.Resolve<ICashAcceptor>();
		public override IDevice Device => model;
		public override string Id => "Cash Acceptor";
	}
}
