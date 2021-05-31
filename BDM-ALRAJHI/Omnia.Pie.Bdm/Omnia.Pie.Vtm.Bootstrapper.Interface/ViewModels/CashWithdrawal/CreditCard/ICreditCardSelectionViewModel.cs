using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Collections.Generic;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.CreditCard
{
	public interface ICreditCardSelectionViewModel : IExpirableBaseViewModel
	{
		double? Amount { get; set; }
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		ICommand QuickCashCommand { get; }
		ICommand AccountSelectedCommand { get; }
	}
}