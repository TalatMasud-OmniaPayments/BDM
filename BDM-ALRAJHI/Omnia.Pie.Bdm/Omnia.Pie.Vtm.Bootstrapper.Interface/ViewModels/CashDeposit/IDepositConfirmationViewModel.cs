using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit
{
	public interface IDepositConfirmationViewModel : IExpirableBaseViewModel
	{
		Account SelectedAccount { get; set; }
		ICommand AddMoreCommand { get; }
		Action AddMoreAction { get; set; }
		List<DepositDenomination> DepositedCash { get; set; }
		int TotalNotes { get; set; }
		int TotalAmount { get; set; }
		string Currency { get; set; }
        string AvailableCapacity { get; set; }
}
}
