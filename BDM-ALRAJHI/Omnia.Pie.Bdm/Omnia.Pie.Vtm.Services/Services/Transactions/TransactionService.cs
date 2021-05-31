namespace Omnia.Pie.Vtm.Services.Transactions
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;
	using Omnia.Pie.Vtm.Services.ISO.Request.Transaction;
	using Omnia.Pie.Vtm.Services.ISO.Response.Transaction;
	using Omnia.Pie.Vtm.Services.ISO.Response.Transaction.ChequeCharges;
	using Omnia.Pie.Vtm.Services.ISO.Transaction;
	using System;
    using System.Linq;

    using System.Collections.Generic;
	using System.Threading.Tasks;

	public class TransactionService : ServiceBase, ITransactionService
	{
		public TransactionService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
		{

		}

		#region Cashwithdrawal

		public async Task<CashWithdrawal> CashWithdrawalDebitCardAsync(string track2, string pin, string iccRequest, string amount, string accountNumber, string accountType,
														  string currencyCode, string convertedAmount, string exchangeRate)
		{
			if (string.IsNullOrEmpty(track2))
				throw new ArgumentNullException(nameof(track2));
			if (string.IsNullOrEmpty(pin))
				throw new ArgumentNullException(nameof(pin));
			//if (string.IsNullOrEmpty(iccRequest)) throw new ArgumentNullException(nameof(iccRequest));
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount));
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));

			amount = ToISOAmount(amount);

			return await ExecuteFaultHandledOperationAsync<CashWithdrawalRequest, CashWithdrawal>(async c =>
			{
				var response = await CashWithdrawalDebitCardAsync(ToCashWithdrawalRequest(track2, pin, iccRequest, amount, accountNumber, accountType, currencyCode,
																					convertedAmount, exchangeRate));
				return ToCashWithdrawal(response);
			});
		}

		private async Task<CashWithdrawalResponse> CashWithdrawalDebitCardAsync(CashWithdrawalRequest request)
			=> await ExecuteServiceAsync<CashWithdrawalRequest, CashWithdrawalResponse>(request);

		private CashWithdrawalRequest ToCashWithdrawalRequest(string track2, string pin, string iccRequest, string transactionAmount, string accountNumber, string accountType,
														  string currencyCode, string convertedAmount, string exchangeRate)
			=> new CashWithdrawalRequest
			{
				Track2 = track2,
				Pin = pin,
				IccRequest = iccRequest,
				TransactionAmount = transactionAmount,
				AccountNumber = accountNumber,

			};

		private CashWithdrawal ToCashWithdrawal(CashWithdrawalResponse response) => new CashWithdrawal
		{
			AuthCode = response?.CashWithdrawal?.AuthCode,
			IccData = response?.CashWithdrawal?.IccResponse
		};

		#endregion

		#region CashWithDrawl Reversal

		public async Task<string> ReverseCashWithdrawalUsingDebitCardAsync(string authCode, string track2, ReversalReason reversalReason)
		{
			if (string.IsNullOrEmpty(authCode))
				throw new ArgumentNullException(nameof(authCode));

			return await ExecuteFaultHandledOperationAsync<CashWithDrawalReversalRequest, string>(async c =>
			{
				var response = await ReverseCashWithdrawalUsingDebitCardAsync(ToReverseCashWithdrawalUsingDebitCardRequest(authCode, track2, reversalReason));
				return ToCashWithdrawal(response);
			});
		}

		private async Task<CashWithdrawalDebitCardReversalResponse> ReverseCashWithdrawalUsingDebitCardAsync(CashWithDrawalReversalRequest request)
			=> await ExecuteServiceAsync<CashWithDrawalReversalRequest, CashWithdrawalDebitCardReversalResponse>(request);

		private CashWithDrawalReversalRequest ToReverseCashWithdrawalUsingDebitCardRequest(string authCode, string track2, ReversalReason reversalReason) => new CashWithDrawalReversalRequest
		{
			AuthCode = authCode,
			Track2 = track2,
			ReversalReason = Convert.ToString(reversalReason)

		};

		private string ToCashWithdrawal(CashWithdrawalDebitCardReversalResponse response) => response?.ResponseCode;


		#endregion

		#region Credit Card

		public async Task<List<CreditCardResult>> GetCreditCardsAsync(string customerIdentifier)
		{
			if (string.IsNullOrEmpty(customerIdentifier))
				throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<CreditCardsRequest, List<CreditCardResult>>(async c =>
			{
				var response = await GetCreditCardAsync(ToCreditCardRequest(customerIdentifier));
				return ToCardImagesList(response);
			});
		}

		private async Task<CreditCardResponse> GetCreditCardAsync(CreditCardsRequest request)
		{
			return await ExecuteServiceAsync<CreditCardsRequest, CreditCardResponse>(request);
		}

		private CreditCardsRequest ToCreditCardRequest(string CustomerIdentifier) => new CreditCardsRequest
		{
			CustomerIdentifier = CustomerIdentifier,
		};

		public List<CreditCardResult> ToCardImagesList(CreditCardResponse res)
			=> res.CreditCards?.ConvertAll(ToCardImage);

		private CreditCardResult ToCardImage(ISO.Response.Transaction.CreditCard response) => new CreditCardResult
		{
			Balance = response?.Balance,
			CardLimit = response?.CardLimit,
			Currency = response?.Currency,
			CurrencyCode = response?.CurrencyCode,
			Number = response?.Number,
			Type = response?.Type,
			StatementMinimumDue = response?.StatementMinimumDue
		};

		#endregion

		#region Credit Card Detail

		public async Task<CreditCardDetailResult> GetCreditCardDetailAsync(string cardNumber, string customerIdentifier)
		{
			if (string.IsNullOrEmpty(cardNumber))
				throw new ArgumentNullException(nameof(cardNumber));

			return await ExecuteFaultHandledOperationAsync<CreditCardDetailRequest, CreditCardDetailResult>(async c =>
			{
				var response = await GetCreditCardDetailAsync(ToDocumentVTMRetrieveCreditCardDetailRequest(cardNumber, customerIdentifier));
				return ToCreditCardDetail(response);
			});
		}

		private async Task<CreditCardDetailResponse> GetCreditCardDetailAsync(CreditCardDetailRequest request)
			=> await ExecuteServiceAsync<CreditCardDetailRequest, CreditCardDetailResponse>(request);

		private CreditCardDetailRequest ToDocumentVTMRetrieveCreditCardDetailRequest(string cardNumber, string customerIdentifier) => new CreditCardDetailRequest
		{
			CardNumber = cardNumber,
			CustomerIdentifier = customerIdentifier,
		};

		private CreditCardDetailResult ToCreditCardDetail(CreditCardDetailResponse response) => new CreditCardDetailResult
		{
			AvailableCreditLimit = response?.CreditCardDetail?.AvailableCreditLimit,
			BilledAmount = response?.CreditCardDetail?.BilledAmount,
			CardNumber = response?.CreditCardDetail?.CardNumber,
			CardStatus = response?.CreditCardDetail?.CardStatus,
			CardType = response?.CreditCardDetail?.CardType,
			CreditCardCategory = response?.CreditCardDetail?.CreditCardCategory,
			CurrencyCode = response?.CreditCardDetail?.CurrencyCode,
			CurrentOutStandingAmount = response?.CreditCardDetail?.CurrentOutStandingAmount,
			MinimumDueAmount = response?.CreditCardDetail?.MinimumDueAmount,
		};

		#endregion

		#region ExchangeResult

		public async Task<ExchangeRateResult> ExchangeRateAsync(string fromCurrency, string toCurrency, string transactionAmount, string paymentType)
		{
			if (string.IsNullOrEmpty(fromCurrency))
				throw new ArgumentNullException(nameof(fromCurrency));
			if (string.IsNullOrEmpty(toCurrency))
				throw new ArgumentNullException(nameof(toCurrency));
			if (string.IsNullOrEmpty(transactionAmount))
				throw new ArgumentNullException(nameof(transactionAmount));
			if (string.IsNullOrEmpty(paymentType))
				throw new ArgumentNullException(nameof(paymentType));

			return await ExecuteFaultHandledOperationAsync<ExchangeRateRequest, ExchangeRateResult>(async c =>
			{
				var response = await ExchangeRateAsync(ToDocumentVTMRetrieveAccountDetailsRequest(fromCurrency, toCurrency, transactionAmount, paymentType));
				return TooExchangeRate(response);
			});
		}

		private async Task<ExchangeRateResponse> ExchangeRateAsync(ExchangeRateRequest request)
			=> await ExecuteServiceAsync<ExchangeRateRequest, ExchangeRateResponse>(request);

		private ExchangeRateRequest ToDocumentVTMRetrieveAccountDetailsRequest(string fromCurrency, string toCurrency, string transactionAmount, string paymentType)
			=> new ExchangeRateRequest
			{
				FromCurrency = fromCurrency,
				PaymentType = paymentType,
				ToCurrency = toCurrency,
				TransactionAmount = transactionAmount,
			};

		private ExchangeRateResult TooExchangeRate(ExchangeRateResponse response) => new ExchangeRateResult
		{
			ExchangeRate = response?.ApplyExchangeRate?.ExchangeRate,
			ExchangeRateCurrency = response?.ApplyExchangeRate?.ExchangeRateCurrency,
		};

		#endregion

		#region CashDeposit

		public async Task<CashDepositResult> CashDepositAsync(string userName, List<DepositedDenominations> depositedDenominations, CashDeposited cashDeposited)
        {

			if (string.IsNullOrEmpty(cashDeposited.CreditAccount))
				throw new ArgumentNullException(nameof(cashDeposited.CreditAccount));
			if (string.IsNullOrEmpty(cashDeposited.CreditAccountCurrency))
				throw new ArgumentNullException(nameof(cashDeposited.CreditAccountCurrency));
			if (string.IsNullOrEmpty(cashDeposited.CreditAmount))
				throw new ArgumentNullException(nameof(cashDeposited.CreditAmount));
			if (string.IsNullOrEmpty(cashDeposited.BagSerialNo))
				throw new ArgumentNullException(nameof(cashDeposited.BagSerialNo));
            if (string.IsNullOrEmpty(cashDeposited.SplitLength))
                throw new ArgumentNullException(nameof(cashDeposited.SplitLength));
            if (string.IsNullOrEmpty(cashDeposited.UpdateBalanceFlag))
                throw new ArgumentNullException(nameof(cashDeposited.UpdateBalanceFlag));
            if (string.IsNullOrEmpty(cashDeposited.MachineType))
                throw new ArgumentNullException(nameof(cashDeposited.MachineType));
            if (string.IsNullOrEmpty(cashDeposited.UpdateBalanceRequester))
                throw new ArgumentNullException(nameof(cashDeposited.UpdateBalanceRequester));
            if (string.IsNullOrEmpty(cashDeposited.SystemFlag))
                throw new ArgumentNullException(nameof(cashDeposited.SystemFlag));

            return await ExecuteFaultHandledOperationAsync<CashDepositRequest, CashDepositResult>(async c =>
			{
                var request = ToCashDepositRequest(userName, depositedDenominations, cashDeposited);

                var response = await CashDepositAsync(request);
				return TooExchangeRate(response);
			});
		}

		private async Task<CashDepositResponse> CashDepositAsync(CashDepositRequest request)
			=> await ExecuteServiceAsync<CashDepositRequest, CashDepositResponse>(request);

		private CashDepositRequest ToCashDepositRequest(string userName, List<DepositedDenominations> depositedDenominations, CashDeposited cashDeposited)
			=> new CashDepositRequest
			{
                Username = userName,
                DepositData = ToDepositData(cashDeposited),
                DenominationList = depositedDenominations.Select(x => new DenominationList()
                {
                    Count = x.Count,
                    Type = x.Type,
                })
                        .ToList(),

            };

		private CashDepositResult TooExchangeRate(CashDepositResponse response) => new CashDepositResult
		{
			//Duplicate = "",
            ResponseCode = response?.ResponseCode,
            MessageId = response?.MessageId,
            SessionId = response?.SessionId,
            TransactionId = response?.TransactionId,
            //HostTransCode = ""
        };

        private DepositData ToDepositData(CashDeposited cashDeposited) => new DepositData
        {
            DebitAccount = cashDeposited.DebitAccount,
            DebitAccountCurrency = cashDeposited.DebitAccountCurrency,
            CreditAccount = cashDeposited.CreditAccount,
            CreditAccountCurrency = cashDeposited.CreditAccountCurrency,
            CreditAmount = cashDeposited.CreditAmount,
            BagSerialNo = cashDeposited.BagSerialNo,
            SplitLength = cashDeposited.SplitLength,
            UpdateBalanceFlag = cashDeposited.UpdateBalanceFlag,
            UpdateBalanceRequester = cashDeposited.UpdateBalanceRequester,
            MachineType = cashDeposited.MachineType,
            SystemFlag = cashDeposited.SystemFlag,
            TransactionDateTime = cashDeposited.TransactionDateTime
        };
       

        #endregion

        #region Issue CheckBook

        public async Task<IssueCheckBookResult> IssueChequeBookAsync(string accountBranch, string accountNumber, string currencyCode, DateTime issueDate, string numberOfCheques)
		{
			if (string.IsNullOrEmpty(accountBranch))
				throw new ArgumentNullException(nameof(accountBranch));
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));
			if (string.IsNullOrEmpty(currencyCode))
				throw new ArgumentNullException(nameof(currencyCode));

			if (string.IsNullOrEmpty(numberOfCheques))
				throw new ArgumentNullException(nameof(numberOfCheques));

			return await ExecuteFaultHandledOperationAsync<IssueCheckBookRequest, IssueCheckBookResult>(async c =>
			{
				var response = await IssueChequeBookAsync(ToIssueChequeBookRequest(accountBranch, accountNumber, currencyCode, ToString(issueDate), numberOfCheques));
				return ToIssueCheckBook(response);
			});
		}

		private async Task<IssueCheckBookResponse> IssueChequeBookAsync(IssueCheckBookRequest request) =>
			await ExecuteServiceAsync<IssueCheckBookRequest, IssueCheckBookResponse>(request);

		private IssueCheckBookRequest ToIssueChequeBookRequest(string accountBranch, string accountNumber, string currencyCode, string issueDate, string numberOfCheques)
			=> new IssueCheckBookRequest
			{
				AccountBranch = accountBranch,
				AccountNumber = accountNumber,
				CurrencyCode = currencyCode,
				IssueDate = issueDate,
				NumberOfCheques = numberOfCheques
			};

		private IssueCheckBookResult ToIssueCheckBook(IssueCheckBookResponse response) => new IssueCheckBookResult
		{
			StartingChequeNumber = response?.ChequeBookIssuance?.StartingChequeNumber,
			HostTransCode = response?.ChequeBookIssuance?.HostTransCode,
			RoutingCode = response?.ChequeBookIssuance?.RoutingCode,
			ReferenceNumber = response?.ChequeBookIssuance?.TransactionReferenceNumber,
		};

		#endregion

		#region Deliver CheckBook

		public async Task<DeliverChequeBookResult> DliverChequeBookAsync(string hostTransCode, string accountBranchCode)
		{
			if (string.IsNullOrEmpty(hostTransCode))
				throw new ArgumentNullException(nameof(hostTransCode));
			if (string.IsNullOrEmpty(accountBranchCode))
				throw new ArgumentNullException(nameof(accountBranchCode));

			return await ExecuteFaultHandledOperationAsync<DeliverChequeBookRequest, DeliverChequeBookResult>(async c =>
			{
				var response = await DliverChequeBookAsync(ToDeliverChequeBookRequest(hostTransCode, accountBranchCode));
				return ToDeliverChequeBook(response);
			});
		}

		private async Task<DeliverChequeBookResponse> DliverChequeBookAsync(DeliverChequeBookRequest request) =>
			await ExecuteServiceAsync<DeliverChequeBookRequest, DeliverChequeBookResponse>(request);

		private DeliverChequeBookRequest ToDeliverChequeBookRequest(string hostTransCode, string accountBranchCode) => new DeliverChequeBookRequest
		{
			AccountBranchCode = accountBranchCode,
			HostTransCode = hostTransCode
		};

		private DeliverChequeBookResult ToDeliverChequeBook(DeliverChequeBookResponse response) => new DeliverChequeBookResult
		{
			ReferenceNumber = response?.DeliveredChequeBook?.ReferenceNumber,
		};

		#endregion

		#region Apply Statement Charges

		public async Task<ApplyStatementChargesResult> ApplyStatementChargesAsync(DateTime statementDateFrom, DateTime statementDateTo, string accountNumber, string customerId, string branchCode)
		{
			if (statementDateFrom == DateTime.MinValue)
				throw new ArgumentNullException(nameof(statementDateFrom));
			if (statementDateTo == DateTime.MinValue)
				throw new ArgumentNullException(nameof(statementDateTo));
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));
			if (string.IsNullOrEmpty(customerId))
				throw new ArgumentNullException(nameof(customerId));

			return await ExecuteFaultHandledOperationAsync<ApplyStatementChargesRequest, ApplyStatementChargesResult>(async c =>
			{
				var response = await ApplyStatementChargesAsync(ToApplyStatementCharges(ToString(statementDateFrom), ToString(statementDateTo), branchCode, accountNumber, customerId));
				return ApplyStatementCharges(response);
			});
		}

		private async Task<ApplyStatementChargesResponse> ApplyStatementChargesAsync(ApplyStatementChargesRequest request) =>
			await ExecuteServiceAsync<ApplyStatementChargesRequest, ApplyStatementChargesResponse>(request);

		private ApplyStatementChargesRequest ToApplyStatementCharges(string statementDateFrom, string statementDateTo, string branchCode, string accountNumber, string customerId) => new ApplyStatementChargesRequest
		{
			StatementDateFrom = statementDateFrom,
			StatementDateTo = statementDateTo,
			BranchCode = branchCode,
			CustomerId = customerId,
			AccountNumber = accountNumber
		};

		private ApplyStatementChargesResult ApplyStatementCharges(ApplyStatementChargesResponse response) => new ApplyStatementChargesResult
		{
			ReferenceNum = response?.ApplyStatementCharge?.ReferenceNum,
			Status = response?.ApplyStatementCharge?.Status
		};

		#endregion

		#region Get Statement Charges

		public async Task<GetStatementChargesResult> GetStatementChargesAsync(DateTime statementDateFrom, DateTime statementDateTo, string chargeIndicator, string accountNumber, string category, string numOfMonths)
		{
			if (statementDateFrom == DateTime.MinValue)
				throw new ArgumentNullException(nameof(statementDateFrom));
			if (statementDateTo == DateTime.MinValue)
				throw new ArgumentNullException(nameof(statementDateTo));
			if (string.IsNullOrEmpty(chargeIndicator))
				throw new ArgumentNullException(nameof(chargeIndicator));
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));
			if (string.IsNullOrEmpty(category))
				throw new ArgumentNullException(nameof(category));
			if (string.IsNullOrEmpty(numOfMonths))
				throw new ArgumentNullException(nameof(numOfMonths));

			return await ExecuteFaultHandledOperationAsync<GetStatementChargesRequest, GetStatementChargesResult>(async c =>
			{
				var response = await GetStatementChargesAsync(ToGetStatementCharges(ToString(statementDateFrom), ToString(statementDateTo), chargeIndicator, accountNumber, category, numOfMonths));
				return GetStatementCharges(response);
			});
		}

		private async Task<GetStatementChargesResponse> GetStatementChargesAsync(GetStatementChargesRequest request) =>
			await ExecuteServiceAsync<GetStatementChargesRequest, GetStatementChargesResponse>(request);

		private GetStatementChargesRequest ToGetStatementCharges(string statementDateFrom, string statementDateTo, string chargeIndicator, string accountNumber, string category, string numOfMonths) => new GetStatementChargesRequest
		{
			StatementDateFrom = statementDateFrom,
			StatementDateTo = statementDateTo,
			ChargeIndicator = chargeIndicator,
			Category = category,
			AccountNumber = accountNumber,
			NumberOfMonths = numOfMonths,
		};

		private GetStatementChargesResult GetStatementCharges(GetStatementChargesResponse response) => new GetStatementChargesResult
		{
			ChargeAmount = response?.Charge?.ChargeAmount,
			ChargeCurrency = response?.Charge?.ChargeCurrency
		};

		#endregion

		#region Cheque Deposit

		public async Task<ChequeDepositResult> ChequeDepositAsync(DateTime chequeDate, DateTime settlementDate, string sessionTime,
			string settlementTime, string issuerBankRtn, string accountNumber, string sequenceNumber, string amount,
			string corrIndicator, string preBankRtn, DateTime endorsementDate, string depositIban, string payeeName,
			string frontImage, string backImage, string frontImageLength, string backImageLength)
		{
			if (chequeDate == DateTime.MinValue)
				throw new ArgumentNullException(nameof(chequeDate));               // 2017-01-15
			if (settlementDate == DateTime.MinValue)
				throw new ArgumentNullException(nameof(settlementDate));       // Todays date with same format
			if (string.IsNullOrEmpty(sessionTime))
				throw new ArgumentNullException(nameof(sessionTime));            // current time hh:mm:ss
			if (string.IsNullOrEmpty(settlementTime))
				throw new ArgumentNullException(nameof(settlementTime));      // current time hh:mm:ss
			if (string.IsNullOrEmpty(issuerBankRtn))
				throw new ArgumentNullException(nameof(issuerBankRtn));        // from MICR (length 9)
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));        // account number on cheque (MICR)
			if (string.IsNullOrEmpty(sequenceNumber))
				throw new ArgumentNullException(nameof(sequenceNumber));      // from MICR (length 6)
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException(nameof(amount));                      // Amount on cheque
			if (string.IsNullOrEmpty(corrIndicator))
				throw new ArgumentNullException(nameof(corrIndicator));        // 0
			if (string.IsNullOrEmpty(preBankRtn))
				throw new ArgumentNullException(nameof(preBankRtn));              // from MICR (length 9) need to confirm
			if (endorsementDate == DateTime.MinValue)
				throw new ArgumentNullException(nameof(endorsementDate));       // Todays date with same format
			if (string.IsNullOrEmpty(depositIban))
				throw new ArgumentNullException(nameof(depositIban));            // Payee IBAN
			if (string.IsNullOrEmpty(payeeName))
				throw new ArgumentNullException(nameof(payeeName));                // payee name
			if (string.IsNullOrEmpty(frontImage))
				throw new ArgumentNullException(nameof(frontImage));              // b64 string
			if (string.IsNullOrEmpty(backImage))
				throw new ArgumentNullException(nameof(backImage));                // b64 string
			if (string.IsNullOrEmpty(frontImageLength))
				throw new ArgumentNullException(nameof(frontImageLength));  // length of front image string
			if (string.IsNullOrEmpty(backImageLength))
				throw new ArgumentNullException(nameof(backImageLength));    // length of back image string

			return await ExecuteFaultHandledOperationAsync<ChequeDepositRequest, ChequeDepositResult>(async c =>
			{
				var response = await ChequeDepositAsync(ToChequeDeposit(ToString(chequeDate, DateFormat.yyyyMMddDashed), ToString(settlementDate, DateFormat.yyyyMMddDashed), sessionTime,
			settlementTime, issuerBankRtn, accountNumber, sequenceNumber, amount, corrIndicator, preBankRtn, ToString(endorsementDate, DateFormat.yyyyMMddDashed), depositIban, payeeName,
			frontImage, backImage, frontImageLength, backImageLength));
				return ChequeDeposit(response);
			});
		}

		private async Task<ChequeDepositResponse> ChequeDepositAsync(ChequeDepositRequest request) =>
			await ExecuteServiceAsync<ChequeDepositRequest, ChequeDepositResponse>(request);

		private ChequeDepositRequest ToChequeDeposit(string chequeDate, string settlementDate, string sessionTime,
			string settlementTime, string issuerBankRtn, string accountNumber, string sequenceNumber, string amount,
			string corrIndicator, string preBankRtn, string endorsementDate, string depositIban, string payeeName,
			string frontImage, string backImage, string frontImageLength, string backImageLength) => new ChequeDepositRequest
			{
				ChequeDate = chequeDate,
				SettlementDate = settlementDate,
				SessionTime = sessionTime,
				SettlementTime = settlementTime,
				IssuerBankRtn = issuerBankRtn,
				AccountNumber = accountNumber,
				SequenceNumber = sequenceNumber,
				Amount = amount,
				CorrIndicator = corrIndicator,
				PreBankRtn = preBankRtn,
				EndorsementDate = endorsementDate,
				DepositIban = depositIban,
				PayeeName = payeeName,
				FrontImage = frontImage,
				BackImage = backImage,
				FrontImageLength = frontImageLength,
				BackImageLength = backImageLength
			};

		private ChequeDepositResult ChequeDeposit(ChequeDepositResponse response) => new ChequeDepositResult
		{
			ReferenceNumber = response?.ChequeDeposit?.ReferenceNumber,
			Status = response?.ChequeDeposit?.Status
		};

		#endregion

		#region Get Cheque Printing Charge

		public async Task<GetChequePrintingChargeResult> GetChequePrintingChargeAsync(ChargeIndicator chargeIndicator, string accountNumber, string numberOfLeaf)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException(nameof(accountNumber));
			if (string.IsNullOrEmpty(numberOfLeaf))
				throw new ArgumentNullException(nameof(numberOfLeaf));

			return await ExecuteFaultHandledOperationAsync<GetChequePrintingChargeRequest, GetChequePrintingChargeResult>(async c =>
			{
				var response = await GetChequePrintingChargeAsync(ToGetChequePrintingCharges(chargeIndicator, accountNumber, numberOfLeaf));
				return GetChequePrintingCharges(response);
			});
		}

		private async Task<GetChequePrintingChargeResponse> GetChequePrintingChargeAsync(GetChequePrintingChargeRequest request) =>
			await ExecuteServiceAsync<GetChequePrintingChargeRequest, GetChequePrintingChargeResponse>(request);

		private GetChequePrintingChargeRequest ToGetChequePrintingCharges(ChargeIndicator chargeIndicator, string accountNumber, string numberOfLeaf) => new GetChequePrintingChargeRequest
		{
			NumberOfLeafs = numberOfLeaf,
			ChargeIndicator = ((int)chargeIndicator).ToString(),
			AccountNumber = accountNumber
		};

		private GetChequePrintingChargeResult GetChequePrintingCharges(GetChequePrintingChargeResponse response) => new GetChequePrintingChargeResult
		{
			ChargeAmount = ToNullableDouble(response?.Charge?.ChargeAmount),
			ChargeCurrency = response?.Charge?.ChargeCurrency
		};

		#endregion

		#region Reverse Charge

		public async Task<ReverseChargeResult> ReverseChargeAsync(string transactionReferenceNumber, ChargeType chargeType)
		{
			if (string.IsNullOrEmpty(transactionReferenceNumber))
				throw new ArgumentNullException(nameof(transactionReferenceNumber));

			return await ExecuteFaultHandledOperationAsync<ReverseChargeRequest, ReverseChargeResult>(async c =>
			{
				var response = await ReverseChargeAsync(ToGetReverseCharge(transactionReferenceNumber, chargeType));
				return GetReverseCharge(response);
			});
		}

		private async Task<ReverseChargeResponse> ReverseChargeAsync(ReverseChargeRequest request) =>
			await ExecuteServiceAsync<ReverseChargeRequest, ReverseChargeResponse>(request);

		private ReverseChargeRequest ToGetReverseCharge(string transactionReferenceNumber, ChargeType chargeType) => new ReverseChargeRequest
		{
			ChargeType = Convert.ToString(chargeType),
			TransactionReferenceNumber = transactionReferenceNumber
		};

		private ReverseChargeResult GetReverseCharge(ReverseChargeResponse response) => new ReverseChargeResult
		{
			ReferenceNumber = response?.ReverseChargesRsp?.ReferenceNumber
		};

		#endregion

		#region Get Coordination Number

		public async Task<CoordinationNumberResult> CoordinationNumberAsync()
		{
			return await ExecuteFaultHandledOperationAsync<CoordinationNumberRequest, CoordinationNumberResult>(async c =>
			{
				var response = await CoordinationNumberAsync(ToCoordinationNumber());
				return GetCoordinationNumber(response);
			});
		}

		private async Task<CoordinationNumberResponse> CoordinationNumberAsync(CoordinationNumberRequest request) =>
			await ExecuteServiceAsync<CoordinationNumberRequest, CoordinationNumberResponse>(request);

		private CoordinationNumberRequest ToCoordinationNumber() => new CoordinationNumberRequest
		{

		};

		private CoordinationNumberResult GetCoordinationNumber(CoordinationNumberResponse response) => new CoordinationNumberResult
		{
			Number = response?.CoordinationNumber?.Number
		};

		#endregion

		#region Transaction Notification

		public async Task<TransactionNotificationResult> TransactionNotificationAsync(Interface.TransactionType notificationType, string transactionMode,
					string cardNumber, string amount, string transactionReference, string accountNumber, string customerIdentifier, Interface.Enums.TransactionStatus transactionStatus = Interface.Enums.TransactionStatus.Successful, string reason = "", string statementMonths = "", string cheaqueLeaves = "", string responseCode = "")
		{
			if (string.IsNullOrEmpty(transactionMode))
				throw new ArgumentNullException(nameof(transactionMode));

			return await ExecuteFaultHandledOperationAsync<TransactionNotificationRequest, TransactionNotificationResult>(async c =>
			{
				var response = await TransactionNotificationAsync(ToTransactionNotification(notificationType, transactionMode,
					cardNumber, amount, transactionReference, accountNumber, customerIdentifier, transactionStatus, reason, statementMonths, cheaqueLeaves, responseCode));
				return TransactionNotification(response);
			});
		}

		private async Task<TransactionNotificationResponse> TransactionNotificationAsync(TransactionNotificationRequest request) =>
			await ExecuteServiceAsync<TransactionNotificationRequest, TransactionNotificationResponse>(request);

		private TransactionNotificationRequest ToTransactionNotification(Interface.TransactionType notificationType, string transactionMode,
			string cardNumber, string amount, string transactionReference, string accountNumber, string customerIdentifier, Interface.Enums.TransactionStatus transactionStatus, string reason, string statementMonths, string cheaqueLeaves, string responseCode) => new TransactionNotificationRequest
			{
				AccountNumber = accountNumber,
				Amount = amount,
				CardNumber = cardNumber,
				TransactionType = notificationType,
				TransactionReference = transactionReference,
				TransactionMode = transactionMode,
				CustomerIdentifier = customerIdentifier,
				TransactionStatus = transactionStatus,
				Reason = reason,
				StatementMonths = statementMonths,
				ChequeLeaves = cheaqueLeaves,
				ResponseCode = responseCode
			};

		private TransactionNotificationResult TransactionNotification(TransactionNotificationResponse response) => new TransactionNotificationResult
		{
			ReferenceNumber = response?.TransactionNotification?.ReferenceNumber,
			Status = response?.TransactionNotification?.Status
		};

		#endregion


	}
}