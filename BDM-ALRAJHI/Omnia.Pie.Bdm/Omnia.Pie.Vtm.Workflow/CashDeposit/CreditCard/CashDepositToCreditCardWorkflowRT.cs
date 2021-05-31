using System;
using System.Linq;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps;
using Omnia.Pie.Vtm.Workflow.Common.Steps;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Communication.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Services.Interface;
using System.Configuration;
using Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard.Steps;
using Omnia.Pie.Vtm.ServicesNdc.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard
{
	class CashDepositToCreditCardWorkflowRT : Workflow
	{
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly ICallTellerViewModel _callTellerViewModel;
		private readonly CreditCardDetailsStep _cashDepositAccountSelectionStep;
		private readonly ManualAccountEntryStep _manualAccountEntryStep;
		private readonly AccountConfirmationStep _accountConfirmationStep;
		private readonly DepositConfirmationStep _depositConfirmationStep;
		private readonly CashDepositResultStep _cashDepositResultStep;
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
        private readonly CheckCashAcceptorStep _checkCashAcceptorStep;
        private readonly TaskCompletionSource<bool> _workflowCompletionTask;

		private void SendAccountsData()
		{
            var _context = Context.Get<ICashDepositContext>();
            var cardNumber = _context.CardUsed.Number;
            _callTellerViewModel.Communicator.SendMessage("{MessageType: '5', StatusEnum: '" + (int)StatusEnum.CreditCardPaymentWithCashCardNumber + "', CardNumber: '" + cardNumber + "'}");
		}

		private void SendSelectedAccountData()
		{
			_callTellerViewModel.Communicator.SendStatus(StatusEnum.CreditCardPaymentWithCashCardNumberConfirmed);
		}

		private void CheckForCancel()
		{
			if (Context.Get<ICashDepositContext>().IsCanceled)
			{
				_workflowCompletionTask.TrySetResult(false);
			}
		}

		private void SendDenominationMessage()
		{
			int totalAmount = 0;
			int totalNotes = 0;
			string TenNote = "", TwentyNote = "", FiftyNote = "", HundredNote = "", TwoHundredNote = "", FiveHundredNote = "", OneThousandNote = "";

			foreach (var item in Context.Get<ICashDepositContext>().DepositedCash)
			{
				if (item.Denomination == 10)
					TenNote = item.Quantity.ToString();
				else if (item.Denomination == 20)
					TwentyNote = item.Quantity.ToString();
				else if (item.Denomination == 50)
					FiftyNote = item.Quantity.ToString();
				else if (item.Denomination == 100)
					HundredNote = item.Quantity.ToString();
				else if (item.Denomination == 200)
					TwoHundredNote = item.Quantity.ToString();
				else if (item.Denomination == 500)
					FiveHundredNote = item.Quantity.ToString();
				else if (item.Denomination == 1000)
					OneThousandNote = item.Quantity.ToString();

				totalAmount += item.Amount;
				totalNotes += item.Quantity;
			}

			string denominationMessage = $@"{{ 
                                                MessageType: '5', 
                                                StatusEnum: '{(int)StatusEnum.CreditCardPaymentWithCashAmountEntered}', 
                                                TenNote: '{TenNote}', 
                                                TwentyNote:'{TwentyNote}', 
                                                FiftyNote: '{FiftyNote}', 
                                                OneHundredNote: '{HundredNote}', 
                                                TwoHundredNote: '{TwoHundredNote}', 
                                                FiveHundredNote: '{FiveHundredNote}', 
                                                OneThousandNote: '{OneThousandNote}'
                                            }}";

			_callTellerViewModel.Communicator.SendMessage(denominationMessage);

		}

		public CashDepositToCreditCardWorkflowRT(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.Financial, "Cash Deposit Credit Card-RT");

			_callTellerViewModel = _container.Resolve<ICallTellerViewModel>();
			_callTellerViewModel.Communicator.RTMessageReceived += _communicator_RTMessageReceived;

			_workflowCompletionTask = new TaskCompletionSource<bool>();
			_receiptFormatter = receiptFormatter;

			_cashDepositAccountSelectionStep = container.Resolve<CreditCardDetailsStep>();
			_manualAccountEntryStep = container.Resolve<ManualAccountEntryStep>();
			_accountConfirmationStep = container.Resolve<AccountConfirmationStep>();
			_depositConfirmationStep = container.Resolve<DepositConfirmationStep>();
			_cashDepositResultStep = container.Resolve<CashDepositResultStep>();
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();
            _checkCashAcceptorStep = _container.Resolve<CheckCashAcceptorStep>();

            AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepConfirmation},{Properties.Resources.StepDenomination}, {Properties.Resources.StepReceiptPrinting}");

			Context = _cashDepositAccountSelectionStep.Context = _manualAccountEntryStep.Context = _accountConfirmationStep.Context = _depositConfirmationStep.Context = _cashDepositResultStep.Context = CreateContext(typeof(CashDepositContext));
			_checkReceiptPrinterStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
				_workflowCompletionTask.TrySetResult(false);
				LoadStandByRT();
			};

			_cashDepositAccountSelectionStep._cardDetailsConfirmedAction = async () =>
			{
				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
						_workflowCompletionTask.TrySetResult(false);
						return;
					}

					if (Context.Get<ICashDepositContext>().ManualAccount)
					{
						await _manualAccountEntryStep.EnterAccountNumber();
					}
					else
					{
						SendAccountsData();
						_accountConfirmationStep._accountDetailsConfirmedAction();
						//await _accountConfirmationStep.ConfirmAccountDetails();
					}
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get card details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
			};

			_manualAccountEntryStep._accountSelectedAction = async () =>
			{
				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
						_workflowCompletionTask.TrySetResult(false);
						return;
					}

					SendAccountsData();
					await _accountConfirmationStep.ConfirmAccountDetails();
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
			};
			_manualAccountEntryStep.BackAction = async () =>
			{
				await _cashDepositAccountSelectionStep.ConfirmCardDetails();
			};

			_accountConfirmationStep._accountDetailsConfirmedAction = async () =>
			{
				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
						_workflowCompletionTask.TrySetResult(false);
						return;
					}
					SendSelectedAccountData();
					LoadWaitScreen();
				}
				catch (DeviceMalfunctionException ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.CollectCash, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						var _cashAcceptor = _container.Resolve<ICashAcceptor>();
						IgnoreExceptions(async () =>
						{
							await _cashAcceptor.RollbackCashAndWaitTakenAsync();
						});
						LoadWaitScreen();
						//await _cashAcceptor.ResetAsync();
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
			};
			_accountConfirmationStep.BackAction = async () =>
			{
				var _context = Context.Get<ICashDepositContext>();
				if (_context.ManualAccount)
				{
					await _manualAccountEntryStep.EnterAccountNumber();
				}
				else
				{
					await _cashDepositAccountSelectionStep.ConfirmCardDetails();
				}
			};

			_depositConfirmationStep._depositConfirmedAction = async () =>
			{
				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{

						IgnoreExceptions(async () =>
						{
							var vmAnimation = _container.Resolve<IAnimationViewModel>();
							vmAnimation.Type(AnimationType.TakeCash);
							_navigator.RequestNavigation(vmAnimation);
							var _cashacceptor = _container.Resolve<ICashAcceptor>();
							await _cashacceptor.RollbackCashAndWaitTakenAsync();
						});

						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
						_workflowCompletionTask.TrySetResult(false);
						return;
					}

                    LoadWaitScreen();
                    SendDenominationMessage();
                    await Task.Delay(2000);
                    _callTellerViewModel.Communicator.SendStatus(StatusEnum.CreditCardPaymentWithCashConfirmed);
                    
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.DeclinedCashDeposit, () =>
					{
						IgnoreExceptions(async () =>
						{
							var vmAnimation = _container.Resolve<IAnimationViewModel>();
							vmAnimation.Type(AnimationType.TakeCash);
							_navigator.RequestNavigation(vmAnimation);
							var _cashacceptor = _container.Resolve<ICashAcceptor>();
							await _cashacceptor.RollbackCashAndWaitTakenAsync();
						});

						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
			};

			_cashDepositResultStep._receiptSelected = async () =>
			{
				try
				{
					LoadWaitScreen();
                    var authCode = new Random().Next(50000).ToString(); ;
                    if (Context.Get<ICashDepositContext>()?.AuthCode != null)
                        authCode = Context.Get<ICashDepositContext>()?.AuthCode;

                    _callTellerViewModel.Communicator.SendMessage("{MessageType: '5', StatusEnum: '" + (int)StatusEnum.CreditCardPaymentWithCashPaymentSuccess + "', FTNumber: '" + authCode + "'}");
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}

				_workflowCompletionTask.TrySetResult(true);
			};

		}

		private async void _communicator_RTMessageReceived(object sender, RTMessageEventArgs e)
		{
			var _context = Context.Get<ICashDepositContext>();
			try
			{
				if (e.MessageCode.Code == (int)StatusEnum.CreditCardPaymentWithCashActivateCashDeposit)
				{
					_logger?.Info("Message for activate cash deposit arrived.");
					await _depositConfirmationStep.DepositCash();
				}

				else if (e.MessageCode.Code == (int)StatusEnum.CreditCardPaymentWithCashConfirmCashDeposit)
				{
                    var _ndcService = _container.Resolve<INdcService>();
                    var _sessionContext = _container.Resolve<ISessionContext>();
                    var res = await _ndcService.CashDepositCCAsync(_sessionContext.CardUsed.CardNumber, Context.Get<ICashDepositContext>().DepositedCash, Context.Get<ICashDepositContext>().TotalAmount.ToString());
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();

                    if (!string.IsNullOrEmpty(res?.AuthCode))
                    {
                        Context.Get<ICashDepositContext>().AuthCode = res.AuthCode;

                        await _cashAcceptor.StoreCashAsync();
                        _cashDepositResultStep._receiptSelected();
                    }
                    else
                    {
                        throw new Exception("Reference No not found.");
                    }
                    
                    _logger?.Info("Message for receipt arrived.");

                    var receiptData = await _receiptFormatter.FormatAsync(new CashDepositToCreditCardReceipt
                    {
                        AuthCode = _context.AuthCode,
                        CardNumber = _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber,
                        Currency = _context.CardUsed.Currency,
                        Denominations = _context.DepositedCash.Select(x => new Framework.Interface.Receipts.Denomination()
                        {
                            Count = x.Quantity,
                            Value = x.Denomination
                        })
                        .ToList(),
                        TransactionStatus = TransactionStatus.Succeeded,
                        CustomerName = _context.CardUsed.Name
                    }, new ReceiptFormattingOptions() { IsMarkupEnabled = true });
                    
					_journal.TransactionSucceeded(_context.AuthCode);
					SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, _context.TotalAmount.ToString(), _context.AuthCode, _context?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
					await _printReceiptStep.PrintReceipt(true, receiptData);

					_callTellerViewModel.Communicator.SendStatus(StatusEnum.CreditCardPaymentWithCashReceiptPrinted);
					_logger?.Info("Message for receipt ended.");
					_workflowCompletionTask.TrySetResult(true);

				}

                else if (e.MessageCode.Code == (int)StatusEnum.CreditCardPaymentWithCashCancelCashDeposit)
                {
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                    _callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
                    IgnoreExceptions(async () =>
                    {
                        await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                    });
                    LoadWaitScreen();

                    _workflowCompletionTask.TrySetResult(false);
                    LoadStandByRT();
                }
            }
			catch (DeviceMalfunctionException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit - Device failure");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.CollectCash, () =>
				{
					var _cashAcceptor = _container.Resolve<ICashAcceptor>();
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					IgnoreExceptions(async () =>
					{
						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					});
					LoadWaitScreen();

					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(true);
			}
			catch (DeviceOperationCanceledException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit - Device timeout");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt, () =>
				{
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(true);
			}
			catch (Exception ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
				{
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(true);
			}
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Workflow: Cash Deposit To Credit Card - RT Assisted");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

                LoadWaitScreen();
                await _checkReceiptPrinterStep.CheckPrinterAsync();
                await _checkCashAcceptorStep.CheckCashAcceptorAsync();

                var _context = Context.Get<ICashDepositContext>();
				await GetCreditCardDetailsAsync();
				_cashDepositAccountSelectionStep.ConfirmCardDetails();
			}
			catch (Exception ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositCreditCard, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, Context.Get<ICashDepositContext>().TotalAmount.ToString(), "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get card details");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
				{
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(false);
				return false;
			}

			return await _workflowCompletionTask.Task;
		}

		private async Task GetCreditCardDetailsAsync()
		{
            _logger?.Info($"Execute Task: Get Credit Card Details");

            var _transactionService = _container.Resolve<ITransactionService>();
			var ct = _container.Resolve<ISessionContext>();
			CreditCardDetailResult _creditCardDetails = null;

			if (!string.IsNullOrEmpty(ct?.CardFdk) && !string.IsNullOrEmpty(ct.EIdNumber))
			{
				_creditCardDetails = await _transactionService.GetCreditCardDetailAsync(_container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
			}
			else
			{
				_creditCardDetails = await _transactionService.GetCreditCardDetailAsync(_container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, string.Empty);
			}

			var _context = Context.Get<ICashDepositContext>();
			_context.CardUsed = new Services.Interface.Entities.CreditCard()
			{
				Name = _container.Resolve<ISessionContext>()?.CardUsed?.AccountName,
				Number = _creditCardDetails.CardNumber,
				Currency = _creditCardDetails.CurrencyCode,
				MinimumPayment = double.Parse(_creditCardDetails.MinimumDueAmount)
			};
		}

		public override void Dispose()
		{
			_logger?.Info("Called dispose");
			_callTellerViewModel.Communicator.RTMessageReceived -= _communicator_RTMessageReceived;
			DisposeSteps();
			//GC.SuppressFinalize(this);
		}
	}
}