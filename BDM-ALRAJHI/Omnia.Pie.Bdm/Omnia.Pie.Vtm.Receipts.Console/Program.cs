using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Framework.Interface.Configuration;
using Omnia.Pie.Vtm.Framework.Configurations;
//using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Receipts.Console
{
	class Program
	{
		static IReceiptFormatter ReceiptFormatter { get; set; }

		static void Main(string[] args)
		{
			Do();
			System.Console.ReadLine();
		}

		static async void Do()
		{
			var container = new UnityContainer();
			container.LoadConfiguration();

			ReceiptFormatter = container.Resolve<IReceiptFormatter>();

			ReceiptFormatter.Metadata = new ReceiptMetadata
			{
				TerminalId = "FGBV1001",
				TerminalLocation = "Gulf Data Hub",
				OwnerName = "FGB"
			};
            await ExecuteAsync(GetBalanceInquiryReceiptAsync);
            await ExecuteAsync(GetUpdateInfoReceiptAsync);
            await ExecuteAsync(GetRegisterFingerprintReceiptAsync);
            await ExecuteAsync(GetTransferFundsReceiptAsync);
			await ExecuteAsync(GetCreditCardPaymentReceiptAsync);
			await ExecuteAsync(GetCreditCardPaymentDeclinedReceiptAsync);
			await ExecuteAsync(GetCreditCardBalanceReceiptAsync);
			await ExecuteAsync(GetCashWithdrawalUsingDebitCardReceiptAsync);
			await ExecuteAsync(GetCashWithdrawalUsingDebitCardEmiratesIdReceiptAsync);
			await ExecuteAsync(GetCashWithdrawalUsingCreditCardReceiptAsync);
			await ExecuteAsync(GetCashDepositToAccountReceiptAsync);
			await ExecuteAsync(GetCashDepositToCreditCardReceiptAsync);
			await ExecuteAsync(GetMinistatementReceiptAsync);
			await ExecuteAsync(GetCardlessCashWithdrawalReceiptAsync);
			await ExecuteAsync(GetChequeBookRequestReceiptAsync);
			await ExecuteAsync(GetUpdateCustomerDetailsMobileReceiptAsync);
			await ExecuteAsync(GetUpdateCustomerDetailsAddressReceiptAsync);
			await ExecuteAsync(GetUpdateCustomerDetailsEmailReceiptAsync);
			await ExecuteAsync(GetUpdateCustomerDetailsEmiratesIdReceiptAsync);
			await ExecuteAsync(GetOffusCardBalanceReceiptAsync);
			await ExecuteAsync(GetOffusCardBalanceDeclinedReceiptAsync);
			await ExecuteAsync(GetCashWithdrawalUsingOffusCardReceiptAsync);
			await ExecuteAsync(GetCashWithdrawalUsingOffusCardDeclinedReceiptAsync);
			await ExecuteAsync(GetChequeDepositToAccountReceiptHeaderAsync);
			await ExecuteAsync(GetChequeDepositToAccountReceiptFooterAsync);
			await ExecuteAsync(GetChequeDepositToCreditCardReceiptHeaderAsync);
			await ExecuteAsync(GetChequeDepositToCreditCardReceiptFooterAsync);

			// Billers
			await ExecuteAsync(GetDewaAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetSewaAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetAddcAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetAadcAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetSalikAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetDuPostpaidAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetDuPrepaidAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatPostpaidAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatWaselAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatElifeAccountBillPaymentReceiptAsync);
			await ExecuteAsync(GetDewaCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetSewaCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetAddcCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetAadcCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetSalikCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetDuPostpaidCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetDuPrepaidCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatPostpaidCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatWaselCreditCardBillPaymentReceiptAsync);
			await ExecuteAsync(GetEtisalatElifeCreditCardBillPaymentReceiptAsync);

			// Supervisor
			await ExecuteAsync(GetClearCardReceiptAsync);
			await ExecuteAsync(GetViewClearCardReceiptAsync);
			await ExecuteAsync(GetClearChequesReceiptAsync);
			await ExecuteAsync(GetViewClearChequesReceiptAsync);
			await ExecuteAsync(GetClearAddCashReceiptAsync);
			await ExecuteAsync(GetClearCashInReceiptAsync);
			await ExecuteAsync(GetViewClearCashInReceiptAsync);
			await ExecuteAsync(GetCashInReceiptAsync);
			await ExecuteAsync(GetAddCashReceiptAsync);
			await ExecuteAsync(GetViewAddCashReceiptAsync);
			await ExecuteAsync(GetSuccessTestCashReceiptAsync);
			await ExecuteAsync(GetFailedTestCashReceiptAsync);
		}
        
        static async Task<string> GetBalanceInquiryReceiptAsync() => await ReceiptFormatter.FormatAsync(new BalanceInquiryReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			Currency = TerminalConfiguration.Section?.Currency,
            AvailableBalance = 1023.4598,
			TransactionStatus = TransactionStatus.Succeeded
		});
        static async Task<string> GetUpdateInfoReceiptAsync() => await ReceiptFormatter.FormatAsync(new UpdateInfoReceipt
        {
            userName = "testuser",
            email = "john.wick@alrajhibank.com",
            mobile = "1234567890"

        });
        static async Task<string> GetRegisterFingerprintReceiptAsync() => await ReceiptFormatter.FormatAsync(new RegisterFingerprintReceipt
        {
            userName = "testuser",
            email = "john.wick@alrajhibank.com",
            mobile = "1234567890"

        });

        static async Task<string> GetTransferFundsReceiptAsync() => await ReceiptFormatter.FormatAsync(new TransferFundsReceipt
		{
			IsEmiratesIdAuthentication = false,
			CardNumber = "1234567887654321",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			ReferenceNumber = "FT123456",
			SourceAccountCurrency = TerminalConfiguration.Section?.Currency,
			SourceAccountNumber = "8765432112345678",
			TargetAccountNumber = "1111432112349999",
			TransactionAmount = 1023.4598,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetCreditCardPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new CreditCardPaymentReceipt
		{
			IsEmiratesIdAuthentication = true,
			CreditCardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ReferenceNumber = "FT123456",
			Currency = TerminalConfiguration.Section?.Currency,
            Amount = 128.8723,
			AvailableBalance = 1023.4598,
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetCreditCardPaymentDeclinedReceiptAsync() => await ReceiptFormatter.FormatAsync(new CreditCardPaymentReceipt
		{
			IsEmiratesIdAuthentication = false,
			CreditCardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ReferenceNumber = "FT123456",
			Currency = TerminalConfiguration.Section?.Currency,
            Amount = 128.8723,
			AvailableBalance = 1023.4598,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetCreditCardBalanceReceiptAsync() => await ReceiptFormatter.FormatAsync(new CreditCardBalanceReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			Currency = TerminalConfiguration.Section?.Currency,
            AvailableLimit = 1023.4598
		});

		static async Task<string> GetCashWithdrawalUsingDebitCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashWithdrawalUsingDebitCardReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Succeeded,
			AuthCode = "111222",
			AccountCurrency = "USD",
			AvailableBalance = 1023.4598,
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
		});

		static async Task<string> GetCashWithdrawalUsingDebitCardEmiratesIdReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashWithdrawalUsingDebitCardReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Succeeded,
			AuthCode = "111222",
			AccountCurrency = "USD",
			AvailableBalance = 1023.4598,
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			IsEmiratesIdAuthentication = true
		});

		static async Task<string> GetCashWithdrawalUsingCreditCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashWithdrawalUsingCreditCardReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Succeeded,
			AuthCode = "111222",
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
			TransactionAmount = 128.8723,
		});

		static async Task<string> GetCashDepositToAccountReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashDepositToAccountReceipt
		{
			UserId = "1234567887654321",
			AccountNumber = "8765432112345678",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			AuthCode = "111222",
			Currency = TerminalConfiguration.Section?.Currency,
            Denominations = new List<Denomination>()
			{
				new Denomination { Value = 100, Count = 3 },
				new Denomination { Value = 500, Count = 2 },
				new Denomination { Value = 1000, Count = 1 },
			},
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetCashDepositToCreditCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashDepositToCreditCardReceipt
		{
			CardNumber = "1234567887654321",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			AuthCode = "111222",
			Currency = TerminalConfiguration.Section?.Currency,
            Denominations = new List<Denomination>()
			{
				new Denomination { Value = 100, Count = 3 },
				new Denomination { Value = 500, Count = 2 },
				new Denomination { Value = 1000, Count = 1 },
			},
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetMinistatementReceiptAsync() => await ReceiptFormatter.FormatAsync(new MinistatementReceipt
		{
			IsEmiratesIdAuthentication = false,
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			Currency = TerminalConfiguration.Section?.Currency,
            Items = new List<MinistatementItem>()
			{
				new MinistatementItem { PostingDate = DateTime.Today, Description = "Record 1", CreditAmount = 342.234 },
				new MinistatementItem { PostingDate = DateTime.Today.AddDays(1), Description = "Record 2", DebitAmount = 345.234 },
				new MinistatementItem { PostingDate = DateTime.Today.AddDays(2), Description = "Record 3", CreditAmount = 567.234 },
				new MinistatementItem { PostingDate = DateTime.Today.AddDays(3), Description = "Record 4" },
				new MinistatementItem { PostingDate = DateTime.Today.AddDays(4) },
				new MinistatementItem { Description = "Record 5" },
			},
			AvailableBalance = 2498.4853
		});

		static async Task<string> GetCardlessCashWithdrawalReceiptAsync() => await ReceiptFormatter.FormatAsync(new CardlessCashWithdrawalReceipt
		{
			MobileNumber = "1234567887",
			TransactionNumber = "4567",
			AuthCode = "111222",
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetChequeBookRequestReceiptAsync() => await ReceiptFormatter.FormatAsync(new ChequeBookRequestReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetUpdateCustomerDetailsMobileReceiptAsync() => await ReceiptFormatter.FormatAsync(new UpdateCustomerDetailsMobileReceipt
		{
			IsEmiratesIdAuthentication = true,
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0526933490",
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetUpdateCustomerDetailsAddressReceiptAsync() => await ReceiptFormatter.FormatAsync(new UpdateCustomerDetailsAddressReceipt
		{
			IsEmiratesIdAuthentication = false,
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			Address = "Dubai",
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetUpdateCustomerDetailsEmailReceiptAsync() => await ReceiptFormatter.FormatAsync(new UpdateCustomerDetailsEmailReceipt
		{
			IsEmiratesIdAuthentication = false,
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			Email = "john_doe@email.com",
			TransactionStatus = TransactionStatus.HardwareFailure
		});

		static async Task<string> GetUpdateCustomerDetailsEmiratesIdReceiptAsync() => await ReceiptFormatter.FormatAsync(new UpdateCustomerDetailsEmiratesIdReceipt
		{
			IsEmiratesIdAuthentication = false,
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			EmiratesIdNumber = "123456789012345698",
			EmiratesIdExpiryDate = DateTime.Today.AddYears(10),
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetDewaAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DewaAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetSewaAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new SewaAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetAddcAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new AddcAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetAadcAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new AadcAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetSalikAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new SalikAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetDuPostpaidAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DuPostpaidAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetDuPrepaidAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DuPrepaidAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.HardwareFailure
		});

		static async Task<string> GetEtisalatPostpaidAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatPostpaidAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetEtisalatWaselAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatWaselAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetEtisalatElifeAccountBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatElifeAccountBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			AccountNumber = "8765432112345678",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetDewaCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DewaCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
			AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetSewaCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new SewaCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetAddcCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new AddcCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetAadcCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new AadcCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723
		});

		static async Task<string> GetSalikCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new SalikCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ConsumerNumber = "1234567890",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetDuPostpaidCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DuPostpaidCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetDuPrepaidCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new DuPrepaidCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetEtisalatPostpaidCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatPostpaidCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            AmountDue = 1023.4598,
			TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetEtisalatWaselCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatWaselCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetEtisalatElifeCreditCardBillPaymentReceiptAsync() => await ReceiptFormatter.FormatAsync(new EtisalatElifeCreditCardBillPaymentReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			MobileNumber = "0501234567",
			Currency = TerminalConfiguration.Section?.Currency,
            TransactionAmount = 128.8723,
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetOffusCardBalanceReceiptAsync() => await ReceiptFormatter.FormatAsync(new OffusCardBalanceReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			Currency = TerminalConfiguration.Section?.Currency,
            AvailableBalance = 1023.4598,
			ApplicationId = "A0000000032010",
			ApplicationLabel = "VISA ELECTRONIC",
			IsUaeSwitchTransaction = true,
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetOffusCardBalanceDeclinedReceiptAsync() => await ReceiptFormatter.FormatAsync(new OffusCardBalanceReceipt
		{
			CardNumber = "1234567887654321",
			TransactionNumber = "4567",
			ApplicationId = "A0000000032010",
			ApplicationLabel = "VISA ELECTRONIC",
			TransactionStatus = TransactionStatus.Declined
		});

		static async Task<string> GetCashWithdrawalUsingOffusCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashWithdrawalUsingOffusCardReceipt
		{
			AuthCode = "111222",
			CardNumber = "1234567887654321",
			TransactionAmount = 1500,
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
            TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Succeeded,
			AvailableBalance = 100023.4598,
			AvailableBalanceCurrency = "USD",
			ApplicationId = "A0000000032010",
			ApplicationLabel = "VISA ELECTRONIC",
			IsUaeSwitchTransaction = true
		});

		static async Task<string> GetCashWithdrawalUsingOffusCardDeclinedReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashWithdrawalUsingOffusCardReceipt
		{
			AuthCode = "111222",
			CardNumber = "1234567887654321",
			TransactionAmount = 1500,
			TransactionCurrency = TerminalConfiguration.Section?.Currency,
            TransactionNumber = "4567",
			TransactionStatus = TransactionStatus.Declined,
			AvailableBalance = 1023.4598,
			AvailableBalanceCurrency = "USD",
			ApplicationId = "A0000000032010",
			ApplicationLabel = "VISA ELECTRONIC",
			IsUaeSwitchTransaction = true,
			IsInsufficientBalance = true
		});

		static async Task<string> GetClearCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearCardsReceipt
		{
			IsView = false,
			Units = Enumerable.Range(1, 5).Select(i => new CardUnit
			{
				Name = $"Unit {i}",
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetViewClearCardReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearCardsReceipt
		{
			IsView = true,
			Units = Enumerable.Range(1, 5).Select(i => new CardUnit
			{
				Name = $"Unit {i}",
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetClearChequesReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearChequesReceipt
		{
			IsView = false,
			Units = Enumerable.Range(1, 5).Select(i => new ChequeUnit
			{
				Name = $"Unit {i}",
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetViewClearChequesReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearChequesReceipt
		{
			IsView = true,
			Units = Enumerable.Range(1, 5).Select(i => new ChequeUnit
			{
				Name = $"Unit {i}",
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetClearAddCashReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearAddCashReceipt
		{
			Units = Enumerable.Range(1, 5).Select(i => new ClearAddCashUnit
			{
				Name = $"Unit {i}",
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Denomination = 100 * i,
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetClearCashInReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearCashInReceipt
		{
			IsView = false,
			CashInUnits = Enumerable.Range(1, 5).Select(i => new CashInUnit
			{
				Name = $"Unit {i}",
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Count = i * 2
			}).ToList(),

			DenominationRecords = Enumerable.Range(1, 5).Select(i => new DenominationRecord
			{
				Value = i * 100,
				Count = i * 2,
				Retracted = i,
				Rejected = i + 1,
				Total = 2 * i + 1
			}).ToList()
		});

		static async Task<string> GetViewClearCashInReceiptAsync() => await ReceiptFormatter.FormatAsync(new ClearCashInReceipt
		{
			IsView = true,
			CashInUnits = Enumerable.Range(1, 5).Select(i => new CashInUnit
			{
				Name = $"Unit {i}",
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Count = i * 2
			}).ToList(),

			DenominationRecords = Enumerable.Range(1, 5).Select(i => new DenominationRecord
			{
				Value = i * 100,
				Count = i * 2,
				Retracted = i,
				Rejected = i + 1,
				Total = 2 * i + 1
			}).ToList()
		});

		static async Task<string> GetCashInReceiptAsync() => await ReceiptFormatter.FormatAsync(new CashInReceipt
		{
			Type = "Cash",
			Units = Enumerable.Range(1, 5).Select(i => new OldCashInUnit
			{
				Index = i,
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Denomination = i * 100,
				Count = i * 2,
				Retracted = i
			}).ToList()
		});

		static async Task<string> GetAddCashReceiptAsync() => await ReceiptFormatter.FormatAsync(new AddCashReceipt
		{
			Units = Enumerable.Range(1, 5).Select(i => new AddCashUnit
			{
				Name = $"Unit {i}",
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Denomination = 100 * i,
				Count = i * 2
			}).ToList()
		});

		static async Task<string> GetFailedTestCashReceiptAsync() => await ReceiptFormatter.FormatAsync(new TestCashReceipt
		{
			IsSuccess = false
		});

		static async Task<string> GetSuccessTestCashReceiptAsync() => await ReceiptFormatter.FormatAsync(new TestCashReceipt
		{
			IsSuccess = true
		});

		static async Task<string> GetViewAddCashReceiptAsync() => await ReceiptFormatter.FormatAsync(new ViewAddCashReceipt
		{
			Units = Enumerable.Range(1, 5).Select(i => new CashOutUnit
			{
				Index = i,
				Currency = i % 2 == 0 ? TerminalConfiguration.Section?.Currency : "USD",
				Denomination = i * 100,
				Count = i * 2,
				Rejected = i,
				Remaining = i + 1,
				Dispensed = i - 1,
				Total = i * 3
			}).ToList()
		});

		static async Task<string> GetChequeDepositToAccountReceiptHeaderAsync() => await ReceiptFormatter.FormatAsync(new ChequeDepositToAccountReceiptHeader
		{
			
		});

		static async Task<string> GetChequeDepositToAccountReceiptFooterAsync() => await ReceiptFormatter.FormatAsync(new ChequeDepositToAccountReceiptFooter
		{
			AccountNumber = "8765432112345678",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			Micr = "123456789",
			TransactionStatus = TransactionStatus.Succeeded
		});

		static async Task<string> GetChequeDepositToCreditCardReceiptHeaderAsync() => await ReceiptFormatter.FormatAsync(new ChequeDepositToCreditCardReceiptHeader
		{

		});

		static async Task<string> GetChequeDepositToCreditCardReceiptFooterAsync() => await ReceiptFormatter.FormatAsync(new ChequeDepositToCreditCardReceiptFooter
		{
			CardNumber = "8765432112345678",
			CustomerName = "John Doe",
			TransactionNumber = "4567",
			Micr = "123456789"
		});

		#region ExecuteAsync

		static async Task ExecuteAsync(Func<Task<string>> func)
		{
			try
			{
				var result = await func();

				System.Console.WriteLine(new String('-', ReceiptsConfiguration.OutputWidth));
				System.Console.WriteLine(result);
				System.Console.WriteLine(new String('-', ReceiptsConfiguration.OutputWidth));
			}
			catch (Exception e)
			{
				System.Console.WriteLine();
				System.Console.WriteLine(e);
				System.Console.WriteLine();
			}
		}

		#endregion ExecuteAsync
	}
}