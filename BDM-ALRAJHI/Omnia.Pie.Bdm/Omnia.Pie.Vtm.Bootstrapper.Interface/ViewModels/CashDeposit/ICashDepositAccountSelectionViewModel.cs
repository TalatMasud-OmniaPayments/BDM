using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit
{
	public interface ICashDepositAccountSelectionViewModel : IExpirableBaseViewModel
	{
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		ICommand ManualAccountEntryCommand { get; }
		Action ManualAccountEntryAction { get; set; }
		ICommand AccountSelectedCommand { get; }
	}
}
