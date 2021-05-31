using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.CashDeposit
{
	public class CashDepositResultViewModel : BaseViewModel, ICashDepositResultViewModel
	{
		public Account SelectedAccount { get; set; }
		public int TotalAmount { get; set; }
		public string Currency { get; set; }
		public string ReceiptLanguage { get; set; }

		private ICommand _printReceiptEnglishCommand;
		public ICommand PrintReceiptEnglishCommand
		{
			get
			{
				if (_printReceiptEnglishCommand == null)
					_printReceiptEnglishCommand = new DelegateCommand(() => { PrintReceipt(true, "en"); });
				return _printReceiptEnglishCommand;
			}
		}

		private ICommand _printReceiptArabicCommand;
		public ICommand PrintReceiptArabicCommand
		{
			get
			{
				if (_printReceiptArabicCommand == null)
					_printReceiptArabicCommand = new DelegateCommand(() => { PrintReceipt(true, "ar"); });
				return _printReceiptArabicCommand;
			}
		}

		private ICommand _noReceiptCommand;
		public ICommand NoReceiptCommand
		{
			get
			{
				if (_noReceiptCommand == null)
					_noReceiptCommand = new DelegateCommand(NoReceiptTrigger);
				return _noReceiptCommand;
			}
		}

		public Action<bool, string> PrintReceipt { get; set; }

		private void NoReceiptTrigger()
		{
			PrintReceipt(false, "");
		}

		private void PrintReceiptTrigger(string lang)
		{
			PrintReceipt(true, lang);
		}
		public void Dispose()
		{

		}
	}
}
