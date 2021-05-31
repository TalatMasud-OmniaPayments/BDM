namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.ChequePrinting
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ChequePrinting;
	using Omnia.Pie.Vtm.Services.Interface.Entities;

	public class ChargesConfirmationViewModel : BaseViewModel, IChargesConfirmationViewModel
	{
		public ChargesConfirmationViewModel()
		{

		}

		public Account SelectedAccount { get; set; }
		public int NumberOfCheques { get; set; }
		public double? Charges { get; set; }

		public void Dispose()
		{

		}
	}
}