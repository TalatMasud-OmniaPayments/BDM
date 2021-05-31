using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.ChequeDeposit
{
	class ChequeDepositEncashConfirmationViewModel : BaseViewModel, IChequeDepositEncashConfirmationViewModel
	{
		public Account SelectedAccount { get; set; }
		public string ChequeDate { get; set; }
		public double Amount { get; set; }
		public Visibility CannotDispenseCoins { get; set; }
		public Visibility CanDispenseCoins { get; set; }

		public void Dispose()
		{
		}
	}
}
