using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class CashDispenserViewModel : DeviceViewModel
	{
		public CashDispenserViewModel()
		{
			Present = new OperationViewModel(() => Model.PresentCashAndWaitTakenAsync(100))
			{
				Id = nameof(Model.PresentCashAndWaitTakenAsync)
			};
			Retract = new OperationViewModel(() => Model.RetractCashAsync())
			{
				Id = nameof(Model.RetractCashAsync)
			};
		}

		new ICashDispenser Model => (ICashDispenser)base.Model;

		public OperationViewModel Present { get; private set; }
		public OperationViewModel Retract { get; private set; }

		public override void Load()
		{
			base.Load();
		}
	}
}