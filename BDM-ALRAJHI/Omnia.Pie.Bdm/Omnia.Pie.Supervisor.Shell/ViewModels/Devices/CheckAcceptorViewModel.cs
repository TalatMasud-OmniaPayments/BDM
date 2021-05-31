namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	using Omnia.Pie.Supervisor.Shell.Service;
	using Omnia.Pie.Supervisor.Shell.Utilities;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Microsoft.Practices.Unity;

	public class CheckAcceptorViewModel : DeviceViewModel
	{
		public CheckAcceptorViewModel()
		{
			Vm = new OperationViewModel<CheckAcceptorViewModel, Cheque[]>(this)
			{
				Id = nameof(IChequeAcceptor.AcceptChequesAsync).ToHumanString(),
				Execute = x => x.device.AcceptChequesAsync(),
				CanExecute = x => Connected
			};

			Operations = new IOperationViewModel<DeviceViewModel, object>[] {
				Vm,
				new OperationViewModel<CheckAcceptorViewModel, object>(this)
				{
					Id = nameof(IChequeAcceptor.StoreChequesAsync).ToHumanString(),
					Execute = async x => {
						await x.device.StoreChequesAsync();
						return null;
					},
					CanExecute = x => Connected
				},
				new OperationViewModel<CheckAcceptorViewModel, object>(this)
				{
					Id = nameof(IChequeAcceptor.RollbackChequesAsync).ToHumanString(),
					Execute = async x => {
							await x.device.RollbackChequesAsync();
							return null;
						},
					CanExecute = x => Connected
				},
				new OperationViewModel<CheckAcceptorViewModel, object>(this)
				{
					Id = nameof(IChequeAcceptor.ResetAsync).ToHumanString(),
					Execute = async x =>
					{
						await x.device.ResetAsync();
						return null;
					},
					CanExecute = x => Connected
				}
			};
		}

		OperationViewModel<CheckAcceptorViewModel, Cheque[]> Vm;
		readonly IChequeAcceptor device = ServiceLocator.Instance.Resolve<IChequeAcceptor>();
		public override IDevice Device => device;
	}
}
