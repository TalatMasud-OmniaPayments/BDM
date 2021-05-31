using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Microsoft.Practices.Unity;
//using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Supervisor.Shell.Utilities;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public class CardReaderViewModel : DeviceViewModel
	{
		public CardReaderViewModel()
		{
			Operations = new IOperationViewModel<DeviceViewModel, object>[] {
				new OperationViewModel<CardReaderViewModel, Card>(this) {
					Id = nameof(ICardReader.ReadCardAsync).ToHumanString(),
					Execute = x => x.device.ReadCardAsync(false),
					CanExecute = x => Connected
				},
				new OperationViewModel<CardReaderViewModel, Card>(this) {
					Id = nameof(ICardReader.ReadCardAsync).ToHumanString(),
					Execute = x => x.device.ReadCardAsync(true),
					CanExecute = x => Connected
				},
				new OperationViewModel<CardReaderViewModel, object>(this) {
					Id = nameof(ICardReader.EjectCardAndWaitTakenAsync).ToHumanString(),
					Execute = async x => {
						await x.device.EjectCardAndWaitTakenAsync();
						return null;
					},
					CanExecute = x => Connected
				}
			};
		}

		readonly ICardReader device = ServiceLocator.Instance.Resolve<ICardReader>();
		public override IDevice Device => device;
	}
}