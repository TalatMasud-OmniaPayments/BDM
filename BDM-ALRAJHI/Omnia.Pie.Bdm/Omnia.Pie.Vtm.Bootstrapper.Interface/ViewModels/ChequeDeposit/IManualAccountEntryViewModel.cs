using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface IManualAccountEntryViewModel : IExpirableBaseViewModel
	{
		string AccountNumber { get; set; }
		ICommand ValidateAccountCommand { get; }
	}
}
