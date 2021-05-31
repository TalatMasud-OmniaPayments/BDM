using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface IChequeDepositConfirmationViewModel : IExpirableBaseViewModel
	{
		Account SelectedAccount { get; set; }
		Cheque[] DepositedCheques { get; set; }
		int TotalCheques { get; set; }
		string ChequeDate { get; set; }
		string ChequeAmount { get; set; }
		Visibility DateVisibile { get; set; }
		ICommand RotateCommand { get; }
		Visibility DaysVisible { get; set; }
	}
}
