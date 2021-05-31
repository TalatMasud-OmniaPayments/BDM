namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	using Omnia.Pie.Supervisor.Shell.Service;
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Vtm.Devices.Interface;

	public class CashDispenserViewModel : DeviceViewModel
	{
		public override IDevice Device => model;
		readonly ICashDispenser model = ServiceLocator.Instance.Resolve<ICashDispenser>();
		public override string Id => "Cash Dispenser";
	}
}