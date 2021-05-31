using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.ServicesNdc.Interface;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
using Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard.Steps;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.Common.Steps;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard
{
	class CashDepositToCreditCardWorkflow : Workflow
	{
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly CreditCardDetailsStep _cashDepositAccountSelectionStep;
		private readonly ManualAccountEntryStep _manualAccountEntryStep;
		private readonly AccountConfirmationStep _accountConfirmationStep;
		private readonly DepositConfirmationStep _depositConfirmationStep;
		private readonly CashDepositResultStep _cashDepositResultStep;
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
        private readonly CheckCashAcceptorStep _checkCashAcceptorStep;
        private readonly TaskCompletionSource<bool> _workflowCompletionTask;

		private void CheckForCancel()
		{
			if (Context.Get<ICashDepositContext>().IsCanceled)
			{
				_journal.TransactionCanceled();
				LoadSelfServiceMenu();
			}
		}

		public CashDepositToCreditCardWorkflow(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.Financial, "Cash Deposit Credit Card-SS");

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

            _checkReceiptPrinterStep.CancelAction = _cashDepositAccountSelectionStep.CancelAction = _manualAccountEntryStep.CancelAction = _accountConfirmationStep.CancelAction = _depositConfirmationStep.CancelAction = _cashDepositResultStep.CancelAction = _printReceiptStep.CancelAction = () =>
            {
                IgnoreExceptions(async () =>
                {
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                    await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                });
                _workflowCompletionTask.TrySetResult(true);
                return;
            };

            AddSteps($"{Properties.Resources.StepCreditCardDetails},{Properties.Resources.StepConfirmation},{Properties.Resources.StepDenomination}, {Properties.Resources.StepReceiptPrinting}");

			Context = _cashDepositAccountSelectionStep.Context = _manualAccountEntryStep.Context = _accountConfirmationStep.Context = _depositConfirmationStep.Context = _cashDepositResultStep.Context = CreateContext(typeof(CashDepositContext));
			_cashDepositAccountSelectionStep._cardDetailsConfirmedAction = async () =>
			{
				try
				{
					CheckForCancel();
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						return;
					}

					if (Context.Get<ICashDepositContext>().ManualAccount)
					{
						await _manualAccountEntryStep.EnterAccountNumber();
					}
					else
					{
						_accountConfirmationStep._accountDetailsConfirmedAction();
					}
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get card details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
					_workflowCompletionTask.SetResult(false);
					return;
				}
			};

			_manualAccountEntryStep._accountSelectedAction = async () =>
			{
				try
				{
					CheckForCancel();
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						return;
					}

					_accountConfirmationStep._accountDetailsConfirmedAction();
					//await _accountConfirmationStep.ConfirmAccountDetails();
				}
				catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
					_workflowCompletionTask.SetResult(false);
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
					CheckForCancel();
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						return;
					}

					await _depositConfirmationStep.DepositCash();
				}
				catch (DeviceMalfunctionException ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.CollectCash);
					var _cashAcceptor = _container.Resolve<ICashAcceptor>();
					IgnoreExceptions(async () =>
					{
						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					});
					LoadWaitScreen();
					//await _cashAcceptor.ResetAsync();
					_workflowCompletionTask.SetResult(false);
				}
                catch (DeviceTimeoutException ex)
                {
                    _logger.Info($"Error [DeviceTimeoutException] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    LoadWaitScreen();
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                    IgnoreExceptions(async () =>
                    {
                        await _cashAcceptor.RetractCashAsync();
                    });
                    SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account confirmation failed");
                    _workflowCompletionTask.SetResult(false);
                }
                catch (Exception ex)
				{
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get account details");
					_logger.Exception(ex);
					await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
					_workflowCompletionTask.SetResult(false);
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
				var _context = Context.Get<ICashDepositContext>();
				var _cashAcceptor = _container.Resolve<ICashAcceptor>();

				try
				{
					if (Context.Get<ICashDepositContext>().IsCanceled)
					{
						var vmAnimation = _container.Resolve<IAnimationViewModel>();
						vmAnimation.Type(AnimationType.TakeCash);
						_navigator.RequestNavigation(vmAnimation);

						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
						CheckForCancel();
						return;
					}

					var _ndcService = _container.Resolve<INdcService>();
					var _sessionContext = _container.Resolve<ISessionContext>();
					var res = await _ndcService.CashDepositCCAsync(_sessionContext.CardUsed.CardNumber, _context.DepositedCash, _context.TotalAmount.ToString());

					if (!string.IsNullOrEmpty(res?.AuthCode))
					{
						_context.AuthCode = res.AuthCode;

						await _cashAcceptor.StoreCashAsync();
						_cashDepositResultStep._receiptSelected();
					}
					else
					{
						throw new Exception("Reference No not found.");
					}
				}
                catch (DeviceTimeoutException ex)
                {
                    _logger.Info($"Error [DeviceTimeoutException] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    LoadWaitScreen();
                    IgnoreExceptions(async () =>
                    {
                        await _cashAcceptor.RetractCashAsync();
                    });
                    SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
                    _workflowCompletionTask.SetResult(false);
                }
                catch (Exception ex)
				{
					_logger.Exception(ex);
					_journal.TransactionFailed(ex.GetBaseException().Message);

					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
					
					await LoadErrorScreenAsync(ErrorType.DeclinedCashDeposit);

					IgnoreExceptions(async () =>
					{
						var vmAnimation = _container.Resolve<IAnimationViewModel>();
						vmAnimation.Type(AnimationType.TakeCash);
						_navigator.RequestNavigation(vmAnimation);

						await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					});

					_workflowCompletionTask.TrySetResult(false);
				}
			};

			_cashDepositResultStep._receiptSelected = async () =>
			{
				try
				{
					var _context = Context.Get<ICashDepositContext>();
					LoadWaitScreen();

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

					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, _context.TotalAmount.ToString(), _context.AuthCode, _context?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);

					_journal.TransactionSucceeded(_context.AuthCode);
					await _printReceiptStep.PrintReceipt(true, receiptData);
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);
					_journal.TransactionFailed(ex.GetBaseException().Message);

					SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
					
					await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt);
					_workflowCompletionTask.SetResult(false);
				}

				_workflowCompletionTask.TrySetResult(true);
			};

		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Workflow: Cash Deposit To Credit Card - Self Service");

            try
			{
				LoadWaitScreen();
				await _checkReceiptPrinterStep.CheckPrinterAsync();
                await _checkCashAcceptorStep.CheckCashAcceptorAsync();
                await GetCreditCardDetailsAsync();
				_cashDepositAccountSelectionStep.ConfirmCardDetails();
			}
			catch (InvalidOperationException ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get card details");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
				_workflowCompletionTask.SetResult(false);
				return false;
			}
			catch (Exception ex)
			{
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.SSCashDepositCreditCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get card details");
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
				_workflowCompletionTask.SetResult(false);
				return false;
			}

			return await _workflowCompletionTask.Task;
		}

		public override void Dispose()
		{

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
	}
}