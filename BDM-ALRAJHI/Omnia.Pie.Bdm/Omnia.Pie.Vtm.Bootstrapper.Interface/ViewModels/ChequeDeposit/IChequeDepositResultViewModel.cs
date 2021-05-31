using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface IChequeDepositResultViewModel : IBaseViewModel
	{
		Account SelectedAccount { get; set; }
		ICommand PrintReceiptEnglishCommand { get; }
		ICommand PrintReceiptArabicCommand { get; }
		ICommand NoReceiptCommand { get; }
		Action<bool, string> PrintReceipt { get; set; }
		string ReceiptLanguage { get; set; }
	}
}
