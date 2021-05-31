using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface IChequeDepositEncashConfirmationViewModel : IBaseViewModel
	{
		Account SelectedAccount { get; set; }
		string ChequeDate { get; set; }
		double Amount { get; set; }
		Visibility CannotDispenseCoins { get; set; }
		Visibility CanDispenseCoins { get; set; }
	}
}
