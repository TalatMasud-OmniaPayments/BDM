using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit
{
	public interface ICashDepositResultViewModel : IBaseViewModel
	{
		Account SelectedAccount { get; set; }
		int TotalAmount { get; set; }
		string Currency { get; set; }
		ICommand PrintReceiptEnglishCommand { get; }
		ICommand PrintReceiptArabicCommand { get; }
		ICommand NoReceiptCommand { get; }
		Action<bool, string> PrintReceipt { get; set; }
		string ReceiptLanguage { get; set; }
	}
}
