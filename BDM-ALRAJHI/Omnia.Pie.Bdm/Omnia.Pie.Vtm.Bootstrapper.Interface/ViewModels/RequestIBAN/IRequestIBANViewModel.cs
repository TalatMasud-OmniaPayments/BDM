using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IRequestIBANViewModel : IExpirableBaseViewModel
	{
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		AccountDetailResult AccountDetail { get; set; }
		string IBANNo { get; set; }
	}
}