namespace Omnia.Pie.Vtm.Services.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;
    using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ITransactionService
	{
		Task<CashWithdrawal> CashWithdrawalDebitCardAsync(string track2, string pin, string iccRequest, string amount, string accountNumber, string accountType,
					string currencyCode, string convertedAmount, string exchangeRate);
		Task<string> ReverseCashWithdrawalUsingDebitCardAsync(string authCode, string track2, ReversalReason reversalReason);
		Task<List<CreditCardResult>> GetCreditCardsAsync(string customerIdentifier);
		Task<CreditCardDetailResult> GetCreditCardDetailAsync(string cardNumber, string customerIdentifier);
		Task<ExchangeRateResult> ExchangeRateAsync(string fromCurrency, string toCurrency, string transactionAmount, string paymentType);
        Task<CashDepositResult> CashDepositAsync(string userName, List<DepositedDenominations> depositedDenominations, CashDeposited cashDeposited);


        Task<IssueCheckBookResult> IssueChequeBookAsync(string accountBranch, string accountNumber,
					string currencyCode, DateTime issueDate, string numberOfCheques);
		Task<DeliverChequeBookResult> DliverChequeBookAsync(string hostTransCode, string accountBranchCode);
		Task<ApplyStatementChargesResult> ApplyStatementChargesAsync(DateTime statementDateFrom, DateTime statementDateTo,
					string accountNumber, string customerId, string branchCode);
		Task<GetStatementChargesResult> GetStatementChargesAsync(DateTime statementDateFrom, DateTime statementDateTo,
					string chargeIndicator, string accountNumber, string category, string numOfMonths);
		Task<ChequeDepositResult> ChequeDepositAsync(DateTime chequeDate, DateTime settlementDate, string sessionTime,
					string settlementTime, string issuerBankRtn, string accountNumber, string sequenceNumber, string amount,
					string corrIndicator, string preBankRtn, DateTime endorsementDate, string depositIban, string payeeName,
					string frontImage, string backImage, string frontImageLength, string backImageLength);
		Task<GetChequePrintingChargeResult> GetChequePrintingChargeAsync(ChargeIndicator chargeIndicator, string accountNumber, string numberOfLeaf);
		Task<ReverseChargeResult> ReverseChargeAsync(string transactionReferenceNumber, ChargeType chargeType);
		Task<CoordinationNumberResult> CoordinationNumberAsync();
		Task<TransactionNotificationResult> TransactionNotificationAsync(TransactionType notificationType , string transactionMode,
					string cardNumber, string amount, string transactionReference, string accountNumber,string customerIdentifier , Interface.Enums.TransactionStatus transactionStatus = Interface.Enums.TransactionStatus.Successful, string reason = "", string statementMonths = "", string cheaqueLeaves = "", string responseCode = "");
	}
}