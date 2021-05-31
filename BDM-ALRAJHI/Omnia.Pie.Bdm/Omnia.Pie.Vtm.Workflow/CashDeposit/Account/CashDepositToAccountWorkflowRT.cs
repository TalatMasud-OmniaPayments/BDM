using System;
using System.Collections.Generic;
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
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account
{
	public class CashDepositToAccountWorkflowRT : Workflow
	{
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly ICallTellerViewModel _callTellerViewModel;
		private readonly CashDepositAccountSelectionStep _cashDepositAccountSelectionStep;
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
			string linkedAccount = @"{{'MessageType' : '5', 
                                               'StatusEnum' : '{1}', 
                                                'LinkedAccount' : {{ 
                                                                    {0}
                                                }}}}";

			int x = 0;
			string accountData = "";

			foreach (var item in Context.Get<ICashDepositContext>().Accounts)
			{
				accountData += string.Format("'{2}: {0}': '{1}',", item.Type, item.Number, ++x);
			}

			linkedAccount = string.Format(linkedAccount, accountData, ((int)StatusEnum.CashDepositAccountLinkedAccounts).ToString());

			_callTellerViewModel.Communicator.SendMessage(linkedAccount);
		}

		private void SendSelectedAccountData()
		{
			var selectedAccountDetails = @"{{'MessageType' : '5', 'StatusEnum' : '{2}', 'SelectedAccountNumber' : {{'{0}':'{1}'}}}}";
			selectedAccountDetails = string.Format(selectedAccountDetails, Context.Get<ICashDepositContext>().SelectedAccount.Type.ToLower(), Context.Get<ICashDepositContext>().SelectedAccount.Number, 102);
			_callTellerViewModel.Communicator.SendMessage(selectedAccountDetails);
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
			string TenNote = "0", TwentyNote = "0", FiftyNote = "0", HundredNote = "0", TwoHundredNote = "0", FiveHundredNote = "0", OneThousandNote = "0";

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

			_callTellerViewModel.Communicator.SendStatus(
				StatusEnum.CashDepositAccountAmountEntered,
				new
				{
					TenNote = TenNote,
					TwentyNote = TwentyNote,
					FiftyNote = FiftyNote,
					OneHundredNote = HundredNote,
					TwoHundredNote = TwoHundredNote,
					FiveHundredNote = FiveHundredNote,
					OneThousandNote = OneThousandNote,
				});

		}

		public CashDepositToAccountWorkflowRT(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
		{
			_journal.TransactionStarted();
			_journal.TransactionName(nameof(CashDepositToAccountWorkflow));
			_callTellerViewModel = _container.Resolve<ICallTellerViewModel>();
			_callTellerViewModel.Communicator.RTMessageReceived += _communicator_RTMessageReceived;
			_callTellerViewModel.Communicator.CurrentSessionEnded += Communicator_CurrentSessionEnded;
			_callTellerViewModel.Communicator.CallEnded += Communicator_CallEnded;

			_workflowCompletionTask = new TaskCompletionSource<bool>();
			_receiptFormatter = receiptFormatter;

			_cashDepositAccountSelectionStep = container.Resolve<CashDepositAccountSelectionStep>();
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
				_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
				_workflowCompletionTask.TrySetResult(false);
				LoadStandByRT();
			};

			Context.Get<ICashDepositContext>().SelfService = false;

			_cashDepositAccountSelectionStep._accountSelectedAction = async () =>
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
						await _accountConfirmationStep.ConfirmAccountDetails();
					}
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					_logger.Exception(ex);
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
					await LoadErrorScreenAsync(ErrorType.InvalidAccountNumber, async () =>
					{
						if (Context.Get<ICashDepositContext>().ManualAccount)
						{
							await _manualAccountEntryStep.EnterAccountNumber();
						}
						else
						{
							await _cashDepositAccountSelectionStep.SelectAccountAsync();
						}
						return;
					}, false);
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
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidAccountNumber, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
					_workflowCompletionTask.TrySetResult(false);
				}
			};
			_manualAccountEntryStep.BackAction = async () =>
			{
				await _cashDepositAccountSelectionStep.SelectAccountAsync();
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
                catch (NoteErrorException ex)
                {
                    _logger.Info($"Error [NoteErrorException] Result: {ex.ErrorResultNumber}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    var _animationViewModel = _container.Resolve<IAnimationViewModel>();
                    _animationViewModel.Type(AnimationType.TakeCash);
                    _navigator.RequestNavigation(_animationViewModel);
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();

                    IgnoreExceptions(async () =>
                    {
                        if (_cashAcceptor.HasPendingCashInside)
                            await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                    });
                    await Task.Delay(4000);

                    await LoadErrorScreenAsync(ErrorType.CollectCash);
                    LoadWaitScreen();

                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account confirmation failed for note error");
                    _workflowCompletionTask.SetResult(false);

                }
                catch (DeviceMalfunctionException ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					_logger.Exception(ex);
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
					var _animationViewModel = _container.Resolve<IAnimationViewModel>();
					_animationViewModel.Type(AnimationType.TakeCash);
					_navigator.RequestNavigation(_animationViewModel);

					var _cashAcceptor = _container.Resolve<ICashAcceptor>();
					IgnoreExceptions(async () =>
					{
						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					});
					await Task.Delay(5000);
					await LoadErrorScreenAsync(ErrorType.CollectCash, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
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
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
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
					await _cashDepositAccountSelectionStep.SelectAccountAsync();
				}
			};

			_depositConfirmationStep._depositConfirmedAction = async () =>
			{
				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						var vmAnimation = _container.Resolve<IAnimationViewModel>();
						vmAnimation.Type(AnimationType.TakeCash);
						_navigator.RequestNavigation(vmAnimation);
						var _cashacceptor = _container.Resolve<ICashAcceptor>();

						IgnoreExceptions(async () =>
						{
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
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.CashDepositAccountConfirmed);

				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					_logger.Exception(ex);
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
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

					_callTellerViewModel.Communicator.SendStatus(StatusEnum.CashDepositAccountPaymentSuccess);
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					_logger.Exception(ex);
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
					{
						_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
						_workflowCompletionTask.TrySetResult(false);
						LoadStandByRT();
					});
					_workflowCompletionTask.TrySetResult(false);
				}
			};

		}

		private async void _communicator_RTMessageReceived(object sender, RTMessageEventArgs e)
		{
			var _context = Context.Get<ICashDepositContext>();
			try
			{
				if (e.MessageCode.Code == (int)StatusEnum.CashDepositAccountActivateCashDeposit)
				{
					_logger?.Info("Message for activate cash deposit arrived.");
					await _depositConfirmationStep.DepositCash();
				}

				else if (e.MessageCode.Code == (int)StatusEnum.CashDepositAccountConfirmCashDeposit)
				{
					var _cashAcceptor = _container.Resolve<ICashAcceptor>();

					var _transactionService = _container.Resolve<ITransactionService>();
					var _cashDepositToAccount = await _transactionService.CashDepositAsync(
						ConfigurationManager.AppSettings["VTMDebitAccount"].ToString(),
						"AED",
						_context.SelectedAccount?.Number,
						Context.Get<ICashDepositContext>().SelectedAccount?.Currency,
						Context.Get<ICashDepositContext>().TotalAmount.ToString(),
						$"{_context.SelectedAccount?.Number}");

					_context.AuthCode = _cashDepositToAccount.HostTransCode;

					await _cashAcceptor.StoreCashAsync();

					_cashDepositResultStep._receiptSelected();

					_logger?.Info("Message for receipt arrived.");
					var receiptData = await _receiptFormatter.FormatAsync(new CashDepositToAccountReceipt
					{
						AccountNumber = _context.SelectedAccount.Number,
						AuthCode = _context.AuthCode,
						CardNumber = _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber,
						Currency = _context.Currency,
						Denominations = _context.DepositedCash.Select(x => new Framework.Interface.Receipts.Denomination()
						{
							Count = x.Quantity,
							Value = x.Denomination
						})
						.ToList(),

						TransactionStatus = TransactionStatus.Succeeded,
						CustomerName = _context.SelectedAccount.Name
					}, new ReceiptFormattingOptions() { IsMarkupEnabled = true });

					_journal.TransactionSucceeded(_context.AuthCode);
					await _printReceiptStep.PrintReceipt(true, receiptData);
					SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, _context.TotalAmount.ToString(), _context.AuthCode, _context?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.CashDepositAccountReceiptPrinted);
					_logger?.Info("Message for receipt ended.");
					_workflowCompletionTask.TrySetResult(true);
				}

				else if (e.MessageCode.Code == (int)StatusEnum.CashDepositAccountCancelCashDeposit)
				{
					var _cashAcceptor = _container.Resolve<ICashAcceptor>();
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					var _anim = _container.Resolve<IAnimationViewModel>();
					_anim.Type(AnimationType.TakeCash);
					_navigator.RequestNavigation(_anim);
					await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					LoadWaitScreen();

					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				}

				else if (e.MessageCode.Code == (int)StatusEnum.EndCurrentSession)
				{
					_journal.TransactionSucceeded(_context.AuthCode ?? "");
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				}
			}
			catch (DeviceMalfunctionException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
				_logger.Exception(ex);
				var _animationViewModel = _container.Resolve<IAnimationViewModel>();
				_animationViewModel.Type(AnimationType.TakeCash);
				_navigator.RequestNavigation(_animationViewModel);

				var _cashAcceptor = _container.Resolve<ICashAcceptor>();
				IgnoreExceptions(async () =>
				{
					await _cashAcceptor.RollbackCashAndWaitTakenAsync();
				});
				await Task.Delay(5000);

				await LoadErrorScreenAsync(ErrorType.CollectCash, () =>
				{
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadWaitScreen();

					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(true);
			}
			catch (DeviceTimeoutException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit - Device timeout");
				_logger.Exception(ex);
				LoadWaitScreen();
				var _cashAcceptor = _container.Resolve<ICashAcceptor>();
				IgnoreExceptions(async () =>
				{
					await _cashAcceptor.RetractCashAsync();
				});
				_workflowCompletionTask.TrySetResult(false);
				LoadStandByRT();
			}
			catch (DeviceOperationCanceledException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit - Device failure");
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
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.InvalidTransaction, () =>
				{
					var _cashAcceptor = _container.Resolve<ICashAcceptor>();
					_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
					IgnoreExceptions(async () =>
					{
						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					});
					_workflowCompletionTask.TrySetResult(false);
					LoadStandByRT();
				});
				_workflowCompletionTask.TrySetResult(true);
			}
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Workflow: Cash Deposit To Account - RT Assisted");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

				LoadWaitScreen();
				await _checkReceiptPrinterStep.CheckPrinterAsync();
                await _checkCashAcceptorStep.CheckCashAcceptorAsync();
                var _context = Context.Get<ICashDepositContext>();
				_context.Accounts = await LoadAccounts();
				_cashDepositAccountSelectionStep.SelectAccountAsync();
			}
			catch (InvalidOperationException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get accounts");
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
			catch (Exception ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTCashDepositAccount, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get accounts");
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

		public override void Dispose()
		{
			_logger?.Info("Called dispose");
			_callTellerViewModel.Communicator.RTMessageReceived -= _communicator_RTMessageReceived;
			DisposeSteps();
		}

		private async Task<List<Services.Interface.Entities.Account>> LoadAccounts()
		{
            _logger?.Info($"Execute Task: Load Accounts");

            var _authenticationService = _container.Resolve<IAuthenticationService>();
			return await _authenticationService.GetAccounts(customerIdentifier: _container.Resolve<ISessionContext>().CustomerIdentifier, conditionId: AccountCriterion.Casa);
		}

		private void Communicator_CallEnded(object sender, Communication.Interface.CallEventArgs e)
		{
			IgnoreExceptions(() =>
			{
				var _CashAcceptor = _container.Resolve<ICashAcceptor>();
				_CashAcceptor.RollbackCashAndWaitTakenAsync();
			});
			_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
			_workflowCompletionTask.TrySetResult(false);
			LoadStandByRT();
		}

		private void Communicator_CurrentSessionEnded(object sender, EventArgs e)
		{
			_callTellerViewModel.Communicator.SendStatus(StatusEnum.EndCurrentSession);
			_workflowCompletionTask.TrySetResult(false);
			LoadStandByRT();
		}
	}
}