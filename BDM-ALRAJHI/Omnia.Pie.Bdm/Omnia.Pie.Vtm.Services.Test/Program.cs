namespace Omnia.Pie.Vtm.Services.Test
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Unity.Configuration;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.ChannelManagement;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using static System.Console;

	class Program
	{
		static void Main(string[] args)
		{
			var _container = new UnityContainer();
			_container.LoadConfiguration();

			var logWriterFactory = new LogWriterFactory();
			var logWriter = logWriterFactory.Create();
			Logger.SetLogWriter(logWriter, throwIfSet: true);

			var serviceManager = _container.Resolve<IServiceManager>();
			InitializeServices(serviceManager);

			CallAllServices(_container);
			//ExecuteAsync(async () => await SendDeviceStatusService(container));
			ReadLine();
		}

		static List<StatementItem> StatementItems;	
		private static async void CallAllServices(UnityContainer container) => await ExecuteAsync(async () =>
		{
			var _customerService = container.Resolve<ICustomerService>();
			//var items = await _customerService.GetStatementItemAsync(
				//					DateTime.Parse("01-06-2017"), DateTime.Parse("01-01-2017"), "6758475847", string.Empty);  // Number of Transactions is for Minisatatement
			var items = await _customerService.GetStatementItemAsync(DateTime.Now.AddMonths(-3), DateTime.Now, "5515515616515165", "20");
			StatementItems = items;
			StatementItems = StatementItems.OrderByDescending(x => x.TransactionDate).ToList();

			return;
			await ExecuteAsync(async () => await TransactionNotificationAsync(container));

			return;
			await ExecuteAsync(async () => await SendSMSAsync(container));

			//await ExecuteAsync(async () => await GetLinkedAccountAsync(container));

			await ExecuteAsync(async () => await GetCreditCardDetail(container));

			await ExecuteAsync(async () => await InsertEventAsync(container));

			await ExecuteAsync(async () => await ProductInfoAsync(container));

			await ExecuteAsync(async () => await CoordinationNumberAsync(container));

			await ExecuteAsync(async () => await GetStatementItemAsync(container));
			//await ExecuteAsync(async () => await GetStatementItemAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await GetStatementItemAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await GetStatementItemAsyncEmptyFields(container));

			await ExecuteAsync(async () => await GenerateTSNAsync(container));
			//await ExecuteAsync(async () => await GenerateTSNAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await GenerateTSNAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await GenerateTSNAsyncEmptyFields(container));

			await ExecuteAsync(async () => await SendDeviceStatusService(container));
			//await ExecuteAsync(async () => await SendDeviceStatusServiceTimeOutError(container));
			//await ExecuteAsync(async () => await SendDeviceStatusServiceErrorMessage(container));
			//await ExecuteAsync(async () => await SendDeviceStatusServiceEmptyFields(container));

			await ExecuteAsync(async () => await GetDebitCardsService(container));
			//await ExecuteAsync(async () => await GetDebitCardsServiceTimeOutError(container));
			//await ExecuteAsync(async () => await GetDebitCardsServiceErrorMessage(container));
			//await ExecuteAsync(async () => await GetDebitCardsServiceEmptyFields(container));

			await ExecuteAsync(async () => await UpdateCallRecord(container));
			//await ExecuteAsync(async () => await UpdateCallRecordTimeOutError(container));
			//await ExecuteAsync(async () => await UpdateCallRecordErrorMessage(container));
			//await ExecuteAsync(async () => await UpdateCallRecordEmptyFields(container));

			await ExecuteAsync(async () => await RegisterCall(container));
			//await ExecuteAsync(async () => await RegisterCallTimeOutError(container));
			//await ExecuteAsync(async () => await RegisterCallErrorMessage(container));
			//await ExecuteAsync(async () => await RegisterCallEmptyFields(container));

			await ExecuteAsync(async () => await CashDepositDetail(container));
			//await ExecuteAsync(async () => await CashDepositDetailTimeOutError(container));
			//await ExecuteAsync(async () => await CashDepositDetailErrorMessage(container));
			//await ExecuteAsync(async () => await CashDepositDetailEmptyFields(container));

			await ExecuteAsync(async () => await GetAccountDetail(container));
			//await ExecuteAsync(async () => await GetAccountDetailTimeOutError(container));
			//await ExecuteAsync(async () => await GetAccountDetailErrorMessage(container));
			//await ExecuteAsync(async () => await GetAccountDetailEmptyFields(container));

			await ExecuteAsync(async () => await ExchangeRate(container));
			//await ExecuteAsync(async () => await ExchangeRateTimeOutError(container));
			//await ExecuteAsync(async () => await ExchangeRateErrorMessage(container));
			//await ExecuteAsync(async () => await ExchangeRateEmptyFields(container));

			await ExecuteAsync(async () => await EmiratesId(container));
			//await ExecuteAsync(async () => await EmiratesIdTimeOutError(container));
			//await ExecuteAsync(async () => await EmiratesIdErrorMessage(container));
			//await ExecuteAsync(async () => await EmiratesIdEmptyFields(container));


			//await ExecuteAsync(async () => await GetCreditCardDetailTimeOutError(container));
			//await ExecuteAsync(async () => await GetCreditCardDetailErrorMessage(container));
			//await ExecuteAsync(async () => await GetCreditCardDetailEmptyFields(container));

			await ExecuteAsync(async () => await CashWithdrawalDebitCard(container));
			//await ExecuteAsync(async () => await CashWithdrawalDebitCardTimeOutError(container));
			//await ExecuteAsync(async () => await CashWithdrawalDebitCardErrorMessage(container));
			//await ExecuteAsync(async () => await CashWithdrawalDebitCardEmptyFields(container));

			await ExecuteAsync(async () => await ReverseCashWithdrawal(container));
			//await ExecuteAsync(async () => await ReverseCashWithdrawalTimeOutError(container));
			//await ExecuteAsync(async () => await ReverseCashWithdrawalErrorMessage(container));
			//await ExecuteAsync(async () => await ReverseCashWithdrawalEmptyFields(container));

			await ExecuteAsync(async () => await SendSmsOtp(container));
			//await ExecuteAsync(async () => await SendSmsOtpTimeoutError(container));
			//await ExecuteAsync(async () => await SendSmsOtpErrorMessage(container));
			//await ExecuteAsync(async () => await SendSmsOtpEmptyFields(container));

			await ExecuteAsync(async () => await ValidateSmsOtp(container));
			//await ExecuteAsync(async () => await ValidateSmsOtpTimeoutError(container));
			//await ExecuteAsync(async () => await ValidateSmsOtpErrorMessage(container));
			//await ExecuteAsync(async () => await ValidateSmsOtpEmptyFields(container));

			await ExecuteAsync(async () => await GetAccounts(container));
			//await ExecuteAsync(async () => await GetAccountsTimeoutError(container));
			//await ExecuteAsync(async () => await GetAccountsErrorMessage(container));
			//await ExecuteAsync(async () => await GetAccountsEmptyFields(container));
			await ExecuteAsync(async () => await Get10Accounts(container));
			await ExecuteAsync(async () => await Get5Accounts(container));
			await ExecuteAsync(async () => await Get3Accounts(container));

			await ExecuteAsync(async () => await GetCustomerDetail(container));
			//await ExecuteAsync(async () => await GetCustomerDetailTimeoutError(container));
			//await ExecuteAsync(async () => await GetCustomerDetailErrorMessage(container));
			//await ExecuteAsync(async () => await GetCustomerDetailEmptyFields(container));

			await ExecuteAsync(async () => await GetLoanAccountsAsync(container));
			//await ExecuteAsync(async () => await GetLoanAccountsAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await GetLoanAccountsAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await GetLoanAccountsAsyncEmptyFields(container));

			await ExecuteAsync(async () => await GetCardImage(container));
			//await ExecuteAsync(async () => await GetCardImageTimeoutError(container));
			//await ExecuteAsync(async () => await GetCardImageErrorMessage(container));
			//await ExecuteAsync(async () => await GetCardImageEmptyFields(container));

			await ExecuteAsync(async () => await GetDepositAccount(container));
			//await ExecuteAsync(async () => await GetDepositAccountTimeoutError(container));
			//await ExecuteAsync(async () => await GetDepositAccountErrorMessage(container));
			//await ExecuteAsync(async () => await GetDepositAccountEmptyFields(container));

			await ExecuteAsync(async () => await GetCreditCard(container));
			//await ExecuteAsync(async () => await GetCreditCardTimeoutError(container));
			//await ExecuteAsync(async () => await GetCreditCardErrorMessage(container));
			//await ExecuteAsync(async () => await GetCreditCardEmptyFields(container));

			await ExecuteAsync(async () => await VerifyPin(container));
			//await ExecuteAsync(async () => await VerifyPinTimeoutError(container));
			//await ExecuteAsync(async () => await VerifyPinErrorMessage(container));
			//await ExecuteAsync(async () => await VerifyPinEmptyFields(container));

			await ExecuteAsync(async () => await GetCustomerIdentifier(container));
			//await ExecuteAsync(async () => await GetCustomerIdentifierTimeoutError(container));
			//await ExecuteAsync(async () => await GetCustomerIdentifierErrorMessage(container));
			//await ExecuteAsync(async () => await GetCustomerIdentifierEmptyFields(container));

			await ExecuteAsync(async () => await IssueCheckBookAsync(container));
			//await ExecuteAsync(async () => await IssueCheckBookAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await IssueCheckBookAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await IssueCheckBookAsyncEmptyFields(container));

			await ExecuteAsync(async () => await DliverChequeBookAsync(container));
			//await ExecuteAsync(async () => await DliverChequeBookAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await DliverChequeBookAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await DliverChequeBookAsyncEmptyFields(container));

			await ExecuteAsync(async () => await SendEmailAsync(container));
			//await ExecuteAsync(async () => await SendEmailAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await SendEmailAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await SendEmailAsyncEmptyFields(container));

			await ExecuteAsync(async () => await ApplyStatementChargesAsync(container));
			//await ExecuteAsync(async () => await ApplyStatementChargesAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await ApplyStatementChargesAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await ApplyStatementChargesAsyncEmptyFields(container));

			await ExecuteAsync(async () => await GetStatementChargesAsync(container));
			//await ExecuteAsync(async () => await GetStatementChargesAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await GetStatementChargesAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await GetStatementChargesAsyncEmptyFields(container));

			await ExecuteAsync(async () => await ChequeDepositAsync(container));
			//await ExecuteAsync(async () => await ChequeDepositAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await ChequeDepositAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await ChequeDepositAsyncEmptyFields(container));

			await ExecuteAsync(async () => await GetChequePrintingChargeAsync(container));

			await ExecuteAsync(async () => await ReverseChargeAsync(container));
			//await ExecuteAsync(async () => await ReverseChargeAsyncTimeoutError(container));
			//await ExecuteAsync(async () => await ReverseChargeAsyncErrorMessage(container));
			//await ExecuteAsync(async () => await ReverseChargeAsyncEmptyFields(container));


		});

		static async Task SendDeviceStatusService(UnityContainer container)
		{
			try
			{
				var _channelManagementService = container.Resolve<IChannelManagementService>();

				#region "SendDeviceStatus"

				WriteLine();
				WriteLine("----------------Executing Send Device Status Service -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {"229887"}");

				WriteLine("----------------Request End------------------------");

				var devStatus = new List<DeviceStatus>();
				devStatus.AddRange(new List<DeviceStatus>()
				{ new DeviceStatus()
					{
						DeviceName = "CardReader",
						ErrorCode = "9J01000",
						Status = "Offline",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "0",
													Name = "Retain Cassett",
												}
											}
						},
					},
					new DeviceStatus()
					{
						DeviceName = "CashAcceptor",
						ErrorCode = "5312500",
						Status = "Offline",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "0",
													Name = "Cassett 1",
													Type = ""
												},
												new Cassette()
												{   Count = "0",
													Name = "Cassett 2",
													Type = ""
												}
											}
						},
					},
					new DeviceStatus()
						{
							DeviceName = "PinPad",
							OperationalStatus = new OperationalStatus()
							{
							},
							ErrorCode = "9EA9200",
							Status = "Offline",
						},
					new DeviceStatus()
						{
							DeviceName = "ReceiptPrinter",
							OperationalStatus = new OperationalStatus()
							{
								InkStatus = "Low"
							},
							ErrorCode = "2010700",
							Status = "Offline",
						}
				});

				var resp = await _channelManagementService.SendDeviceStatus(devStatus);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendDeviceStatusServiceTimeOutError(UnityContainer container)
		{
			try
			{
				var _channelManagementService = container.Resolve<IChannelManagementService>();

				#region "SendDeviceStatus"

				WriteLine();
				WriteLine("----------------Executing Send Device Status Service Time Out Error------------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {"22"}");

				WriteLine("----------------Request End------------------------");

				var devStatus = new List<DeviceStatus>();
				devStatus.AddRange(new List<DeviceStatus>()
				{ new DeviceStatus()
					{
						DeviceName = "Ca",
						ErrorCode = string.Empty,
						Status = "On",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "12",
													Name = "Re",
													Type = "Re"
												}
											}
						},
					},
					new DeviceStatus()
					{
						DeviceName = "Ca",
						ErrorCode = string.Empty,
						Status = "On",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "10",
													Name = "Ca",
													Type = "De"
												},
												new Cassette()
												{   Count = "10",
													Name = "Ca",
													Type = "Re"
												}
											}
						},
					}
				});

				var resp = await _channelManagementService.SendDeviceStatus(devStatus);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendDeviceStatusServiceErrorMessage(UnityContainer container)
		{
			try
			{
				var _channelManagementService = container.Resolve<IChannelManagementService>();

				#region "SendDeviceStatus"

				WriteLine();
				WriteLine("----------------Executing Send Device Status Service Error Message -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine("----------------Request End------------------------");

				var devStatus = new List<DeviceStatus>();
				devStatus.AddRange(new List<DeviceStatus>()
				{ new DeviceStatus()
					{
						DeviceName = "Card ",
						ErrorCode = string.Empty,
						Status = "Onli",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "1234",
													Name = "Reta",
													Type = "Reta"
												}
											}
						},
					},
					new DeviceStatus()
					{
						DeviceName = "Cash ",
						ErrorCode = string.Empty,
						Status = "Onli",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "1000",
													Name = "Cass",
													Type = "Depo"
												},
												new Cassette()
												{   Count = "1000",
													Name = "Cass",
													Type = "Reje"
												}
											}
						},
					}
				});

				var resp = await _channelManagementService.SendDeviceStatus(devStatus);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendDeviceStatusServiceEmptyFields(UnityContainer container)
		{
			try
			{
				var _channelManagementService = container.Resolve<IChannelManagementService>();

				#region "SendDeviceStatus"

				WriteLine();
				WriteLine("----------------Executing Send Device Status Service Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine("----------------Request End------------------------");

				var devStatus = new List<DeviceStatus>();
				devStatus.AddRange(new List<DeviceStatus>()
				{ new DeviceStatus()
					{
						DeviceName = "",
						ErrorCode = string.Empty,
						Status = "",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "",
													Name = "",
													Type = ""
												}
											}
						},
					},
					new DeviceStatus()
					{
						DeviceName = "",
						ErrorCode = string.Empty,
						Status = "",
						OperationalStatus = new OperationalStatus()
						{
							InkStatus = string.Empty,
							Cassettes = new List<Cassette>()
											{ new Cassette()
												{   Count = "",
													Name = "",
													Type = ""
												},
												new Cassette()
												{   Count = "",
													Name = "",
													Type = ""
												}
											}
						},
					}
				});

				var resp = await _channelManagementService.SendDeviceStatus(devStatus);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetDebitCardsService(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "DebitCardsService"

				WriteLine();
				WriteLine("----------------Executing Debit Cards Service -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");

				WriteLine("----------------Request End------------------------");

				//var devStatus = new List<DebitCard>();
				//devStatus.AddRange(new List<DebitCard>()
				//{ new DebitCard()
				//	{
				//		CardNumber = "123456475456",
				//		CardStatus = string.Empty,
				//		LinkedAccounts = new List<LinkedAccount>()
				//		{
				//			new LinkedAccount
				//			{
				//				AccountNumber = "4565645454445",
				//				AccountType = "Finance Account"

				//			}
				//		},
				//	},

				//});

				var resp = await _customerDetailService.GetDebitCardsAsync("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDebitCardsServiceTimeOutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "DebitCardsService"

				WriteLine();
				WriteLine("----------------Executing Debit Cards Service Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"32"}");

				WriteLine("----------------Request End------------------------");

				var resp = await _customerDetailService.GetDebitCardsAsync("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDebitCardsServiceErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "DebitCardsService"

				WriteLine();
				WriteLine("----------------Executing Debit Cards Service Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var resp = await _customerDetailService.GetDebitCardsAsync("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDebitCardsServiceEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "DebitCardsService"

				WriteLine();
				WriteLine("----------------Executing Debit Cards Service Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {""}");

				WriteLine("----------------Request End------------------------");

				var resp = await _customerDetailService.GetDebitCardsAsync("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Response : {resp}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task UpdateCallRecord(UnityContainer container)
		{
			try
			{
				var _UpdateCallRecordService = container.Resolve<ICommunicationService>();

				#region "Update Call Record"

				WriteLine();
				WriteLine("---------------- Executing Update Call Record Service -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _UpdateCallRecordService.UpdateCallRecordAsync("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.ResponseCode ?? "ResponseCode CallId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task UpdateCallRecordTimeOutError(UnityContainer container)
		{
			try
			{
				var _UpdateCallRecordService = container.Resolve<ICommunicationService>();

				#region "Update Call Record"

				WriteLine();
				WriteLine("----------------Executing Update Call Record Service Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"32"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _UpdateCallRecordService.UpdateCallRecordAsync("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.ResponseCode ?? "ResponseCode CallId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task UpdateCallRecordErrorMessage(UnityContainer container)
		{
			try
			{
				var _UpdateCallRecordService = container.Resolve<ICommunicationService>();

				#region "Update Call Record"

				WriteLine();
				WriteLine("----------------Executing Update Call Record Service Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _UpdateCallRecordService.UpdateCallRecordAsync("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.ResponseCode ?? "ResponseCode CallId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task UpdateCallRecordEmptyFields(UnityContainer container)
		{
			try
			{
				var _UpdateCallRecordService = container.Resolve<ICommunicationService>();

				#region "Update Call Record"

				WriteLine();
				WriteLine("----------------Executing Update Call Record Service Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {""}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _UpdateCallRecordService.UpdateCallRecordAsync("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.ResponseCode ?? "ResponseCode CallId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task RegisterCall(UnityContainer container)
		{
			try
			{
				var _registerCallService = container.Resolve<ICommunicationService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Register Call Service -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"callId : {"229887"}");
				WriteLine($"callStartTime : {"2500"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _registerCallService.RegisterCallAsync("229887", "2500");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.CallId ?? "Null CallId");
				WriteLine(registercall?.CallStartTime ?? "Null CallStartTime");
				WriteLine(registercall?.LogStatusCode ?? "Null LogStatusCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task RegisterCallTimeOutError(UnityContainer container)
		{
			try
			{
				var _registerCallService = container.Resolve<ICommunicationService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Register Call Service Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"callId : {"12"}");
				WriteLine($"callStartTime : {"12"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _registerCallService.RegisterCallAsync("12", "12");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.CallId ?? "Null CallId");
				WriteLine(registercall?.CallStartTime ?? "Null CallStartTime");
				WriteLine(registercall?.LogStatusCode ?? "Null LogStatusCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task RegisterCallErrorMessage(UnityContainer container)
		{
			try
			{
				var _registerCallService = container.Resolve<ICommunicationService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Register Call Service Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"callId : {"12"}");
				WriteLine($"callStartTime : {"1234"}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _registerCallService.RegisterCallAsync("1234", "1234");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.CallId ?? "Null CallId");
				WriteLine(registercall?.CallStartTime ?? "Null CallStartTime");
				WriteLine(registercall?.LogStatusCode ?? "Null LogStatusCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task RegisterCallEmptyFields(UnityContainer container)
		{
			try
			{
				var _registerCallService = container.Resolve<ICommunicationService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Register Call Service Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"callId : {""}");
				WriteLine($"callStartTime : {""}");

				WriteLine("----------------Request End------------------------");

				var registercall = await _registerCallService.RegisterCallAsync("", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(registercall?.CallId ?? "Null CallId");
				WriteLine(registercall?.CallStartTime ?? "Null CallStartTime");
				WriteLine(registercall?.LogStatusCode ?? "Null LogStatusCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task CashDepositDetail(UnityContainer container)
		{
			try
			{
				var _cashDepositService = container.Resolve<ITransactionService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Cash Deposit -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"DebitAccount : {"229887"}");
				WriteLine($"DebitAccountCurrency : {"12"}");
				WriteLine($"CreditAccount : {"229887"}");
				WriteLine($"CreditAccountCurrency : {"12"}");
				WriteLine($"CreditAmount : {"229887"}");
				WriteLine($"CreditNarrative : {"12"}");

				WriteLine("----------------Request End------------------------");
                var depositedDenominations = new List<DepositedDenominations>();

                DepositedDenominations denom = new DepositedDenominations();
                denom.Count = 10;
                denom.Type = 50;

                depositedDenominations.Add(denom);

                var cashDeposited = new CashDeposited
                {

                    DebitAccount = "Test account",
                    DebitAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAccount = "Test Account",
                    CreditAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAmount = "10",
                    BagSerialNo = "288419915125822",
                    SplitLength = "1",
                    UpdateBalanceFlag = "Y",
                    UpdateBalanceRequester = "LOCK_BOX",
                    MachineType = "A",
                    SystemFlag = "Y"
                };

                //var customerDetail = await _cashDepositService.CashDepositAsync("329887", "AED", "1234567", "USD", "2500", "OK");
                var customerDetail = await _cashDepositService.CashDepositAsync("329887", depositedDenominations, cashDeposited);


                WriteLine();
				WriteLine("----------------Response Start----------------------");

				//WriteLine(customerDetail?.Duplicate ?? "Null Duplicate");
				//WriteLine(customerDetail?.HostTransCode ?? "Null HostTransCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashDepositDetailTimeOutError(UnityContainer container)
		{
			try
			{
				var _cashDepositService = container.Resolve<ITransactionService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Cash Deposit Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"DebitAccount : {"12"}");
				WriteLine($"DebitAccountCurrency : {"12"}");
				WriteLine($"CreditAccount : {"12"}");
				WriteLine($"CreditAccountCurrency : {"12"}");
				WriteLine($"CreditAmount : {"12"}");
				WriteLine($"CreditNarrative : {"12"}");

				WriteLine("----------------Request End------------------------");

                var depositedDenominations = new List<DepositedDenominations>();

                DepositedDenominations denom = new DepositedDenominations();
                denom.Count = 10;
                denom.Type = 50;

                depositedDenominations.Add(denom);

                var cashDeposited = new CashDeposited
                {

                    DebitAccount = "Test account",
                    DebitAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAccount = "Test Account",
                    CreditAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAmount = "10",
                    BagSerialNo = "288419915125822",
                    SplitLength = "1",
                    UpdateBalanceFlag = "Y",
                    UpdateBalanceRequester = "LOCK_BOX",
                    MachineType = "A",
                    SystemFlag = "Y"
                };

                //var customerDetail = await _cashDepositService.CashDepositAsync("329887", "AED", "1234567", "USD", "2500", "OK");
                var customerDetail = await _cashDepositService.CashDepositAsync("329887", depositedDenominations, cashDeposited);

                WriteLine();
				WriteLine("----------------Response Start----------------------");

				//WriteLine(customerDetail?.Duplicate ?? "Null Duplicate");
				//WriteLine(customerDetail?.HostTransCode ?? "Null HostTransCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashDepositDetailErrorMessage(UnityContainer container)
		{
			try
			{
				var _cashDepositService = container.Resolve<ITransactionService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing Cash Deposit Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"DebitAccount : {"1234"}");
				WriteLine($"DebitAccountCurrency : {"1234"}");
				WriteLine($"CreditAccount : {"1234"}");
				WriteLine($"CreditAccountCurrency : {"1234"}");
				WriteLine($"CreditAmount : {"1234"}");
				WriteLine($"CreditNarrative : {"1234"}");

				WriteLine("----------------Request End------------------------");

                var depositedDenominations = new List<DepositedDenominations>();

                DepositedDenominations denom = new DepositedDenominations();
                denom.Count = 10;
                denom.Type = 50;

                depositedDenominations.Add(denom);

                var cashDeposited = new CashDeposited
                {

                    DebitAccount = "Test account",
                    DebitAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAccount = "Test Account",
                    CreditAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAmount = "10",
                    BagSerialNo = "288419915125822",
                    SplitLength = "1",
                    UpdateBalanceFlag = "Y",
                    UpdateBalanceRequester = "LOCK_BOX",
                    MachineType = "A",
                    SystemFlag = "Y"
                };

                //var customerDetail = await _cashDepositService.CashDepositAsync("329887", "AED", "1234567", "USD", "2500", "OK");
                var customerDetail = await _cashDepositService.CashDepositAsync("329887", depositedDenominations, cashDeposited);

                WriteLine();
				WriteLine("----------------Response Start----------------------");

				//WriteLine(customerDetail?.Duplicate ?? "Null Duplicate");
				//WriteLine(customerDetail?.HostTransCode ?? "Null HostTransCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashDepositDetailEmptyFields(UnityContainer container)
		{
			try
			{
				var _cashDepositService = container.Resolve<ITransactionService>();

				#region "CashDeposit Response"

				WriteLine();
				WriteLine("----------------Executing CashDeposit Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"DebitAccount : {""}");
				WriteLine($"DebitAccountCurrency : {""}");
				WriteLine($"CreditAccount : {""}");
				WriteLine($"CreditAccountCurrency : {""}");
				WriteLine($"CreditAmount : {""}");
				WriteLine($"CreditNarrative : {""}");

				WriteLine("----------------Request End------------------------");

                var depositedDenominations = new List<DepositedDenominations>();

                DepositedDenominations denom = new DepositedDenominations();
                denom.Count = 10;
                denom.Type = 50;

                depositedDenominations.Add(denom);

                var cashDeposited = new CashDeposited
                {

                    DebitAccount = "Test account",
                    DebitAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAccount = "Test Account",
                    CreditAccountCurrency = TerminalConfiguration.Section.Currency,
                    CreditAmount = "10",
                    BagSerialNo = "288419915125822",
                    SplitLength = "1",
                    UpdateBalanceFlag = "Y",
                    UpdateBalanceRequester = "LOCK_BOX",
                    MachineType = "A",
                    SystemFlag = "Y"
                };

                //var customerDetail = await _cashDepositService.CashDepositAsync("329887", "AED", "1234567", "USD", "2500", "OK");
                var customerDetail = await _cashDepositService.CashDepositAsync("329887", depositedDenominations, cashDeposited);

                WriteLine();
				WriteLine("----------------Response Start----------------------");
				//WriteLine(customerDetail?.Duplicate ?? "Null Duplicate");
				//WriteLine(customerDetail?.HostTransCode ?? "Null HostTransCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetAccountDetail(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetAccountDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Account Detail -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {"229887"}");
				WriteLine($"customerId : {"329887"}");


				WriteLine("----------------Request End------------------------");

				var accountDetail = await _customerDetailService.GetAccountDetail("229887", "329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(accountDetail?.AccountCurrency ?? "Null AccountCurrency");
				WriteLine(accountDetail?.AccountNumber ?? "Null AccountNumber");
				WriteLine(accountDetail?.AccountStatus ?? "Null AccountStatus");
				WriteLine(accountDetail?.AccountTitle ?? "Null AccountTitle");
				WriteLine(accountDetail?.AccountType ?? "Null AccountType");
				WriteLine(accountDetail?.AvailableBalance == null ? "Null AvailableBalance" : accountDetail?.AvailableBalance.ToString());
				WriteLine(accountDetail?.BranchId ?? "Null BranchId");
				WriteLine(accountDetail?.IBAN ?? "Null IBAN");
				WriteLine(accountDetail?.ResponseCode ?? "Null ResponseCode");


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountDetailTimeOutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetAccountDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetAccountDetail Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {"22"}");
				WriteLine($"customerId : {"32"}");


				WriteLine("----------------Request End------------------------");

				var accountDetail = await _customerDetailService.GetAccountDetail("22", "32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(accountDetail?.AccountCurrency ?? "Null AccountCurrency");
				WriteLine(accountDetail?.AccountNumber ?? "Null AccountNumber");
				WriteLine(accountDetail?.AccountStatus ?? "Null AccountStatus");
				WriteLine(accountDetail?.AccountTitle ?? "Null AccountTitle");
				WriteLine(accountDetail?.AccountType ?? "Null AccountType");
				WriteLine(accountDetail?.AvailableBalance == null ? "Null AvailableBalance" : accountDetail?.AvailableBalance.ToString());
				WriteLine(accountDetail?.BranchId ?? "Null BranchId");
				WriteLine(accountDetail?.IBAN ?? "Null IBAN");
				WriteLine(accountDetail?.ResponseCode ?? "Null ResponseCode");


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountDetailErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetAccountDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Account Detail Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {"2234"}");
				WriteLine($"customerId : {"3298"}");


				WriteLine("----------------Request End------------------------");

				var accountDetail = await _customerDetailService.GetAccountDetail("2234", "3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(accountDetail?.AccountCurrency ?? "Null AccountCurrency");
				WriteLine(accountDetail?.AccountNumber ?? "Null AccountNumber");
				WriteLine(accountDetail?.AccountStatus ?? "Null AccountStatus");
				WriteLine(accountDetail?.AccountTitle ?? "Null AccountTitle");
				WriteLine(accountDetail?.AccountType ?? "Null AccountType");
				WriteLine(accountDetail?.AvailableBalance == null ? "Null AvailableBalance" : accountDetail?.AvailableBalance.ToString());
				WriteLine(accountDetail?.BranchId ?? "Null BranchId");
				WriteLine(accountDetail?.IBAN ?? "Null IBAN");
				WriteLine(accountDetail?.ResponseCode ?? "Null ResponseCode");


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountDetailEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetAccountDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetAccountDetail Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountNumber : {""}");
				WriteLine($"customerId : {""}");


				WriteLine("----------------Request End------------------------");

				var accountDetail = await _customerDetailService.GetAccountDetail("", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(accountDetail?.AccountCurrency ?? "Null AccountCurrency");
				WriteLine(accountDetail?.AccountNumber ?? "Null AccountNumber");
				WriteLine(accountDetail?.AccountStatus ?? "Null AccountStatus");
				WriteLine(accountDetail?.AccountTitle ?? "Null AccountTitle");
				WriteLine(accountDetail?.AccountType ?? "Null AccountType");
				WriteLine(accountDetail?.AvailableBalance == null ? "Null AvailableBalance" : accountDetail?.AvailableBalance.ToString());
				WriteLine(accountDetail?.BranchId ?? "Null BranchId");
				WriteLine(accountDetail?.IBAN ?? "Null IBAN");
				WriteLine(accountDetail?.ResponseCode ?? "Null ResponseCode");


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ExchangeRate(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Exchange Rate -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"FromCurrency : {"USD"}");
				WriteLine($"ToCurrency : {"AED"}");
				WriteLine($"TransactionAmount : {"500"}");
				WriteLine($"PaymentType : {"1"}");

				WriteLine("----------------Request End------------------------");

				var exc = await _transactionService.ExchangeRateAsync("USD", "AED", "2500", "1");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {exc?.ExchangeRate}");
				WriteLine($"Status : {exc?.ExchangeRateCurrency}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ExchangeRateTimeOutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Exchange Rate Time Out Error - -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"FromCurrency : {"US"}");
				WriteLine($"ToCurrency : {"AE"}");
				WriteLine($"TransactionAmount : {"50"}");
				WriteLine($"PaymentType : {"12"}");

				WriteLine("----------------Request End------------------------");

				var exc = await _transactionService.ExchangeRateAsync("US", "AE", "25", "12");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {exc?.ExchangeRate}");
				WriteLine($"Status : {exc?.ExchangeRateCurrency}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ExchangeRateErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Exchange Rate Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"FromCurrency : {"USuj"}");
				WriteLine($"ToCurrency : {"AhjE"}");
				WriteLine($"TransactionAmount : {"5005"}");
				WriteLine($"PaymentType : {"1234"}");

				WriteLine("----------------Request End------------------------");

				var exc = await _transactionService.ExchangeRateAsync("USuj", "AhjE", "5005", "1234");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {exc?.ExchangeRate}");
				WriteLine($"Status : {exc?.ExchangeRateCurrency}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ExchangeRateEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Exchange Rate Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"FromCurrency : {""}");
				WriteLine($"ToCurrency : {""}");
				WriteLine($"TransactionAmount : {""}");
				WriteLine($"PaymentType : {""}");

				WriteLine("----------------Request End------------------------");

				var exc = await _transactionService.ExchangeRateAsync("", "", "", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {exc?.ExchangeRate}");
				WriteLine($"Status : {exc?.ExchangeRateCurrency}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task EmiratesId(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Emirates Id Status -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"EidNumber : {"784-1854566486-7"}");
				WriteLine($"Name : {"Muhammad"}");
				WriteLine($"ExpiryDate : {"11/12/2018"}");

				WriteLine("----------------Request End------------------------");

				var emid = await _authenticationService.ValidateEmiratesId("784-1854566486-7", "Muhammad", "11/12/2018");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {emid?.Status}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.InvalidEmiratesIdException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task EmiratesIdTimeOutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Emirates Id Status Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"EidNumber : {"46"}");
				WriteLine($"Name : {"F6"}");
				WriteLine($"ExpiryDate : {"AF"}");

				WriteLine("----------------Request End------------------------");

				var emid = await _authenticationService.ValidateEmiratesId("46", "F6", "AF");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {emid?.Status}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task EmiratesIdErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Emirates Id Status Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"EidNumber : {"4646"}");
				WriteLine($"Name : {"F6F6"}");
				WriteLine($"ExpiryDate : {"AFAF"}");

				WriteLine("----------------Request End------------------------");

				var emid = await _authenticationService.ValidateEmiratesId("4646", "F6F6", "AFAF");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {emid?.Status}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task EmiratesIdEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "Emirates Id Response"

				WriteLine();
				WriteLine("----------------Executing Emirates Id Status Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"EidNumber : {""}");
				WriteLine($"Name : {""}");
				WriteLine($"ExpiryDate : {""}");

				WriteLine("----------------Request End------------------------");

				var emid = await _authenticationService.ValidateEmiratesId("", "", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine($"Status : {emid?.Status}");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetCreditCardDetail(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCardDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit Card Detail -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CardNumber : {"4421660065848016"}");

				WriteLine("----------------Request End------------------------");

				var CreditCardDetail = await _transactionService.GetCreditCardDetailAsync("4421660065848016", "559887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(CreditCardDetail?.AvailableCreditLimit ?? "Null AvailableCreditLimit");
				WriteLine(CreditCardDetail?.BilledAmount ?? "Null BilledAmount");
				WriteLine(CreditCardDetail?.CardNumber ?? "Null CardNumber");
				WriteLine(CreditCardDetail?.CardStatus ?? "Null CardStatus");
				WriteLine(CreditCardDetail?.CardType ?? "Null CardType");
				WriteLine(CreditCardDetail?.CreditCardCategory ?? "Null CreditCardCategory");
				WriteLine(CreditCardDetail?.CurrencyCode ?? "Null CurrencyCode");
				WriteLine(CreditCardDetail?.CurrentOutStandingAmount ?? "Null CurrentOutStandingAmount");
				WriteLine(CreditCardDetail?.MinimumDueAmount ?? "Null MinimumDueAmount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCreditCardDetailErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCardDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit Card Detail Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CardNumber : {"1212"}");

				WriteLine("----------------Request End------------------------");

				var CreditCardDetail = await _transactionService.GetCreditCardDetailAsync("1212", "329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(CreditCardDetail?.AvailableCreditLimit ?? "Null AvailableCreditLimit");
				WriteLine(CreditCardDetail?.BilledAmount ?? "Null BilledAmount");
				WriteLine(CreditCardDetail?.CardNumber ?? "Null CardNumber");
				WriteLine(CreditCardDetail?.CardStatus ?? "Null CardStatus");
				WriteLine(CreditCardDetail?.CardType ?? "Null CardType");
				WriteLine(CreditCardDetail?.CreditCardCategory ?? "Null CreditCardCategory");
				WriteLine(CreditCardDetail?.CurrencyCode ?? "Null CurrencyCode");
				WriteLine(CreditCardDetail?.CurrentOutStandingAmount ?? "Null CurrentOutStandingAmount");
				WriteLine(CreditCardDetail?.MinimumDueAmount ?? "Null MinimumDueAmount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task CashWithdrawalDebitCard(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "CashWithdrawalDebitCard Response"

				WriteLine();
				WriteLine("----------------Executing Cash Withdrawal Debit Card -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"4689156465896556=8913235613256921"}");
				WriteLine($"Pin : {"F65EA78723121055"}");
				WriteLine($"ICC Request : {"9D77B9BF30AA0FF523CCEB43F6586CCEC30EF3203258B82779FA6D2CEE2DE6E766DBC7CD486C8340ECD403D37EC46CCD7FBBDEFABFF560EA7DBA93C7D81445548F01F19000"}");
				WriteLine($"TransactionAmount : {"500"}");
				WriteLine($"AccountNumber : {"45155654564"}");
				WriteLine($"AccountType : {"Saving Account"}");
				WriteLine($"CurrencyCode : {"CS101"}");
				WriteLine($"ConvertedAmount : {"500"}");
				WriteLine($"ExchangeRate : {"3.2"}");

				WriteLine("----------------Request End------------------------");

				var cashwithdraw = await _transactionService.CashWithdrawalDebitCardAsync("4689156465896556=8913235613256921", "F65EA78723121055", "9D77B9BF30AA0FF523CCEB43F6586CCEC30EF3203258B82779FA6D2CEE2DE6E766DBC7CD486C8340ECD403D37EC46CCD7FBBDEFABFF560EA7DBA93C7D81445548F01F19000", "500", "45155654564", "Saving Account", "CS101", "500", "3.2");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(cashwithdraw?.AuthCode ?? "Null AuthCode");
				WriteLine(cashwithdraw?.IccData ?? "Null IccData");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashWithdrawalDebitCardTimeOutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "CashWithdrawalDebitCard Response"

				WriteLine();
				WriteLine("----------------Executing CashWithdrawalDebitCard Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"46"}");
				WriteLine($"Pin : {"F6"}");
				WriteLine($"ICC Request : {"9D"}");
				WriteLine($"TransactionAmount : {"50"}");
				WriteLine($"AccountNumber : {"45"}");
				WriteLine($"AccountType : {"Sa"}");
				WriteLine($"CurrencyCode : {"CS"}");
				WriteLine($"ConvertedAmount : {"50"}");
				WriteLine($"ExchangeRate : {"3."}");

				WriteLine("----------------Request End------------------------");

				var cashwithdraw = await _transactionService.CashWithdrawalDebitCardAsync("46", "F6", "9D", "50", "45", "Sa", "CS", "50", "3.");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(cashwithdraw?.AuthCode ?? "Null AuthCode");
				WriteLine(cashwithdraw?.IccData ?? "Null IccData");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashWithdrawalDebitCardErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "CashWithdrawalDebitCard Response"

				WriteLine();
				WriteLine("----------------Executing CashWithdrawalDebitCard Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"4612"}");
				WriteLine($"Pin : {"F645"}");
				WriteLine($"ICC Request : {"977D"}");
				WriteLine($"TransactionAmount : {"5780"}");
				WriteLine($"AccountNumber : {"4545"}");
				WriteLine($"AccountType : {"Salk"}");
				WriteLine($"CurrencyCode : {"CSlk"}");
				WriteLine($"ConvertedAmount : {"5450"}");
				WriteLine($"ExchangeRate : {"3.45"}");

				WriteLine("----------------Request End------------------------");

				var cashwithdraw = await _transactionService.CashWithdrawalDebitCardAsync("4612", "F645", "977D", "5780", "4545", "Salk", "CSlk", "5450", "3.45");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(cashwithdraw?.AuthCode ?? "Null AuthCode");
				WriteLine(cashwithdraw?.IccData ?? "Null IccData");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task CashWithdrawalDebitCardEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "CashWithdrawalDebitCard Response"

				WriteLine();
				WriteLine("----------------Executing CashWithdrawal Debit Card Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {""}");
				WriteLine($"Pin : {""}");
				WriteLine($"ICC Request : {""}");
				WriteLine($"TransactionAmount : {""}");
				WriteLine($"AccountNumber : {""}");
				WriteLine($"AccountType : {""}");
				WriteLine($"CurrencyCode : {""}");
				WriteLine($"ConvertedAmount : {""}");
				WriteLine($"ExchangeRate : {""}");

				WriteLine("----------------Request End------------------------");

				var cashwithdraw = await _transactionService.CashWithdrawalDebitCardAsync("", "", "", "", "", "", "", "", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(cashwithdraw?.AuthCode ?? "Null AuthCode");
				WriteLine(cashwithdraw?.IccData ?? "Null IccData");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ReverseCashWithdrawal(UnityContainer container)
		{
			try
			{
				var _cashwithdrawalReversal = container.Resolve<ITransactionService>();

				#region "ReverseCashWithdrawal Response"

				WriteLine();
				WriteLine("----------------Executing ReverseCashWithdrawal -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"AuthCode : {"1234"}");
				WriteLine($"track2 : {"100002"}");
				WriteLine($"ReversalReason : {"ReversalReason.None"}");

				WriteLine("----------------Request End------------------------");

				var reversal = await _cashwithdrawalReversal.ReverseCashWithdrawalUsingDebitCardAsync("1234", "100002", ReversalReason.None);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reversal ?? "Null reversal");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseCashWithdrawalTimeOutError(UnityContainer container)
		{
			try
			{
				var _cashwithdrawalReversal = container.Resolve<ITransactionService>();

				#region "ReverseCashWithdrawal Response"

				WriteLine();
				WriteLine("----------------Executing Reverse CashWithdrawal Time Out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"AuthCode : {"12"}");
				WriteLine($"track2 : {"10"}");
				WriteLine($"ReversalReason : {"Re"}");

				WriteLine("----------------Request End------------------------");

				var reversal = await _cashwithdrawalReversal.ReverseCashWithdrawalUsingDebitCardAsync("12", "10", ReversalReason.None);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reversal ?? "Null reversal");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseCashWithdrawalErrorMessage(UnityContainer container)
		{
			try
			{
				var _cashwithdrawalReversal = container.Resolve<ITransactionService>();

				#region "ReverseCashWithdrawal Response"

				WriteLine();
				WriteLine("----------------Executing ReverseCashWithdrawal Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"AuthCode : {"1212"}");
				WriteLine($"track2 : {"1012"}");
				WriteLine($"ReversalReason : {"Re"}");

				WriteLine("----------------Request End------------------------");

				var reversal = await _cashwithdrawalReversal.ReverseCashWithdrawalUsingDebitCardAsync("1212", "1012", ReversalReason.None);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reversal ?? "Null reversal");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseCashWithdrawalEmptyFields(UnityContainer container)
		{
			try
			{
				var _cashwithdrawalReversal = container.Resolve<ITransactionService>();

				#region "ReverseCashWithdrawal Response"

				WriteLine();
				WriteLine("----------------Executing ReverseCashWithdrawal Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"AuthCode : {""}");
				WriteLine($"track2 : {""}");
				WriteLine($"ReversalReason : {""}");

				WriteLine("----------------Request End------------------------");

				var reversal = await _cashwithdrawalReversal.ReverseCashWithdrawalUsingDebitCardAsync("", "", ReversalReason.None);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reversal ?? "Null reversal");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task SendSmsOtp(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "SendSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing SendSmsOtp -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CustomerIdentifier : {"329887"}");

				WriteLine("----------------Request End------------------------");

				var sendSmsotp = await _authenticationService.SendSmsOtp("329887", "3");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendSmsotp?.Otp ?? "Null Otp");
				WriteLine(sendSmsotp?.Uuid ?? "Null Uuid");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendSmsOtpTimeoutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "SendSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing SendSmsOtp Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CustomerIdentifier : {"32"}");

				WriteLine("----------------Request End------------------------");

				var sendSmsotp = await _authenticationService.SendSmsOtp("32", "3");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendSmsotp?.Otp ?? "Null Otp");
				WriteLine(sendSmsotp?.Uuid ?? "Null Uuid");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendSmsOtpErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "SendSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing SendSmsOtp Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CustomerIdentifier : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var sendSmsotp = await _authenticationService.SendSmsOtp("3298", "3");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendSmsotp?.Otp ?? "Null Otp");
				WriteLine(sendSmsotp?.Uuid ?? "Null Uuid");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task SendSmsOtpEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "SendSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing SendSmsOtp Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"CustomerIdentifier : {""}");

				WriteLine("----------------Request End------------------------");

				var sendSmsotp = await _authenticationService.SendSmsOtp("", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendSmsotp?.Otp ?? "Null Otp");
				WriteLine(sendSmsotp?.Uuid ?? "Null Uuid");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ValidateSmsOtp(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "ValidateSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing ValidateSmsOtp -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"otp : {"15"}");
				WriteLine($"Uuid : {"gsdgjkkj"}");

				WriteLine("----------------Request End------------------------");

				var validateSmsOtp = await _authenticationService.ValidateSmsOtp("sdfjks", "gsdgjkkj");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(Convert.ToString(validateSmsOtp?.OtpMatched) ?? "Null OtpMatched");
				WriteLine(validateSmsOtp?.OtpMismatchCount ?? "Null OtpMismatchCount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ValidateSmsOtpTimeoutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "ValidateSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing ValidateSmsOtp Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"otp : {"15"}");
				WriteLine($"Uuid : {"gs"}");

				WriteLine("----------------Request End------------------------");

				var validateSmsOtp = await _authenticationService.ValidateSmsOtp("15", "gs");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(Convert.ToString(validateSmsOtp?.OtpMatched) ?? "Null OtpMatched");
				WriteLine(validateSmsOtp?.OtpMismatchCount ?? "Null OtpMismatchCount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ValidateSmsOtpErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "ValidateSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing ValidateSmsOtp Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"otp : {"1515"}");
				WriteLine($"Uuid : {"gsgs"}");

				WriteLine("----------------Request End------------------------");

				var validateSmsOtp = await _authenticationService.ValidateSmsOtp("1515", "gsgs");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(Convert.ToString(validateSmsOtp?.OtpMatched) ?? "Null OtpMatched");
				WriteLine(validateSmsOtp?.OtpMismatchCount ?? "Null OtpMismatchCount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ValidateSmsOtpEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "ValidateSmsOtp Response"

				WriteLine();
				WriteLine("----------------Executing ValidateSmsOtp Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"otp : {""}");
				WriteLine($"Uuid : {""}");

				WriteLine("----------------Request End------------------------");

				var validateSmsOtp = await _authenticationService.ValidateSmsOtp("", "");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(Convert.ToString(validateSmsOtp?.OtpMatched) ?? "Null OtpMatched");
				WriteLine(validateSmsOtp?.OtpMismatchCount ?? "Null OtpMismatchCount");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetAccounts(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "1223323", Username: "1223323",  conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
						WriteLine(item?.Name ?? "Null Name");
						WriteLine("Null AccountingUnitId");
						WriteLine("Null AvailableBalance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine("Null TypeCode");
					}
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountsTimeoutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"32"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "32", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
						WriteLine(item?.Name ?? "Null Name");
						WriteLine("Null AccountingUnitId");
						WriteLine("Null AvailableBalance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine("Null TypeCode");
					}
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountsErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"3298"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "1223323", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
                        WriteLine(item?.Name ?? "Null Name");
                        WriteLine("Null AccountingUnitId");
                        WriteLine("Null AvailableBalance");
                        WriteLine(item?.Currency ?? "Null Currency");
                        WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
                        WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
                        WriteLine(item?.Type ?? "Null Type");
                        WriteLine("Null TypeCode");
                    }
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetAccountsEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {""}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
                        WriteLine(item?.Name ?? "Null Name");
                        WriteLine("Null AccountingUnitId");
                        WriteLine("Null AvailableBalance");
                        WriteLine(item?.Currency ?? "Null Currency");
                        WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
                        WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
                        WriteLine(item?.Type ?? "Null Type");
                        WriteLine("Null TypeCode");
                    }
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task Get10Accounts(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Get10Accounts-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "329887", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
                        WriteLine(item?.Name ?? "Null Name");
                        WriteLine("Null AccountingUnitId");
                        WriteLine("Null AvailableBalance");
                        WriteLine(item?.Currency ?? "Null Currency");
                        WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
                        WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
                        WriteLine(item?.Type ?? "Null Type");
                        WriteLine("Null TypeCode");
                    }
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task Get5Accounts(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Get5Accounts-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "329887", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
                        WriteLine(item?.Name ?? "Null Name");
                        WriteLine("Null AccountingUnitId");
                        WriteLine("Null AvailableBalance");
                        WriteLine(item?.Currency ?? "Null Currency");
                        WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
                        WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
                        WriteLine(item?.Type ?? "Null Type");
                        WriteLine("Null TypeCode");
                    }
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task Get3Accounts(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetAccounts Response"

				WriteLine();
				WriteLine("----------------Executing Get Accounts Get3Accounts-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");
				WriteLine($"conditionId : {"AccountCriterion.All"}");

				WriteLine("----------------Request End------------------------");

				var accounts = await _authenticationService.GetAccounts(customerIdentifier: "329887", Username: "1223323", conditionId: AccountCriterion.All);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (accounts != null)
				{
					foreach (var item in accounts)
					{
                        WriteLine(item?.Name ?? "Null Name");
                        WriteLine("Null AccountingUnitId");
                        WriteLine("Null AvailableBalance");
                        WriteLine(item?.Currency ?? "Null Currency");
                        WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
                        WriteLine(Convert.ToString(item?.Number) ?? "Null Number");
                        WriteLine(item?.Type ?? "Null Type");
                        WriteLine("Null TypeCode");
                    }
				}
				else
				{
					WriteLine("List of Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetCustomerDetail(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetCustomerDetail -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");


				WriteLine("----------------Request End------------------------");

				var customerDetail = await _customerDetailService.GetCustomerDetail("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerDetail?.Address1 ?? "Null Address1");
				WriteLine(customerDetail?.Address2 ?? "Null Address2");
				WriteLine(customerDetail?.BirthDate ?? "Null BirthDate");
				WriteLine(customerDetail?.Country ?? "Null Country");
				WriteLine(customerDetail?.CustomerCategory ?? "Null CustomerCategory");
				WriteLine(customerDetail?.CustomerInformationFileId ?? "Null CustomerInformationFileId");
				WriteLine(customerDetail?.CustomerStatus ?? "Null CustomerStatus");
				WriteLine(customerDetail?.Email ?? "Null Email");
				WriteLine(customerDetail?.FullName ?? "Null FullName");
				WriteLine(customerDetail?.Gender ?? "Null Gender");
				WriteLine(customerDetail?.Nationality ?? "Null Nationality");
				WriteLine(customerDetail?.MobileNumber ?? "Null PhoneNumber");
				WriteLine(customerDetail?.VisaRefNumber ?? "Null VisaRefNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerDetailTimeoutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetCustomerDetail Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"32"}");


				WriteLine("----------------Request End------------------------");

				var customerDetail = await _customerDetailService.GetCustomerDetail("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerDetail?.Address1 ?? "Null Address1");
				WriteLine(customerDetail?.Address2 ?? "Null Address2");
				WriteLine(customerDetail?.BirthDate ?? "Null BirthDate");
				WriteLine(customerDetail?.Country ?? "Null Country");
				WriteLine(customerDetail?.CustomerCategory ?? "Null CustomerCategory");
				WriteLine(customerDetail?.CustomerInformationFileId ?? "Null CustomerInformationFileId");
				WriteLine(customerDetail?.CustomerStatus ?? "Null CustomerStatus");
				WriteLine(customerDetail?.Email ?? "Null Email");
				WriteLine(customerDetail?.FullName ?? "Null FullName");
				WriteLine(customerDetail?.Gender ?? "Null Gender");
				WriteLine(customerDetail?.Nationality ?? "Null Nationality");
				WriteLine(customerDetail?.MobileNumber ?? "Null PhoneNumber");
				WriteLine(customerDetail?.VisaRefNumber ?? "Null VisaRefNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerDetailErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetCustomerDetail Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"3298"}");


				WriteLine("----------------Request End------------------------");

				var customerDetail = await _customerDetailService.GetCustomerDetail("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerDetail?.Address1 ?? "Null Address1");
				WriteLine(customerDetail?.Address2 ?? "Null Address2");
				WriteLine(customerDetail?.BirthDate ?? "Null BirthDate");
				WriteLine(customerDetail?.Country ?? "Null Country");
				WriteLine(customerDetail?.CustomerCategory ?? "Null CustomerCategory");
				WriteLine(customerDetail?.CustomerInformationFileId ?? "Null CustomerInformationFileId");
				WriteLine(customerDetail?.CustomerStatus ?? "Null CustomerStatus");
				WriteLine(customerDetail?.Email ?? "Null Email");
				WriteLine(customerDetail?.FullName ?? "Null FullName");
				WriteLine(customerDetail?.Gender ?? "Null Gender");
				WriteLine(customerDetail?.Nationality ?? "Null Nationality");
				WriteLine(customerDetail?.MobileNumber ?? "Null PhoneNumber");
				WriteLine(customerDetail?.VisaRefNumber ?? "Null VisaRefNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerDetailEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetCustomerDetail Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {""}");


				WriteLine("----------------Request End------------------------");

				var customerDetail = await _customerDetailService.GetCustomerDetail("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerDetail?.Address1 ?? "Null Address1");
				WriteLine(customerDetail?.Address2 ?? "Null Address2");
				WriteLine(customerDetail?.BirthDate ?? "Null BirthDate");
				WriteLine(customerDetail?.Country ?? "Null Country");
				WriteLine(customerDetail?.CustomerCategory ?? "Null CustomerCategory");
				WriteLine(customerDetail?.CustomerInformationFileId ?? "Null CustomerInformationFileId");
				WriteLine(customerDetail?.CustomerStatus ?? "Null CustomerStatus");
				WriteLine(customerDetail?.Email ?? "Null Email");
				WriteLine(customerDetail?.FullName ?? "Null FullName");
				WriteLine(customerDetail?.Gender ?? "Null Gender");
				WriteLine(customerDetail?.Nationality ?? "Null Nationality");
				WriteLine(customerDetail?.MobileNumber ?? "Null PhoneNumber");
				WriteLine(customerDetail?.VisaRefNumber ?? "Null VisaRefNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetLoanAccountsAsync(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetLoanAccountsAsync Response"

				WriteLine();
				WriteLine("----------------Executing GetLoanAccountsAsync -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"LoanAccounts : {"LoanAccounts"}");


				WriteLine("----------------Request End------------------------");

				var getLoanAccounts = await _customerDetailService.GetLoanAccountsAsync("LoanAccounts");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (getLoanAccounts != null)
				{
					foreach (var item in getLoanAccounts)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null AvailableBalance");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of GetLoanAccounts is Null.");
				}


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetLoanAccountsAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetLoanAccountsAsync Response"

				WriteLine();
				WriteLine("----------------Executing GetLoanAccountsAsync Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"LoanAccounts : {"Lo"}");


				WriteLine("----------------Request End------------------------");

				var getLoanAccounts = await _customerDetailService.GetLoanAccountsAsync("Lo");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (getLoanAccounts != null)
				{
					foreach (var item in getLoanAccounts)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null AvailableBalance");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of GetLoanAccounts is Null.");
				}


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetLoanAccountsAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetLoanAccountsAsync Response"

				WriteLine();
				WriteLine("----------------Executing GetLoanAccountsAsync  Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"LoanAccounts : {"Loan"}");


				WriteLine("----------------Request End------------------------");

				var getLoanAccounts = await _customerDetailService.GetLoanAccountsAsync("Loan");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (getLoanAccounts != null)
				{
					foreach (var item in getLoanAccounts)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null AvailableBalance");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of GetLoanAccounts is Null.");
				}


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetLoanAccountsAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetLoanAccountsAsync Response"

				WriteLine();
				WriteLine("----------------Executing GetLoanAccountsAsync Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"LoanAccounts : {""}");


				WriteLine("----------------Request End------------------------");

				var getLoanAccounts = await _customerDetailService.GetLoanAccountsAsync("");


				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (getLoanAccounts != null)
				{
					foreach (var item in getLoanAccounts)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null AvailableBalance");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of GetLoanAccounts is Null.");
				}


				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetCardImage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetCardImage Response"

				WriteLine();
				WriteLine("----------------Executing Get Card Image -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cif : {"329887"}");

				WriteLine("----------------Request End------------------------");

				var cardImages = await _authenticationService.GetCardImage("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (cardImages != null)
				{
					foreach (var item in cardImages)
					{
						WriteLine(Convert.ToString(item?.CardImageNo) ?? "Null CardImageNo");
						WriteLine(item?.ExpiryDate ?? "Null ExpiryDate");
						WriteLine(item?.Pan ?? "Null Pan");
						WriteLine(item?.Track2 ?? "Null Track2");
					}
				}
				else
				{
					WriteLine("List of CardImages is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCardImageTimeoutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetCardImage Response"

				WriteLine();
				WriteLine("----------------Executing Get Card Image Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cif : {"32"}");

				WriteLine("----------------Request End------------------------");

				var cardImages = await _authenticationService.GetCardImage("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (cardImages != null)
				{
					foreach (var item in cardImages)
					{
						WriteLine(Convert.ToString(item?.CardImageNo) ?? "Null CardImageNo");
						WriteLine(item?.ExpiryDate ?? "Null ExpiryDate");
						WriteLine(item?.Pan ?? "Null Pan");
						WriteLine(item?.Track2 ?? "Null Track2");
					}
				}
				else
				{
					WriteLine("List of CardImages is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCardImageErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetCardImage Response"

				WriteLine();
				WriteLine("----------------Executing Get Card Image Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cif : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var cardImages = await _authenticationService.GetCardImage("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (cardImages != null)
				{
					foreach (var item in cardImages)
					{
						WriteLine(Convert.ToString(item?.CardImageNo) ?? "Null CardImageNo");
						WriteLine(item?.ExpiryDate ?? "Null ExpiryDate");
						WriteLine(item?.Pan ?? "Null Pan");
						WriteLine(item?.Track2 ?? "Null Track2");
					}
				}
				else
				{
					WriteLine("List of CardImages is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCardImageEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "GetCardImage Response"

				WriteLine();
				WriteLine("----------------Executing Get Card Image Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cif : {""}");

				WriteLine("----------------Request End------------------------");

				var cardImages = await _authenticationService.GetCardImage("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (cardImages != null)
				{
					foreach (var item in cardImages)
					{
						WriteLine(Convert.ToString(item?.CardImageNo) ?? "Null CardImageNo");
						WriteLine(item?.ExpiryDate ?? "Null ExpiryDate");
						WriteLine(item?.Pan ?? "Null Pan");
						WriteLine(item?.Track2 ?? "Null Track2");
					}
				}
				else
				{
					WriteLine("List of CardImages is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetDepositAccount(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetDepositAccount Response"

				WriteLine();
				WriteLine("----------------Executing Get Deposit Account -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"329887"}");

				WriteLine("----------------Request End------------------------");

				var depositAccounts = await _customerDetailService.GetDepositAccount("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (depositAccounts != null)
				{
					foreach (var item in depositAccounts)
					{
						WriteLine(Convert.ToString(item?.Balance) ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of Deposit Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDepositAccountTimeoutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetDepositAccount Response"

				WriteLine();
				WriteLine("----------------Executing Get Deposit Account Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"32"}");

				WriteLine("----------------Request End------------------------");

				var depositAccounts = await _customerDetailService.GetDepositAccount("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (depositAccounts != null)
				{
					foreach (var item in depositAccounts)
					{
						WriteLine(Convert.ToString(item?.Balance) ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of Deposit Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDepositAccountErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetDepositAccount Response"

				WriteLine();
				WriteLine("----------------Executing Get Deposit Account Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var depositAccounts = await _customerDetailService.GetDepositAccount("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (depositAccounts != null)
				{
					foreach (var item in depositAccounts)
					{
						WriteLine(Convert.ToString(item?.Balance) ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of Deposit Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetDepositAccountEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetDepositAccount Response"

				WriteLine();
				WriteLine("----------------Executing Get Deposit Account Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {""}");

				WriteLine("----------------Request End------------------------");

				var depositAccounts = await _customerDetailService.GetDepositAccount("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (depositAccounts != null)
				{
					foreach (var item in depositAccounts)
					{
						WriteLine(Convert.ToString(item?.Balance) ?? "Null Balance");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
					}
				}
				else
				{
					WriteLine("List of Deposit Accounts is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetCreditCard(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCard Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit Card-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Customer Identifier : {"329887"}");

				WriteLine("----------------Request End------------------------");

				var CreditCard = await _transactionService.GetCreditCardsAsync("329887");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (CreditCard != null)
				{
					foreach (var item in CreditCard)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.CardLimit ?? "Null CardLimit");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine(item?.StatementMinimumDue ?? "Null StatementMinimumDue");
					}
				}
				else
				{
					WriteLine("List of Credit Card is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCreditCardTimeoutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCard Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit CardAsync Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Customer Identifier : {"32"}");

				WriteLine("----------------Request End------------------------");

				var CreditCard = await _transactionService.GetCreditCardsAsync("32");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (CreditCard != null)
				{
					foreach (var item in CreditCard)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.CardLimit ?? "Null CardLimit");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine(item?.StatementMinimumDue ?? "Null StatementMinimumDue");
					}
				}
				else
				{
					WriteLine("List of Credit Card is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCreditCardErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCard Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit Card Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Customer Identifier : {"3298"}");

				WriteLine("----------------Request End------------------------");

				var CreditCard = await _transactionService.GetCreditCardsAsync("3298");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (CreditCard != null)
				{
					foreach (var item in CreditCard)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.CardLimit ?? "Null CardLimit");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine(item?.StatementMinimumDue ?? "Null StatementMinimumDue");
					}
				}
				else
				{
					WriteLine("List of Credit Card is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCreditCardEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetCreditCard Response"

				WriteLine();
				WriteLine("----------------Executing Get Credit Card Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Customer Identifier : {""}");

				WriteLine("----------------Request End------------------------");

				var CreditCard = await _transactionService.GetCreditCardsAsync("");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (CreditCard != null)
				{
					foreach (var item in CreditCard)
					{
						WriteLine(item?.Balance ?? "Null Balance");
						WriteLine(item?.CardLimit ?? "Null CardLimit");
						WriteLine(item?.Currency ?? "Null Currency");
						WriteLine(item?.CurrencyCode ?? "Null CurrencyCode");
						WriteLine(item?.Number ?? "Null Number");
						WriteLine(item?.Type ?? "Null Type");
						WriteLine(item?.StatementMinimumDue ?? "Null StatementMinimumDue");
					}
				}
				else
				{
					WriteLine("List of Credit Card is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task VerifyPin(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "VerifyPin Response"

				WriteLine();
				WriteLine("----------------Executing VerifyPin -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"4689156465896556=8913235613256921"}");
				WriteLine($"Pin : {"F65EA78723121055"}");
				WriteLine($"ICC Request : {"AFHHJSNNNCEEIKK"}");

				WriteLine("----------------Request End------------------------");

				var pin = await _authenticationService.VerifyPin("4689156465896556=8913235613256921", "F65EA78723121055", "AFHHJSNNNCEEIKK");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(pin?.CustomerIdentifier ?? "Null CustomerIdentifier");
				WriteLine(pin?.ResponseCode ?? "Null ResponseCode");
				WriteLine(pin?.AuthCode ?? "Null AuthCode");
				WriteLine(pin?.IccResponse ?? "Null IccResponse");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task VerifyPinTimeoutError(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "VerifyPin Response"

				WriteLine();
				WriteLine("----------------Executing VerifyPin Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"46"}");
				WriteLine($"Pin : {"F6"}");
				WriteLine($"ICC Request : {"AF"}");

				WriteLine("----------------Request End------------------------");

				var pin = await _authenticationService.VerifyPin("46", "F6", "AF");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(pin?.CustomerIdentifier ?? "Null CustomerIdentifier");
				WriteLine(pin?.ResponseCode ?? "Null ResponseCode");
				WriteLine(pin?.AuthCode ?? "Null AuthCode");
				WriteLine(pin?.IccResponse ?? "Null IccResponse");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task VerifyPinErrorMessage(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "VerifyPin Response"

				WriteLine();
				WriteLine("----------------Executing VerifyPin Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {"4646"}");
				WriteLine($"Pin : {"F6F6"}");
				WriteLine($"ICC Request : {"AFAF"}");

				WriteLine("----------------Request End------------------------");

				var pin = await _authenticationService.VerifyPin("4646", "F6F6", "AFAF");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(pin?.CustomerIdentifier ?? "Null CustomerIdentifier");
				WriteLine(pin?.ResponseCode ?? "Null ResponseCode");
				WriteLine(pin?.AuthCode ?? "Null AuthCode");
				WriteLine(pin?.IccResponse ?? "Null IccResponse");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task VerifyPinEmptyFields(UnityContainer container)
		{
			try
			{
				var _authenticationService = container.Resolve<IAuthenticationService>();

				#region "VerifyPin Response"

				WriteLine();
				WriteLine("----------------Executing VerifyPin Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"Track 2 : {""}");
				WriteLine($"Pin : {""}");
				WriteLine($"ICC Request : {""}");

				WriteLine("----------------Request End------------------------");

				var pin = await _authenticationService.VerifyPin("", "", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(pin?.CustomerIdentifier ?? "Null CustomerIdentifier");
				WriteLine(pin?.ResponseCode ?? "Null ResponseCode");
				WriteLine(pin?.AuthCode ?? "Null AuthCode");
				WriteLine(pin?.IccResponse ?? "Null IccResponse");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetCustomerIdentifier(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerIdentifier Response"

				WriteLine();
				WriteLine("----------------Executing Get Customer Identifier-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cardNumber : {"229887"}");
				WriteLine($"cardType : {"Private"}");

				WriteLine("----------------Request End------------------------");

				var customerIdentifier = await _customerDetailService.GetCustomerIdentifierAsync("229887", CardType.CreditCardDebitCard);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerIdentifier?.CustomerId ?? "Null CustomerId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerIdentifierTimeoutError(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Customer Identifier Time out Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cardNumber : {"22"}");
				WriteLine($"cardType : {"Pr"}");


				WriteLine("----------------Request End------------------------");

				var customerIdentifier = await _customerDetailService.GetCustomerIdentifierAsync("22", CardType.CreditCardDebitCard);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerIdentifier?.CustomerId ?? "Null CustomerId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerIdentifierErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing GetCustomerDetail Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cardNumber : {"2251"}");
				WriteLine($"cardType : {"Priv"}");


				WriteLine("----------------Request End------------------------");

				var customerIdentifier = await _customerDetailService.GetCustomerIdentifierAsync("2222", CardType.CreditCardDebitCard);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerIdentifier?.CustomerId ?? "Null CustomerId");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetCustomerIdentifierEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerDetailService = container.Resolve<ICustomerService>();

				#region "GetCustomerDetail Response"

				WriteLine();
				WriteLine("----------------Executing Get Customer Identifier Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"cardNumber : {""}");
				WriteLine($"cardType : {""}");



				WriteLine("----------------Request End------------------------");

				var customerIdentifier = await _customerDetailService.GetCustomerIdentifierAsync("", CardType.CreditCardDebitCard);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(customerIdentifier?.CustomerId ?? "Null Address1");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task IssueCheckBookAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Issue ChequeBook"

				WriteLine();
				WriteLine("----------------Executing Issue Cheque Book-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"accountBranch : {"229887"}");
				WriteLine($"accountNumber : {"458785854744"}");
				WriteLine($"currencyCode : {"10145"}");
				WriteLine($"issueDate : {"11 sep 2011"}");
				WriteLine($"numberOfCheques : {"50"}");

				WriteLine("----------------Request End------------------------");

				var issueCheckBook = await _transactionService.IssueChequeBookAsync("229887", "458785854744", "10145", DateTime.Now, "50");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(issueCheckBook?.StartingChequeNumber ?? "Null StartingChequeNumber");
				WriteLine(issueCheckBook?.HostTransCode ?? "Null HostTransCode");
				WriteLine(issueCheckBook?.RoutingCode ?? "Null RoutingCode");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task DliverChequeBookAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Issue ChequeBook"

				WriteLine();
				WriteLine("----------------Executing Deliver Cheque Book Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"hostTransCode : {"229887"}");
				WriteLine($"accountBranchCode : {"458785854744"}");

				WriteLine("----------------Request End------------------------");

				var deliverCheaqueBook = await _transactionService.DliverChequeBookAsync("229887", "458785854744");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(deliverCheaqueBook?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task DliverChequeBookAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Issue ChequeBook"

				WriteLine();
				WriteLine("----------------Executing Deliver Cheque Book Async Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"hostTransCode : {"22"}");
				WriteLine($"accountBranchCode : {"45"}");

				WriteLine("----------------Request End------------------------");

				var deliverCheaqueBook = await _transactionService.DliverChequeBookAsync("22", "45");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(deliverCheaqueBook?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task DliverChequeBookAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Issue ChequeBook"

				WriteLine();
				WriteLine("----------------Executing Deliver Cheque Book Async Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"hostTransCode : {"2234"}");
				WriteLine($"accountBranchCode : {"4655"}");

				WriteLine("----------------Request End------------------------");

				var deliverCheaqueBook = await _transactionService.DliverChequeBookAsync("2234", "4655");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(deliverCheaqueBook?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task DliverChequeBookAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Issue ChequeBook"

				WriteLine();
				WriteLine("----------------Executing Deliver Cheque Book Async Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"hostTransCode : {""}");
				WriteLine($"accountBranchCode : {""}");

				WriteLine("----------------Request End------------------------");

				var deliverCheaqueBook = await _transactionService.DliverChequeBookAsync("", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(deliverCheaqueBook?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task SendEmailAsync(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "SendEmailAsync"

				WriteLine();
				WriteLine("----------------Executing Send Email Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"attachment : {"546444654646546546546464564564654"}");
				WriteLine($"fileName : {"uptc"}");
				WriteLine($"emailType : {"LC"}");
				WriteLine($"cutomerId : {"32658985"}");
				WriteLine($"customerName : {"Muhammad"}");
				WriteLine($"requestDate : {"15 september 2017"}");

				var bArry = Convert.ToBase64String(Encoding.UTF8.GetBytes("Halo World"));
				var attachment = new Attachment() { Content = bArry, FileName = "LC" };

				WriteLine("----------------Request End------------------------");

				var sendEmail = await _communicationService.SendEmailAsync(attachment, EmailType.LC, "32658985", "Muhammad", DateTime.Now, "", "0", "5555555555555555");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendEmail?.EmailSessionId ?? "Null ReferenceNumber");
				WriteLine(sendEmail?.Status ?? "Null Status");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GenerateTSNAsync(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "GenerateTSNAsync"

				WriteLine();
				WriteLine("----------------Executing Generate TSN Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");


				WriteLine("----------------Request End------------------------");

				var GenerateTSN = await _communicationService.GenerateTSNAsync();

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(GenerateTSN?.value ?? "Null value");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GenerateTSNAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "GenerateTSNAsync"

				WriteLine();
				WriteLine("----------------Executing Generate TSN Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");


				WriteLine("----------------Request End------------------------");

				var GenerateTSN = await _communicationService.GenerateTSNAsync();

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(GenerateTSN?.value ?? "Null value");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GenerateTSNAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "GenerateTSNAsync"

				WriteLine();
				WriteLine("----------------Executing Generate TSN Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");


				WriteLine("----------------Request End------------------------");

				var GenerateTSN = await _communicationService.GenerateTSNAsync();

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(GenerateTSN?.value ?? "Null value");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GenerateTSNAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "GenerateTSNAsync"

				WriteLine();
				WriteLine("----------------Executing Generate TSN Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");


				WriteLine("----------------Request End------------------------");

				var GenerateTSN = await _communicationService.GenerateTSNAsync();

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(GenerateTSN?.value ?? "Null value");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetStatementItemAsync(UnityContainer container)
		{
			try
			{
				var _customerService = container.Resolve<ICustomerService>();

				#region "GetStatementItemAsync"

				WriteLine();
				WriteLine("----------------Executing Get Statement Item Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"statementDateFrom : {"01-05-2011"}");
				WriteLine($"statementDateTo : {"01-05-2018"}");
				WriteLine($"accountNumber : {"4515515616515165"}");
				WriteLine($"numOfTransactions : {"20"}");

				WriteLine("----------------Request End------------------------");

				var GetStatementItem = await _customerService.GetStatementItemAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "3515515616515165", "20");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (GetStatementItem != null)
				{
					foreach (var item in GetStatementItem)
					{
						WriteLine(Convert.ToString(item?.CreditAmount) ?? "Null CreditAmount");
						WriteLine(Convert.ToString(item?.DebitAmount) ?? "Null DebitAmount");
						WriteLine(item?.Description ?? "Null Description");
						//WriteLine(Convert.ToString(item?.RunningBalance) ?? "Null RunningBalance");
						WriteLine(Convert.ToString(item?.TransactionDate) ?? "Null TransactionDate");
						WriteLine(Convert.ToString(item?.ValueDate) ?? "Null ValueDate");
					}
				}
				else
				{
					WriteLine("List of Statement Item is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementItemAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _customerService = container.Resolve<ICustomerService>();

				#region "GetStatementItemAsyncTimeoutError"

				WriteLine();
				WriteLine("----------------Executing Get Get Statement Item Async Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"statementDateFrom : {"01"}");
				WriteLine($"statementDateTo : {"01"}");
				WriteLine($"accountNumber : {"45"}");
				WriteLine($"numOfTransactions : {"20"}");

				WriteLine("----------------Request End------------------------");

				var GetStatementItem = await _customerService.GetStatementItemAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "4515515616515165", "20");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (GetStatementItem != null)
				{
					foreach (var item in GetStatementItem)
					{
						WriteLine(Convert.ToString(item?.CreditAmount) ?? "Null CreditAmount");
						WriteLine(Convert.ToString(item?.DebitAmount) ?? "Null DebitAmount");
						WriteLine(item?.Description ?? "Null Description");
						//WriteLine(Convert.ToString(item?.RunningBalance) ?? "Null RunningBalance");
						WriteLine(Convert.ToString(item?.TransactionDate) ?? "Null TransactionDate");
						WriteLine(Convert.ToString(item?.ValueDate) ?? "Null ValueDate");
					}
				}
				else
				{
					WriteLine("List of Statement Item is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementItemAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _customerService = container.Resolve<ICustomerService>();

				#region "GetStatementItemAsyncErrorMessage"

				WriteLine();
				WriteLine("----------------Executing Get Statement Item Async Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"statementDateFrom : {"01-0"}");
				WriteLine($"statementDateTo : {"01-0"}");
				WriteLine($"accountNumber : {"4515"}");
				WriteLine($"numOfTransactions : {"20"}");

				WriteLine("----------------Request End------------------------");

				var GetStatementItem = await _customerService.GetStatementItemAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "4515515616515165", "20");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (GetStatementItem != null)
				{
					foreach (var item in GetStatementItem)
					{
						WriteLine(Convert.ToString(item?.CreditAmount) ?? "Null CreditAmount");
						WriteLine(Convert.ToString(item?.DebitAmount) ?? "Null DebitAmount");
						WriteLine(item?.Description ?? "Null Description");
						//WriteLine(Convert.ToString(item?.RunningBalance) ?? "Null RunningBalance");
						WriteLine(Convert.ToString(item?.TransactionDate) ?? "Null TransactionDate");
						WriteLine(Convert.ToString(item?.ValueDate) ?? "Null ValueDate");
					}
				}
				else
				{
					WriteLine("List of Statement Item is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementItemAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _customerService = container.Resolve<ICustomerService>();

				#region "GetStatementItemAsyncEmptyFields"

				WriteLine();
				WriteLine("----------------Executing Get Get Statement Item Async Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"statementDateFrom : {"01"}");
				WriteLine($"statementDateTo : {"01"}");
				WriteLine($"accountNumber : {"45"}");
				WriteLine($"numOfTransactions : {"20"}");

				WriteLine("----------------Request End------------------------");

				var GetStatementItem = await _customerService.GetStatementItemAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "4515515616515165", "20");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				if (GetStatementItem != null)
				{
					foreach (var item in GetStatementItem)
					{
						WriteLine(Convert.ToString(item?.CreditAmount) ?? "Null CreditAmount");
						WriteLine(Convert.ToString(item?.DebitAmount) ?? "Null DebitAmount");
						WriteLine(item?.Description ?? "Null Description");
						//WriteLine(Convert.ToString(item?.RunningBalance) ?? "Null RunningBalance");
						WriteLine(Convert.ToString(item?.TransactionDate) ?? "Null TransactionDate");
						WriteLine(Convert.ToString(item?.ValueDate) ?? "Null ValueDate");
					}
				}
				else
				{
					WriteLine("List of Statement Item is Null.");
				}

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ApplyStatementChargesAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ApplyStatementChargesAsync"

				WriteLine();
				WriteLine("----------------Executing Apply Statement Charges Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"statementDateFrom : {"12-05-2010"}");
				WriteLine($"statementDateTo : {"10-02-2018"}");
				WriteLine($"branchCode : {"EF582"}");
				WriteLine($"accountNumber : {"5215552512515252"}");
				WriteLine($"customerId : {"3255455654"}");
				WriteLine("----------------Request End------------------------");

				var ApplyStatementCharge = await _transactionService.ApplyStatementChargesAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "5215552512515252", "3255455654", "12345");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(ApplyStatementCharge?.ReferenceNum ?? "Null ReferenceNum");
				WriteLine(ApplyStatementCharge?.Status ?? "Null Status");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}


		static async Task GetStatementChargesAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetStatementChargesAsync"

				WriteLine();
				WriteLine("----------------Executing Apply Statement Charges Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"statementDateFrom : {"12-05-2010"}");
				WriteLine($"statementDateTo : {"10-02-2018"}");
				WriteLine($"chargeIndicator : {"0"}");
				WriteLine($"accountNumber : {"5215552512515252"}");
				WriteLine($"category : {"0"}");
				WriteLine("----------------Request End------------------------");

				var getStatementCharges = await _transactionService.GetStatementChargesAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "0", "5215552512515252", "0", "1");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(getStatementCharges?.ChargeAmount ?? "Null ChargeAmount");
				WriteLine(getStatementCharges?.ChargeCurrency ?? "Null ChargeCurrency");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementChargesAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetStatementChargesAsync"

				WriteLine();
				WriteLine("----------------Executing Apply Statement Charges Timeout Error Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"statementDateFrom : {"12"}");
				WriteLine($"statementDateTo : {"10"}");
				WriteLine($"chargeIndicator : {"0"}");
				WriteLine($"accountNumber : {"52"}");
				WriteLine($"category : {"0"}");
				WriteLine("----------------Request End------------------------");

				var getStatementCharges = await _transactionService.GetStatementChargesAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "0", "52", "0", "1");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(getStatementCharges?.ChargeAmount ?? "Null ChargeAmount");
				WriteLine(getStatementCharges?.ChargeCurrency ?? "Null ChargeCurrency");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementChargesAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetStatementChargesAsync"

				WriteLine();
				WriteLine("----------------Executing Apply Statement Charges Error Message Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"statementDateFrom : {"12-0"}");
				WriteLine($"statementDateTo : {"10-1"}");
				WriteLine($"chargeIndicator : {"0"}");
				WriteLine($"accountNumber : {"5232"}");
				WriteLine($"category : {"0"}");
				WriteLine("----------------Request End------------------------");

				var getStatementCharges = await _transactionService.GetStatementChargesAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "0", "5232", "0", "1");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(getStatementCharges?.ChargeAmount ?? "Null ChargeAmount");
				WriteLine(getStatementCharges?.ChargeCurrency ?? "Null ChargeCurrency");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task GetStatementChargesAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetStatementChargesAsync"

				WriteLine();
				WriteLine("----------------Executing Apply Statement Charges Empty Fields Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"statementDateFrom : {""}");
				WriteLine($"statementDateTo : {""}");
				WriteLine($"chargeIndicator : {""}");
				WriteLine($"accountNumber : {""}");
				WriteLine($"category : {""}");
				WriteLine("----------------Request End------------------------");

				var getStatementCharges = await _transactionService.GetStatementChargesAsync(DateTime.Now.AddMonths(-1), DateTime.Now, "", "", "", "");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(getStatementCharges?.ChargeAmount ?? "Null ChargeAmount");
				WriteLine(getStatementCharges?.ChargeCurrency ?? "Null ChargeCurrency");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ChequeDepositAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ChequeDepositAsync"

				WriteLine();
				WriteLine("----------------Executing Cheque Deposit Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"chequeDate : {"2017-07-05"}");
				WriteLine($"settlementDate : {"2017-06-04"}");
				WriteLine($"sessionTime : {"15"}");
				WriteLine($"settlementTime : {"12:14"}");
				WriteLine($"issuerBankRtn : {"yess"}");
				WriteLine($"accountNumber : {"52654654644646"}");
				WriteLine($"sequenceNumber : {"123456789"}");
				WriteLine($"amount : {"15600"}");
				WriteLine($"corrIndicator : {"hijklmnopqrst"}");
				WriteLine($"preBankRtn : {"sdfdf"}");
				WriteLine($"endorsementDate : {"2010-05-04"}");
				WriteLine($"depositIban : {"456464654654654564654"}");
				WriteLine($"payeeName : {"Muhammad"}");
				WriteLine($"frontImage : {"5215552512515252"}");
				WriteLine($"backImage : {"sfdfsfdsfdsfsdfdsf"}");
				WriteLine($"frontImageLength : {"5321"}");
				WriteLine($"backImageLength : {"5242"}");
				WriteLine("----------------Request End------------------------");

				var chequeDeposit = await _transactionService.ChequeDepositAsync(DateTime.Now, DateTime.Now, "15", "12:14", "yess",
					"52654654644646", "123456789", "15600", "hijklmnopqrst", "sdfdf", DateTime.Now, "456464654654654564654", "Muhammad",
					"5215552512515252", "sfdfsfdsfdsfsdfdsf", "5321", "5242");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(chequeDeposit?.ReferenceNumber ?? "Null ReferenceNumber");
				WriteLine(chequeDeposit?.Status ?? "Null Status");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task GetChequePrintingChargeAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "GetChequePrintingChargeAsync"

				WriteLine();
				WriteLine("----------------Executing Get Cheque Printing Charge Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"chargeIndicator : {"1"}");
				WriteLine($"accountNumber : {"5223121131321213"}");
				WriteLine($"numberOfBooks : {"1"}");

				WriteLine("----------------Request End------------------------");

				var chequePrintingCharge = await _transactionService.GetChequePrintingChargeAsync(ChargeIndicator.DeductionFromAccount, "5223121131321213", "1");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(chequePrintingCharge?.ChargeAmount ?? 0);
				WriteLine(chequePrintingCharge?.ChargeCurrency ?? "Null Currency");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ReverseChargeAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ReverseChargeAsync"

				WriteLine();
				WriteLine("----------------Executing Reverse Charge Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"transactionReferenceNumber : {"9898198"}");
				WriteLine($"chargeType : {"1"}");

				WriteLine("----------------Request End------------------------");

				var reverseCharge = await _transactionService.ReverseChargeAsync("9898198", ChargeType.StatementCharges);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reverseCharge?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseChargeAsyncTimeoutError(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ReverseChargeAsyncTimeoutError"

				WriteLine();
				WriteLine("----------------Executing Reverse Charge Async Timeout Error-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"transactionReferenceNumber : {"98"}");
				WriteLine($"chargeType : {"1"}");

				WriteLine("----------------Request End------------------------");

				var reverseCharge = await _transactionService.ReverseChargeAsync("98", ChargeType.StatementCharges);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reverseCharge?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseChargeAsyncErrorMessage(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ReverseChargeAsyncErrorMessage"

				WriteLine();
				WriteLine("----------------Executing Reverse Charge Async Error Message-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"transactionReferenceNumber : {"9898"}");
				WriteLine($"chargeType : {"1"}");

				WriteLine("----------------Request End------------------------");

				var reverseCharge = await _transactionService.ReverseChargeAsync("9898", ChargeType.StatementCharges);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reverseCharge?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}
		static async Task ReverseChargeAsyncEmptyFields(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "ReverseChargeAsyncEmptyFields"

				WriteLine();
				WriteLine("----------------Executing Reverse Charge Async Empty Fields-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"transactionReferenceNumber : {""}");
				WriteLine($"chargeType : {""}");

				WriteLine("----------------Request End------------------------");

				var reverseCharge = await _transactionService.ReverseChargeAsync("", ChargeType.StatementCharges);

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(reverseCharge?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task ProductInfoAsync(UnityContainer container)
		{
			try
			{
				var _customerService = container.Resolve<ICustomerService>();

				#region "ProductInfoAsync"

				WriteLine();
				WriteLine("----------------Executing Product Info Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"leadType : {"9898198"}");
				WriteLine($"mobileNumber : {"524564654464"}");
				WriteLine($"email : {"abc@gmail.com"}");

				WriteLine("----------------Request End------------------------");

				var productInfo = await _customerService.ProductInfoAsync("9898198", "524564654464", "abc@gmail.com", "0");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(productInfo?.ReferenceNumber ?? "Null ReferenceNumber");
				WriteLine(productInfo?.Status ?? "Null Status");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task CoordinationNumberAsync(UnityContainer container)
		{
			try
			{
				var _transactionService = container.Resolve<ITransactionService>();

				#region "Coordination Number Async"

				WriteLine();
				WriteLine("----------------Executing Coordination Number Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");


				WriteLine("----------------Request End------------------------");

				var coordinationNumber = await _transactionService.CoordinationNumberAsync();

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(coordinationNumber?.Number ?? "Null Number");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task InsertEventAsync(UnityContainer container)
		{
			try
			{
				var _channelManagementService = container.Resolve<IChannelManagementService>();

				#region "Insert Event Async"

				WriteLine();
				WriteLine("----------------Executing Insert Event Async-----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");
				WriteLine($"Event : {"Supervisor"}");
				WriteLine($"value : {"On"}");

				WriteLine("----------------Request End------------------------");

				var insertEvent = await _channelManagementService.InsertEventAsync("Supervisor", "On");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(insertEvent?.Result ?? "Null Result");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}


		static async Task TransactionNotificationAsync(UnityContainer container)
		{
			try
			{
				var _transationService = container.Resolve<ITransactionService>();

				#region "Transaction Notification Async"

				WriteLine();
				WriteLine("----------------Executing Transaction Notification Async -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"notificationType : {"25"}");
				WriteLine($"transactionResponseCode : {"Private"}");
				WriteLine($"cardNumber : {"5775554645464646"}");
				WriteLine($"amount : {"2020"}");
				WriteLine($"transactionReference : {"0087"}");
				WriteLine($"accountNumber : {"5254645644644"}");
				WriteLine($"customerIdentifier : {"52325232523252"}");
				WriteLine($"TransactionStatus : {"Successful"}");
				WriteLine($"Reason : {"Primary"}");
				WriteLine($"StatementMonths : {"March"}");
				WriteLine($"ChequeLeaves : {"20"}");
				WriteLine($"ResponseCode : {"051"}");

				WriteLine("----------------Request End------------------------");

				//var transNote = await _transationService.TransactionNotificationAsync(Interface.TransactionType.RTChequeDeposit, "Private", "5775554645464646", "2020", "0087", "5254645644644", "52325232523252", Interface.Enums.TransactionStatus.Successful, "Primary", "March", "20", "051");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				//WriteLine(transNote?.ReferenceNumber ?? "Null ReferenceNumber");
				//WriteLine(transNote?.Status ?? "Null Status");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task SendSMSAsync(UnityContainer container)
		{
			try
			{
				var _communicationService = container.Resolve<ICommunicationService>();

				#region "Send SMS Async"

				WriteLine();
				WriteLine("----------------Executing Send SMS Async -----------------");
				WriteLine();

				WriteLine("----------------Request Start----------------------");

				WriteLine($"customerIdentifier : {"52325232523252"}");
				WriteLine($"type : {"3"}");
				WriteLine($"referenceNumber : {"5775554645464646"}");
				WriteLine($"IBAN : {"2020"}");

				WriteLine("----------------Request End------------------------");

				var sendSMS = await _communicationService.SendSmsAsync("52325232523252", SmsType.IbanSms, "5775554645464646", "2020");

				WriteLine();
				WriteLine("----------------Response Start----------------------");

				WriteLine(sendSMS?.ReferenceNumber ?? "Null ReferenceNumber");

				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}


		#region Private Helpers

		private static async Task ExecuteAsync(Func<Task> func)
		{
			try
			{
				await func();
			}
			catch (Exception e)
			{
				WriteLine();
				WriteLine(e);
				WriteLine();
			}
		}

		private static void InitializeServices(IServiceManager serviceManager)
		{
			serviceManager.Acquirer = BuildAcquirer(TerminalConfiguration.Section);
			serviceManager.Terminal = BuildTerminal(TerminalConfiguration.Section);
		}

		private static Acquirer BuildAcquirer(TerminalSection terminal) => new Acquirer
		{
			AcquiringInstitutionId = terminal.AcquiringInstitutionId,
			BranchId = terminal.BranchId
		};

		private static Terminal BuildTerminal(TerminalSection terminal) => new Terminal
		{
			Id = terminal.Id,
			MachineSerialNo = terminal.MachineSerialNo,
			CountryCode = terminal.CountryCode,
			TerminalLanguage = terminal.TerminalLanguage,
			MerchantId = terminal.MerchantId,
			Type = terminal.Type,
			OwnerName = terminal.OwnerName,
			StateName = terminal.StateName,
			BranchId = terminal?.BranchId,
			Platform = terminal?.Platform,
		};

		#endregion
	}
}