namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using System;
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;

	public class AuthenticatedCreditCardStandbyViewModel : ExpirableBaseViewModel, IAuthenticatedCreditCardStandbyViewModel
	{
		public bool TellerAssistanceVisibility { get; set; } = false;
		public bool IsSelfModeCalling { get; set; } = false;

		public bool BalanceInquiryVisibility { get; set; }
		public bool CashDepositToCardVisibility { get; set; }

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

		public Action BalanceInquiryAction { get; set; }

		private ICommand _balanceInquiryCommand;
		public ICommand BalanceInquiryCommand
		{
			get
			{
				if (_balanceInquiryCommand == null)
					_balanceInquiryCommand = new DelegateCommand(BalanceInquiryAction);

				return _balanceInquiryCommand;
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

		public Action CashDepositToCardAction { get; set; }

		private ICommand _cashDepositToCardCommand;
		public ICommand CashDepositToCardCommand
		{
			get
			{
				if (_cashDepositToCardCommand == null)
					_cashDepositToCardCommand = new DelegateCommand(CashDepositToCardAction);
				return _cashDepositToCardCommand;
			}
		}

		public void Dispose()
		{

		}
	}
}