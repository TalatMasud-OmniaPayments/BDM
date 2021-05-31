namespace Omnia.Pie.Vtm.Services
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using Omnia.Pie.Vtm.Services.ISO.Customer;
	using Omnia.Pie.Vtm.Services.ISO.Request.Customer;
	using Omnia.Pie.Vtm.Services.ISO.Response.Customer;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class CustomerService : ServiceBase, ICustomerService
	{
		public CustomerService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
		{

		}

        #region "Get Customer Detail"

        public async Task<CustomerDetail> GetCustomerDetail(string Username)
        {
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));

            return await ExecuteFaultHandledOperationAsync<CustomerDetailsRequest, CustomerDetail>(async c =>
            {
                var request = ToDocumentRetrieveCustomerDetailsRequest(Username, Username);

                var response = await GetAllCustomerDetail(request);
                return ToCustomerDetail(response);
            });
        }


        public async Task<CustomerDetail> GetCustomerDetail(string Username, string enquiryAccount)
		{
			if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));
            if (string.IsNullOrEmpty(enquiryAccount)) throw new ArgumentNullException(nameof(enquiryAccount));

            return await ExecuteFaultHandledOperationAsync<CustomerDetailsRequest, CustomerDetail>(async c =>
			{
                var request = ToDocumentRetrieveCustomerDetailsRequest(Username, enquiryAccount);

                var response = await GetAllCustomerDetail(request);
				return ToCustomerDetail(response);
			});
		}

		private async Task<CustomerDetailsResponse> GetAllCustomerDetail(CustomerDetailsRequest request)
			=> await ExecuteServiceAsync<CustomerDetailsRequest, CustomerDetailsResponse>(request);

		private CustomerDetailsRequest ToDocumentRetrieveCustomerDetailsRequest(string Username, string enquiryAccount) => new CustomerDetailsRequest
        {
            Username = Username,
            UserAccount = new EnquiryAccount
            {
                Username = enquiryAccount,   
            },
        };

		private CustomerDetail ToCustomerDetail(CustomerDetailsResponse response) => new CustomerDetail
		{
			Address1 = response?.CustomerDetail?.AddressLine1,
			Address2 = response?.CustomerDetail?.AddressLine2,
			Country = response?.CustomerDetail?.BirthDate,
			Nationality = response?.CustomerDetail?.Nationality,
			FullName = response?.CustomerDetail?.FirstName + " " + response?.CustomerDetail?.LastName,
			VisaRefNumber = response?.CustomerDetail?.VisaRefNumber,
			VisaExpiryDate = ToNullableDateTime(response?.CustomerDetail?.VisaExpiry),
			PassportNumber = response?.CustomerDetail?.PassportNumber,
			PassportExpiryDate = ToNullableDateTime(response?.CustomerDetail?.PassportExpiry),
			Email = response?.CustomerDetail?.Email,
			BirthDate = response?.CustomerDetail?.BirthDate,
			Address3 = response?.CustomerDetail?.AddressLine3,
			Gender = response?.CustomerDetail?.Gender,
			MobileNumber = response?.CustomerDetail?.MobileNumber,
			CustomerCategory = response?.CustomerDetail?.CustomerCategory,
			CustomerStatus = response?.CustomerDetail?.CustomerStatus,
			CustomerInformationFileId = response?.CustomerDetail?.CifId,
			EmiratesId = response?.CustomerDetail?.EmiratesId,
			Salary = response?.CustomerDetail?.Salary,
		};

		#endregion

		#region Loan Accounts

		public async Task<List<LoanAccountResult>> GetLoanAccountsAsync(string customerIdentifier)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<LoanAccountsrequest, List<LoanAccountResult>>(async c =>
			{
				var response = await GetLoanAccountsAsync(ToLoanAccountsRequest(customerIdentifier));
				return ToLoanAccountList(response);
			});
		}

		private async Task<LoanAccountsResponse> GetLoanAccountsAsync(LoanAccountsrequest request)
		{
			return await ExecuteServiceAsync<LoanAccountsrequest, LoanAccountsResponse>(request);
		}

		private LoanAccountsrequest ToLoanAccountsRequest(string customerIdentifier)
		{
			return new LoanAccountsrequest() { CustomerIdentifier = customerIdentifier };
		}

		private List<LoanAccountResult> ToLoanAccountList(LoanAccountsResponse res)
		{
			return res.LoanAccounts?.ConvertAll(ToAccount);
		}

		private LoanAccountResult ToAccount(ISO.Response.Customer.LoanAccount response)
		{
			return new LoanAccountResult
			{
				Balance = response?.Balance,
				Currency = response?.Currency,
				CurrencyCode = response?.CurrencyCode,
				Number = response?.Number,
				Type = response?.Type,
			};
		}

		#endregion

		#region Deposit Accounts

		public async Task<List<DepositAccountResult>> GetDepositAccount(string customerIdentifier)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<DepositAccountRequest, List<DepositAccountResult>>(async c =>
			{
				var response = await GetDepositAccountsAsync(ToDepositAccountsRequest(customerIdentifier));
				//if (response?.ResponseCode == "000")
				//	throw new AuthorizationValidationException("");
				return ToDepositAccountList(response);
			});
		}

		private async Task<DepositAccountResponse> GetDepositAccountsAsync(DepositAccountRequest request)
		{
			return await ExecuteServiceAsync<DepositAccountRequest, DepositAccountResponse>(request);
		}

		private DepositAccountRequest ToDepositAccountsRequest(string customerIdentifier)
		{
			return new DepositAccountRequest() { CustomerIdentifier = customerIdentifier };
		}

		private List<DepositAccountResult> ToDepositAccountList(DepositAccountResponse res)
		{
			return res.DepositAccounts?.ConvertAll(ToDepositAccount);
		}

		private DepositAccountResult ToDepositAccount(DepositAccount response)
		{
			return new DepositAccountResult
			{
				Balance = response?.Balance,
				Currency = response?.Currency,
				CurrencyCode = response?.CurrencyCode,
				Number = response?.Number,
				Type = response?.Type,
			};
		}

		#endregion

		#region "Get Account Detail"

		public async Task<AccountDetailResult> GetAccountDetail(string accountNumber, string customerId = "")
		{
			if (string.IsNullOrEmpty(accountNumber)) throw new ArgumentNullException(nameof(accountNumber));
			//if (string.IsNullOrEmpty(customerId)) throw new ArgumentNullException(nameof(customerId)); // As per george, this is not a required parameter.

			return await ExecuteFaultHandledOperationAsync<AccountDetailRequest, AccountDetailResult>(async c =>
			{
				var response = await GetAccountDetail(ToDocumentVTMRetrieveCustomerDetailsRequest(accountNumber, customerId));
				return ToAccountDetail(response);
			});
		}

        


        private async Task<AccountDetailResponse> GetAccountDetail(AccountDetailRequest request)
			=> await ExecuteServiceAsync<AccountDetailRequest, AccountDetailResponse>(request);

		private AccountDetailRequest ToDocumentVTMRetrieveCustomerDetailsRequest(string accountNumber, string customerId) => new AccountDetailRequest
		{
			AccountNumber = accountNumber,
			CustomerId = customerId
		};

        public async Task<AccountDetailResult> GetAccountDetail(string Username, string CustomerIdentifier, string ConditionId)
        {
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));
            //if (string.IsNullOrEmpty(customerId)) throw new ArgumentNullException(nameof(customerId)); // As per george, this is not a required parameter.

            return await ExecuteFaultHandledOperationAsync<AccountDetailRequest1, AccountDetailResult>(async c =>
            {
                var response = await GetAccountDetail(ToDocumentVTMRetrieveCustomerDetailsRequest(Username, CustomerIdentifier,ConditionId));
                return ToAccountDetail(response);
            });
        }
        private async Task<AccountDetailResponse> GetAccountDetail(AccountDetailRequest1 request)
            => await ExecuteServiceAsync<AccountDetailRequest1, AccountDetailResponse>(request);

        private AccountDetailRequest1 ToDocumentVTMRetrieveCustomerDetailsRequest(string Username, string CustomerIdentifier, string ConditionId) => new AccountDetailRequest1
        {
            Username = Username,
            CustomerIdentifier = CustomerIdentifier,
            ConditionId = ConditionId,
        };

        private AccountDetailResult ToAccountDetail(AccountDetailResponse response) => new AccountDetailResult
		{
			AccountCurrency = response?.AccountDetail?.AccountCurrency,
			AccountNumber = response?.AccountDetail?.AccountNumber,
			AccountStatus = response?.AccountDetail?.AccountStatus,
			AccountTitle = response?.AccountDetail?.AccountTitle,
			AccountType = response?.AccountDetail?.AccountType,
            AccountOpenDate = response?.AccountDetail?.AccountOpenDate,
            AvailableBalance = ToNullableDouble(response?.AccountDetail?.AvailableBalance),
			BranchId = response?.AccountDetail?.BranchId,
			IBAN = response?.AccountDetail?.IBAN
		};

		#endregion

		#region Statement Item

		public async Task<List<Interface.Entities.StatementItem>> GetStatementItemAsync(DateTime statementDateFrom, DateTime statementDateTo, string accountNumber, string numOfTransactions)
		{
			if (statementDateFrom == DateTime.MinValue) throw new ArgumentNullException(nameof(statementDateFrom));
			if (statementDateTo == DateTime.MinValue) throw new ArgumentNullException(nameof(statementDateTo));
			if (string.IsNullOrEmpty(accountNumber)) throw new ArgumentNullException(nameof(accountNumber));

			return await ExecuteFaultHandledOperationAsync<StatementItemRequest, List<Interface.Entities.StatementItem>>(async c =>
			{
                var request = ToStatementItemRequest(ToString(statementDateFrom), ToString(statementDateTo), accountNumber, numOfTransactions);

                var response = await GetStatementItemAsync(request);
				return ToStatementItemList(response);
			});
		}

		private async Task<StatementItemResponse> GetStatementItemAsync(StatementItemRequest request)
		{
			return await ExecuteServiceAsync<StatementItemRequest, StatementItemResponse>(request);
		}

		private StatementItemRequest ToStatementItemRequest(string statementDateFrom, string statementDateTo, string accountNumber, string numOfTransactions)
		{
			return new StatementItemRequest()
			{
				AccountNumber = accountNumber,
				NumOfTransactions = numOfTransactions,
				StatementDateFrom = statementDateFrom,
				StatementDateTo = statementDateTo,
			};
		}

		private List<Interface.Entities.StatementItem> ToStatementItemList(StatementItemResponse res)
		{
			return res.TransactionHistory?.ConvertAll(ToStatementItem);
		}

		private Interface.Entities.StatementItem ToStatementItem(ISO.Response.Customer.StatementItem response)
		{
			var item = new Interface.Entities.StatementItem
			{
				CreditAmount = (response?.TransactionEffect?.ToUpper() == "CR" ? ToNullableDouble(response?.TransactionAmount) : null), // Need to verify the TransactionEffect == CR/DR
				DebitAmount = (response?.TransactionEffect?.ToUpper() == "DR" ? ToNullableDouble(response?.TransactionAmount) : null),
				Description = response?.TransactionNarration,
				RunningBalance = ToNullableDouble(response?.RunningBalance),
				TransactionDate = ToNullableDateTime(response?.TransactionDate, DateFormat.yyyyMMddHHmm),
				ValueDate = ToNullableDateTime(response?.ValueDate, DateFormat.yyyyMMdd)
			};

			return item;
		}

        #endregion



        #region Statement Item

        public async Task<List<Interface.Entities.UserTransaction>> GetUserTransactionsAsync(string Username, DateTime statementDateFrom, DateTime statementDateTo, string accountNumber, string NumOfTransactions , string SortAs)
        {
            if (statementDateFrom == DateTime.MinValue) throw new ArgumentNullException(nameof(statementDateFrom));
            if (statementDateTo == DateTime.MinValue) throw new ArgumentNullException(nameof(statementDateTo));
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));

            return await ExecuteFaultHandledOperationAsync<UserTransactionRequest, List<Interface.Entities.UserTransaction>>(async c =>
            {
                var request = ToUserTransactionsRequest(Username, ToString(statementDateFrom), ToString(statementDateTo), accountNumber, NumOfTransactions, SortAs);

                var response = await GetUserTransactionsAsync(request);
                return ToUserTransactionsList(response);
            });
        }

        private async Task<UserTransactionResponse> GetUserTransactionsAsync(UserTransactionRequest request)
        {
            return await ExecuteServiceAsync<UserTransactionRequest, UserTransactionResponse>(request);
        }

        private UserTransactionRequest ToUserTransactionsRequest(string Username, string statementDateFrom, string statementDateTo, string accountNumber, string NumOfTransactions, string SortAs)
        {
            return new UserTransactionRequest()
            {
                Username = Username,
                StatementDateFrom = statementDateFrom,
                StatementDateTo = statementDateTo,
                AccountNumber = accountNumber,
                NumOfTransactions = NumOfTransactions,
                SortAs = SortAs, 

            };
        }

        private List<Interface.Entities.UserTransaction> ToUserTransactionsList(UserTransactionResponse res)
        {


            res.Transactions = res.Transactions ?? new List<ISO.Response.Customer.UserTransaction>(); 

            return res.Transactions?.ConvertAll(ToUserTransaction);
        }

        private Interface.Entities.UserTransaction ToUserTransaction(ISO.Response.Customer.UserTransaction response)
        {
            var item = new Interface.Entities.UserTransaction
            {
                SessionId = response?.SessionId, // Need to verify the TransactionEffect == CR/DR
                MessageId = response?.MessageId,
                TransactionId = response?.TransactionId,
                BagSerialNo = response?.TransactionId,
                RequestDateTime = ToNullableDateTime(response?.RequestDateTime, DateFormat.yyyyMMddTHHmmss),
                TransactionDateTime = ToNullableDateTime(response?.TransactionDateTime, DateFormat.yyyyMMddTHHmmss),
                CountryCode = response?.CountryCode,
                SplitLength = response?.SplitLength,
                MachineType = response?.Machine_Type,
                SystemFlag = response?.SystemFlag,
                CurrencyCode = response?.CurrencyCode,
                IPAddress = response?.IPAddress,
                SessionLang = response?.SessionLang,
                UpdateBalanceFlag = response?.UpdateBalanceFlag,
                UpdateBalanceRequester = response?.UpdateBalanceRequester,
                UserId = response?.UserId,
                TerminalId = response?.TerminalId,
                BranchId = response?.BranchId,
                AccountNo = response?.AccountNo,
                Amount = response?.Amount,
                DenominationList = response?.DenominationList

            };

            return item;
        }
        
            #endregion

            #region Get Debit Cards

            public async Task<List<Interface.Entities.DebitCard>> GetDebitCardsAsync(string customerIdentifier)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<GetDebitCardsRequest, List<Interface.Entities.DebitCard>>(async c =>
			{
				var response = await GetDebitCardsAsync(ToGetDebitCards(customerIdentifier));
				return ToGetDebitCardList(response);
			});
		}

		private async Task<GetDebitCardsResponse> GetDebitCardsAsync(GetDebitCardsRequest request)
		{
			return await ExecuteServiceAsync<GetDebitCardsRequest, GetDebitCardsResponse>(request);
		}

		private GetDebitCardsRequest ToGetDebitCards(string customerIdentifier)
		{
			return new GetDebitCardsRequest() { CustomerIdentifier = customerIdentifier };
		}

		private List<Interface.Entities.DebitCard> ToGetDebitCardList(GetDebitCardsResponse res)
		{
			return res.DebitCards?.ConvertAll(ToDebitCards);
		}

		private Interface.Entities.DebitCard ToDebitCards(ISO.Response.Customer.DebitCard resp)
		{
			return new Interface.Entities.DebitCard()
			{
				CardNumber = resp?.CardNumber,
				CardStatus = resp?.CardStatus,
				LinkedAccounts = GetLinkedAccounts(resp)
			};
		}

		private List<Interface.Entities.LinkedAccount> GetLinkedAccounts(ISO.Response.Customer.DebitCard item)
		{
			var list = new List<Interface.Entities.LinkedAccount>();

			if (item?.LinkedAccounts != null)
			{
				foreach (var itm in item.LinkedAccounts)
				{
					list.Add(new Interface.Entities.LinkedAccount()
					{
						AccountNumber = itm?.AccountNumber,
						AccountType = itm?.AccountType,
					});
				}
			}

			return list;
		}

		#endregion

		#region "Get Customer Identifier"

		public async Task<CustomerIdentifierResult> GetCustomerIdentifierAsync(string cardNumber, CardType cardType)
		{
			if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentNullException(nameof(cardNumber));

			return await ExecuteFaultHandledOperationAsync<CustomerIdentifierRequest, CustomerIdentifierResult>(async c =>
			{
				var response = await GetCustomerIdentifierAsync(ToCustomerIdentifierRequest(cardNumber, cardType));
				return ToCustomerDetail(response);
			});
		}

		private async Task<CustomerIdentifierResponse> GetCustomerIdentifierAsync(CustomerIdentifierRequest request)
			=> await ExecuteServiceAsync<CustomerIdentifierRequest, CustomerIdentifierResponse>(request);

		private CustomerIdentifierRequest ToCustomerIdentifierRequest(string cardNumber, CardType cardType) => new CustomerIdentifierRequest
		{
			CardNumber = cardNumber,
			CardType = ((int)cardType).ToString(),
		};

		private CustomerIdentifierResult ToCustomerDetail(CustomerIdentifierResponse response) => new CustomerIdentifierResult
		{
			CustomerId = response?.CustomerIdentifier?.CustomerId
		};

		#endregion

		#region "Save Product Info"

		public async Task<ProductInfoResult> ProductInfoAsync(string leadType, string mobileNumber, string email, string language)
		{
			if (string.IsNullOrEmpty(leadType)) throw new ArgumentNullException(nameof(leadType));
			if (string.IsNullOrEmpty(mobileNumber) && string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(mobileNumber));

			return await ExecuteFaultHandledOperationAsync<ProductInfoRequest, ProductInfoResult>(async c =>
			{
				var response = await ProductInfoAsync(ToProductInfoRequest(leadType, mobileNumber, email, language));
				return ToProductInfo(response);
			});
		}

		private async Task<ProductInfoResponse> ProductInfoAsync(ProductInfoRequest request)
			=> await ExecuteServiceAsync<ProductInfoRequest, ProductInfoResponse>(request);

		private ProductInfoRequest ToProductInfoRequest(string leadType, string mobileNumber, string email, string language) => new ProductInfoRequest
		{
			LeadType = leadType,
			MobileNumber = mobileNumber,
			Email = email,
			Language = language,
		};

		private ProductInfoResult ToProductInfo(ProductInfoResponse response) => new ProductInfoResult
		{
			ReferenceNumber = response?.ProductInfo?.ReferenceNumber,
			Status = response?.ProductInfo.Status
		};

		#endregion

		#region GetLinked Accounts

		/*public async Task<List<Account>> GetLinkedAccountAsync(string cardNumber, string customerIdentifier)
		{
			if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentNullException(nameof(cardNumber));
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<GetLinkedAccountRequest, List<Account>>(async c =>
			{
				var response = await GetLinkedAccountAsync(ToLinkedAccountsRequest(cardNumber, customerIdentifier));
				return ToLinkedAccountList(response);
			});
		}

		private async Task<GetLinkedAccountResponse> GetLinkedAccountAsync(GetLinkedAccountRequest request)
		{
			return await ExecuteServiceAsync<GetLinkedAccountRequest, GetLinkedAccountResponse>(request);
		}

		private GetLinkedAccountRequest ToLinkedAccountsRequest(string cardNumber, string customerIdentifier)
		{
			return new GetLinkedAccountRequest() { CardNumber = cardNumber, CustomerIdentifier = customerIdentifier };
		}

		private List<Account> ToLinkedAccountList(GetLinkedAccountResponse res)
		{
			return res.Accounts?.ConvertAll(ToLinkedAccount);
		}

		private Account ToLinkedAccount(Accounts response)
		{
			return new Account
			{
				Name = response?.Name,
				Type = response?.Type,
				TypeCode = response?.TypeCode,
				Currency = response?.Currency,
				CurrencyCode = response?.CurrencyCode,
				Number = response?.Number,
				AvailableBalance = double.Parse(response?.AvailableBalance),
				AccountingUnitId = response?.AccountingUnitId
			};
		}*/

		#endregion
	}
}