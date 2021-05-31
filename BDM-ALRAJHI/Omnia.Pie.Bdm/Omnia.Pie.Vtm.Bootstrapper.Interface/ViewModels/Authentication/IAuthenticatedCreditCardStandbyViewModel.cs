namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface IAuthenticatedCreditCardStandbyViewModel : IExpirableBaseViewModel
	{

		bool IsSelfModeCalling { get; set; }

		bool BalanceInquiryVisibility { get; set; }
		bool CashDepositToCardVisibility { get; set; }

		Action BalanceInquiryAction { get; set; }
		ICommand BalanceInquiryCommand { get; }

		Action CashWithdrawalAction { get; set; }
		ICommand CashWithdrawalCommand { get; }

		Action CashDepositToCardAction { get; set; }
		ICommand CashDepositToCardCommand { get; }		
	}
}