using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class CoinDispenserViewModel : DeviceViewModel
	{
		public CoinDispenserViewModel()
		{
			Present = new OperationViewModel(() => Model.PresentCoinsAsync(Amount))
			{
				Id = nameof(Model.PresentCoinsAsync)
			};
		}

		private int amount = 2;
		public int Amount
		{
			get { return amount; }
			set { SetProperty(ref amount, value); }
		}

		new ICoinDispenser Model => (ICoinDispenser)base.Model;

		public OperationViewModel Present { get; private set; }

		public override void Load()
		{
			base.Load();
		}
	}
}