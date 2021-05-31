using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class CashAcceptorViewModel : DeviceViewModel
	{
		public CashAcceptorViewModel()
		{
			AcceptCash = new OperationViewModel<Cash>(() => Model.AcceptCashAsync())
			{
				Id = nameof(Model.AcceptCashAsync)
			};
			AcceptMoreCash = new OperationViewModel<Cash>(() => Model.AcceptMoreCashAsync())
			{
				Id = nameof(Model.AcceptMoreCashAsync)
			};
			CancelAcceptCash = new OperationViewModel(() => Model.CancelAcceptCash())
			{
				Id = nameof(Model.CancelAcceptCash)
			};
			StoreCash = new OperationViewModel(() => Model.StoreCashAsync())
			{
				Id = nameof(Model.StoreCashAsync)
			};
			RollbackCash = new OperationViewModel(() => Model.RollbackCashAndWaitTakenAsync())
			{
				Id = nameof(Model.RollbackCashAndWaitTakenAsync)
			};
			RetractCash = new OperationViewModel(() => Model.RetractCashAsync())
			{
				Id = nameof(Model.RetractCashAsync)
			};
		}

		new ICashAcceptor Model => (ICashAcceptor)base.Model;

		public bool HasMediaInserted => Model.HasMediaInserted;
		public bool IsCashInRunning => Model.IsCashInRunning;

		public OperationViewModel AcceptCash { get; private set; }
		public OperationViewModel AcceptMoreCash { get; private set; }
		public OperationViewModel CancelAcceptCash { get; private set; }
		public OperationViewModel StoreCash { get; private set; }
		public OperationViewModel RollbackCash { get; private set; }
		public OperationViewModel RetractCash { get; private set; }

		public override void Load()
		{
			base.Load();
			RaisePropertyChanged(nameof(IsCashInRunning));
			RaisePropertyChanged(nameof(HasMediaInserted));
		}
	}
}
