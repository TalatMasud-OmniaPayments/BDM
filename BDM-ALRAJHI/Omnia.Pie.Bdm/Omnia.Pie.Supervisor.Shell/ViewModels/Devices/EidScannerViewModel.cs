using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
//using Omnia.Pie.Vtm.Devices.EmiratesIdScanner;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public class EidScannerViewModel : DeviceViewModel
	{
		public EidScannerViewModel()
		{
			Operations = new IOperationViewModel<DeviceViewModel, object>[] {
				new OperationViewModel<EidScannerViewModel, ScannedEmiratesId>(this)
				{
					Id = nameof(IEmiratesIdScanner.ScanEmiratesIdAsync).ToHumanString(),
					Execute = x => x.device.ScanEmiratesIdAsync(),
					CanExecute = x => Connected
				},
				new OperationViewModel<EidScannerViewModel, object>(this)
				{
					Id = nameof(IEmiratesIdScanner.EjectEmiratesIdAsync).ToHumanString(),
					Execute = async x => {
							await x.device.EjectEmiratesIdAsync();
							return null;
						},
					CanExecute = x => Connected
				}
			};
		}

		public override IDevice Device => device;
		readonly IEmiratesIdScanner device = ServiceLocator.Instance.Resolve<IEmiratesIdScanner>();
		public override string Id => "Emirates ID scanner";
	}
}
