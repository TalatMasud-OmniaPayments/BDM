namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;

	public interface IConfirmationBalanceEnquiryViewModel : IExpirableBaseViewModel
	{
		Account SelectedAccount { get; set; }
		AccountDetailResult AcountDetail { get; set; }
	}
}