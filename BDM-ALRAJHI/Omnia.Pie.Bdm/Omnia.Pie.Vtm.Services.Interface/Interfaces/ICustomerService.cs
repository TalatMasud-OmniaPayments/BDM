namespace Omnia.Pie.Vtm.Services.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ICustomerService
	{
		Task<CustomerDetail> GetCustomerDetail(string customerIdentifier);
        Task<CustomerDetail> GetCustomerDetail(string Username, string enquiryAccount);

        Task<List<LoanAccountResult>> GetLoanAccountsAsync(string customerIdentifier);
		Task<List<DepositAccountResult>> GetDepositAccount(string customerIdentifier);
		Task<AccountDetailResult> GetAccountDetail(string accountNumber, string customerId = "");
        Task<AccountDetailResult> GetAccountDetail(string Username, string CustomerIdentifier, string ConditionId);

        Task<List<StatementItem>> GetStatementItemAsync(DateTime statementDateFrom, DateTime statementDateTo, string accountNumber, string numOfTransactions);
        Task<List<Interface.Entities.UserTransaction>> GetUserTransactionsAsync(string Username, DateTime statementDateFrom, DateTime statementDateTo, string accountNumber, string NumOfTransactions, string SortAs);
        Task<List<DebitCard>> GetDebitCardsAsync(string CustomerIdentifier);
		Task<CustomerIdentifierResult> GetCustomerIdentifierAsync(string cardNumber, CardType cardType);
		Task<ProductInfoResult> ProductInfoAsync(string leadType, string mobileNumber,string email, string language);
		//Task<List<Account>> GetLinkedAccountAsync(string cardNumber, string customerIdentifier);
	}

	public enum CardType
	{
		CreditCardDebitCard = 1,
		EmiratesIdCard = 2,
	}
}