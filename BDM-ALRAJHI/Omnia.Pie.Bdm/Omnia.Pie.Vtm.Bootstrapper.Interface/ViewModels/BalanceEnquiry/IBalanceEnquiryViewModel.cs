namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System.Collections.Generic;

	public interface IBalanceEnquiryViewModel : IExpirableBaseViewModel
	{
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		AccountDetailResult AccountDetail { get; set; }
	}
}