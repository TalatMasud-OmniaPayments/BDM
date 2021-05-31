namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;
	using System.Collections.Generic;

	public class AuthenticatedDebitCardStandbyViewModel : ExpirableBaseViewModel, IAuthenticatedDebitCardStandbyViewModel
	{
		public bool TellerAssistanceVisibility { get; set; } = false;
		public bool IsSelfModeCalling { get; set; } = false;

		public Action<string> TellerAssistedAction { get; set; }

		private ICommand _tellerAssistanceCommand;
		public ICommand TellerAssistanceCommand
		{
			get
			{
				if (_tellerAssistanceCommand == null)
				{
					_tellerAssistanceCommand = new DelegateCommand<string>
					(
						x => TellerAssistedAction?.Invoke(x),
						x => TellerAssistanceVisibility == true
					);
				}

				return _tellerAssistanceCommand;
			}
		}

		public Action AccountInquiryAction { get; set; }

		private ICommand _accountInquiryCommand;
		public ICommand AccountInquiryCommand
		{
			get
			{
				if (_accountInquiryCommand == null)
					_accountInquiryCommand = new DelegateCommand(AccountInquiryAction);

				return _accountInquiryCommand;
			}
		}

		public Action CashWithdrawalAction { get; set; }

		private ICommand _cashWithdrawalCommand;
		public ICommand CashWithdrawalCommand
		{
			get
			{
				if (_cashWithdrawalCommand == null)
					_cashWithdrawalCommand = new DelegateCommand(CashWithdrawalAction);
				return _cashWithdrawalCommand;
			}
		}

		public Action FundsTransferAction { get; set; }

		private ICommand _fundsTransferCommand;
		public ICommand FundsTransferCommand
		{
			get
			{
				if (_fundsTransferCommand == null)
					_fundsTransferCommand = new DelegateCommand(FundsTransferAction);
				return _fundsTransferCommand;
			}
		}

		public Action DocumentsAction { get; set; }

		private ICommand _documentsCommand;
		public ICommand DocumentsCommand
		{
			get
			{
				if (_documentsCommand == null)
					_documentsCommand = new DelegateCommand(DocumentsAction);
				return _documentsCommand;
			}
		}

		public Action BillPaymentAction { get; set; }

		private ICommand _billPaymentCommand;
		public ICommand BillPaymentCommand
		{
			get
			{
				if (_billPaymentCommand == null)
					_billPaymentCommand = new DelegateCommand(BillPaymentAction);
				return _billPaymentCommand;
			}
		}

		public Action OtherTransactionsAction { get; set; }

		private ICommand _otherTransactionsCommand;
		public ICommand OtherTransactionsCommand
		{
			get
			{
				if (_otherTransactionsCommand == null)
					_otherTransactionsCommand = new DelegateCommand(OtherTransactionsAction);
				return _otherTransactionsCommand;
			}
		}

		public string AccountNumber { get; set; }
		public string Name { get; set; }
		public double? BalanceAmount { get; set; }

		public List<string> TransactionHistory { get; set; }

		public void Dispose()
		{
			
		}
	}
}