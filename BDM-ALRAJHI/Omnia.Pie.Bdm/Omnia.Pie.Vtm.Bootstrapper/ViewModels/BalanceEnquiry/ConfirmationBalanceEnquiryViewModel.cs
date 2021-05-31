namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;

	public class ConfirmationBalanceEnquiryViewModel : ExpirableBaseViewModel, IConfirmationBalanceEnquiryViewModel
	{
		public Account SelectedAccount { get; set; }
		public AccountDetailResult AcountDetail { get; set; }

		public void Dispose()
		{

		}
	}
}