using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices {
	public class PrinterViewModel : DeviceViewModel {
		public override IDevice Device => model;
		readonly IReceiptPrinter model = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
		public override string Id => "Printer/Scanner";
	}
}
