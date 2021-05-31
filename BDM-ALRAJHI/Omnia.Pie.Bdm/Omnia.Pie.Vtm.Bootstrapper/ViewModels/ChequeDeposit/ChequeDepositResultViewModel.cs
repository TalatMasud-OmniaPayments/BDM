using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	public class ChequeDepositResultViewModel : BaseViewModel, IChequeDepositResultViewModel
	{
		public Account SelectedAccount { get; set; }

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

		private void NoReceiptTrigger()
		{
			PrintReceipt(false, "");
		}

		private void PrintReceiptTrigger(string lang)
		{
			PrintReceipt(true, lang);
		}

		public Action<bool, string> PrintReceipt { get; set; }
		public string ReceiptLanguage { get; set; }

		public void Dispose()
		{

		}
	}
}
