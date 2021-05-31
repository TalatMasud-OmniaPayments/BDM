namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Input;

	public interface IAuthenticatedDebitCardStandbyViewModel : IExpirableBaseViewModel
	{

		bool IsSelfModeCalling { get; set; }

		string Name { get; set; }
		string AccountNumber { get; set; }
		double? BalanceAmount { get; set; }

		List<string> TransactionHistory { get; set; }

		Action AccountInquiryAction { get; set; }
		ICommand AccountInquiryCommand { get; }

		Action CashWithdrawalAction { get; set; }
		ICommand CashWithdrawalCommand { get; }

		Action FundsTransferAction { get; set; }
		ICommand FundsTransferCommand { get; }

		Action DocumentsAction { get; set; }
		ICommand DocumentsCommand { get; }

		Action BillPaymentAction { get; set; }
		ICommand BillPaymentCommand { get; }

		Action OtherTransactionsAction { get; set; }
		ICommand OtherTransactionsCommand { get; }

	}
}