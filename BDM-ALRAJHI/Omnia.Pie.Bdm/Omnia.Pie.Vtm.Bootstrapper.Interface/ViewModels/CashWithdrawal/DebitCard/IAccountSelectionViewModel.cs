using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Collections.Generic;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.DebitCard
{
	public interface IAccountSelectionViewModel : IExpirableBaseViewModel
	{
		double? Amount { get; set; }
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		//Action<Account, double?> AccountSelectedAction { get; set; }
		//ICommand AccountSelectedCommand { get; }
		ICommand QuickCashCommand { get; }
		ICommand AccountSelectedCommand { get; }
	}
}
