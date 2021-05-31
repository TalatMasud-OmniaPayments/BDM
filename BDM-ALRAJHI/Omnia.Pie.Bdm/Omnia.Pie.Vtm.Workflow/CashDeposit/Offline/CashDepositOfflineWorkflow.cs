using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Workflow.Common.Steps;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Services.Interface;
using System.Configuration;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using Omnia.Pie.Vtm.Framework.Configurations;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Workflow.Authentication;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Offline
{
    public class CashDepositOfflineWorkflow : Workflow
    {
        private readonly IReceiptFormatter _receiptFormatter;
        private readonly CashDepositAccountSelectionStep _cashDepositAccountSelectionStep;
        private readonly ManualAccountEntryStep _manualAccountEntryStep;
        private readonly AccountConfirmationStep _accountConfirmationStep;
        private readonly DepositConfirmationStep _depositConfirmationStep;
        private readonly CashDepositResultStep _cashDepositResultStep;
        private readonly PrintReceiptStep _printReceiptStep;
        private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
        private readonly CheckCashAcceptorStep _checkCashAcceptorStep;

        private readonly TaskCompletionSource<bool> _workflowCompletionTask;
        private readonly ITransactionStore _transactionStore = ServiceLocator.Instance.Resolve<ITransactionStore>();
        //public IDataContext businessContext { get; set; }
        public UserInfo userInfo;
        private void CheckForCancel()
        {
            if (Context.Get<ICashDepositContext>().IsCanceled)
            {
                _journal.TransactionCanceled();
                //_workflowCompletionTask.TrySetResult(false);
                LoadSelfServiceMenu();
            }
        }


        public CashDepositOfflineWorkflow(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
        {
            _journal.TransactionStarted(EJTransactionType.Financial, "Cash Deposit to Account - Offline");
            _journal.TransactionName(nameof(CashDepositOfflineWorkflow));
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


            _checkReceiptPrinterStep.CancelAction = _cashDepositAccountSelectionStep.CancelAction = _manualAccountEntryStep.CancelAction = _accountConfirmationStep.CancelAction = _depositConfirmationStep.CancelAction = _cashDepositResultStep.CancelAction = _printReceiptStep.CancelAction = () =>
            {
                IgnoreExceptions(async () =>
                {
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                    if (_cashAcceptor.IsCashInRunning)
                    {
                        await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                    }
                    _journal.TransactionCanceled();
                    _workflowCompletionTask.TrySetResult(false);
                    LoadMainScreen();
                });
                //_workflowCompletionTask.TrySetResult(true);
                return;
            };

            AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepConfirmation},{Properties.Resources.StepDenomination}, {Properties.Resources.StepReceiptPrinting}");

            Context = _cashDepositAccountSelectionStep.Context = _manualAccountEntryStep.Context = _accountConfirmationStep.Context = _depositConfirmationStep.Context = _cashDepositResultStep.Context = CreateContext(typeof(CashDepositContext));

            Context.Get<ICashDepositContext>().SelfService = true;

            _cashDepositAccountSelectionStep._accountSelectedAction = async () =>
            {
                try
                {
                    CheckForCancel();
                    if (Context.Get<ICashDepositContext>().IsCanceled)
                    {
                        //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
                        //_workflowCompletionTask.SetResult(false);
                        return;
                    }
                    //CheckForCancel();
                    if (Context.Get<ICashDepositContext>().ManualAccount)
                    {
                        await _manualAccountEntryStep.EnterAccountNumber();
                    }
                    else
                    {
                        await _accountConfirmationStep.ConfirmAccountDetails();
                    }
                }
                catch (Exception ex)
                {
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    _logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
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

                    await _accountConfirmationStep.ConfirmAccountDetails();
                }
                catch (Exception ex)
                {
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    _logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
                    _workflowCompletionTask.SetResult(false);
                    return;
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

                    CheckForCancel();
                    if (Context.Get<ICashDepositContext>().IsCanceled)
                    {
                        //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
                        //_workflowCompletionTask.SetResult(false);
                        return;
                    }
                    
                    await _depositConfirmationStep.DepositCash();
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

                    Context.Get<ICashDepositContext>().IsCanceled = true;
                    CheckForCancel();
                    //_workflowCompletionTask.SetResult(false);

                }
                catch (DeviceMalfunctionException ex)
                {



                    _logger.Info($"Error [DeviceMalfunctionException] Result: {ex.ErrorResultNumber}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    var _animationViewModel = _container.Resolve<IAnimationViewModel>();
                    _animationViewModel.Type(AnimationType.TakeCash);
                    _navigator.RequestNavigation(_animationViewModel);
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();

                    IgnoreExceptions(async () =>
                    {
                        if (_cashAcceptor.HasPendingCashInside)
                        {


                            await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                        }

                    });
                    await Task.Delay(4000);

                    await LoadErrorScreenAsync(ErrorType.CollectCash);
                    LoadWaitScreen();
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account confirmation failed for note error");

                    Context.Get<ICashDepositContext>().IsCanceled = true;
                    CheckForCancel();
                }
                catch (DeviceTimeoutException ex)
                {
                    _logger.Info($"Error [DeviceTimeoutException] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    //LoadWaitScreen();
                    var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                    IgnoreExceptions(async () =>
                    {
                        //await _cashAcceptor.RetractCashAsync();

                        await LoadErrorScreenAsync(ErrorType.DeclinedCashDeposit, async () =>
                        {
                            var vmAnimation = _container.Resolve<IAnimationViewModel>();
                            vmAnimation.Type(AnimationType.RetractingCash);
                            _navigator.RequestNavigation(vmAnimation);

                            await _cashAcceptor.RetractCashAsync();
                            LoadSelfServiceMenu();
                        }, false);
                    });
                }
                catch (Exception ex)
                {
                    _logger.Info($"Error [Exception] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account confirmation failed");
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
                    await _cashDepositAccountSelectionStep.SelectAccountAsync();
                }
            };

            _depositConfirmationStep._depositConfirmedAction = async () =>
            {
                var _context = Context.Get<ICashDepositContext>();
                var _cashAcceptor = _container.Resolve<ICashAcceptor>();
                try
                {
                    //LoadWaitScreen();
                    if (Context.Get<ICashDepositContext>().IsCanceled)
                    {
                        var vmAnimation = _container.Resolve<IAnimationViewModel>();
                        vmAnimation.Type(AnimationType.TakeCash);
                        _navigator.RequestNavigation(vmAnimation);

                        await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                        CheckForCancel();
                        _workflowCompletionTask.TrySetResult(false);
                        return;
                    }

                    //LoadWaitScreen();

                    /*var _transactionService = _container.Resolve<ITransactionService>();
                    var _cashDepositToAccount = await _transactionService.CashDepositAsync(
                        ConfigurationManager.AppSettings["VTMDebitAccount"].ToString(),
                        "SAR",
                        _context.SelectedAccount.Number,
                        Context.Get<ICashDepositContext>().SelectedAccount.Currency,
                        Context.Get<ICashDepositContext>().TotalAmount.ToString(),
                        $"{_context.SelectedAccount.Number}");

                    
                    
                    Context.Get<ICashDepositContext>().AuthCode = _cashDepositToAccount.HostTransCode;*/
                    string timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");

                    var depositedDenominations = _context.DepositedCash.Select(x => new DepositedDenominations()
                    {
                        Count = x.Quantity,
                        Type = x.Denomination,
                    })
                        .ToList();
                    var cashDeposited = new CashDeposited
                    {

                        DebitAccount = ConfigurationManager.AppSettings["VTMDebitAccount"].ToString(),
                        DebitAccountCurrency = TerminalConfiguration.Section.Currency,
                        CreditAccount = _context.SelectedAccount.Number,
                        CreditAccountCurrency = TerminalConfiguration.Section.Currency,
                        CreditAmount = Context.Get<ICashDepositContext>().TotalAmount.ToString(),
                        BagSerialNo = ConfigurationManager.AppSettings["BagSerialNo"].ToString(),
                        SplitLength = ConfigurationManager.AppSettings["SplitLength"].ToString(),
                        UpdateBalanceFlag = ConfigurationManager.AppSettings["UpdateBalanceFlag"].ToString(),
                        UpdateBalanceRequester = ConfigurationManager.AppSettings["UpdateBalanceRequester"].ToString(),
                        MachineType = ConfigurationManager.AppSettings["MachineType"].ToString(),
                        SystemFlag = ConfigurationManager.AppSettings["SystemFlag"].ToString(),
                        TransactionDateTime = timestamp


                        

                    };
                    var transaction = (new DeviceTransaction
                    {
                        Username = userInfo.Username,
                        MessageTimestamp = timestamp,
                        isOffline = 1,
                        isUploaded = 0,
                        OnlineAuthCode = "-1",
                    });
                    bool isSaved = await _transactionStore.SaveTranasactionLocally(transaction, cashDeposited, depositedDenominations);

                    await _cashAcceptor.StoreCashAsync();

                    Context.Get<ICashDepositContext>().AuthCode = TerminalConfiguration.Section?.Id + "-" + transaction.id;
                    _cashDepositResultStep._receiptSelected();
                    //await _cashDepositResultStep.SelectReceipt();
                }
                catch (DeviceTimeoutException ex)
                {
                    _logger.Info($"Error [DeviceTimeoutException] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    LoadWaitScreen();
                    IgnoreExceptions(async () =>
                    {
                        await _cashAcceptor.RetractCashAsync();
                    });
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
                    _workflowCompletionTask.SetResult(false);
                }
                catch (Exception ex)
                {
                    _logger.Info($"Error [Exception] Result: {ex.Message}");
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    //_logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.DeclinedCashDeposit);
                    IgnoreExceptions(async () =>
                    {
                        var vmAnimation = _container.Resolve<IAnimationViewModel>();
                        vmAnimation.Type(AnimationType.TakeCash);
                        _navigator.RequestNavigation(vmAnimation);

                        await _cashAcceptor.RollbackCashAndWaitTakenAsync();
                    });
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the deposit");
                    _workflowCompletionTask.TrySetResult(false);
                }
            };

            _cashDepositResultStep._receiptSelected = async () =>
            {
                try
                {
                    var _context = Context.Get<ICashDepositContext>();
                    //LoadWaitScreen();

                    var receiptData = await _receiptFormatter.FormatAsync(new CashDepositToAccountReceipt
                    {
                        AccountNumber = _context.SelectedAccount.Number,
                        AuthCode = _context.AuthCode, // Comes from service call.
                        UserId = _container.Resolve<IAuthDataContext>()?.Username,
                        TransactionDate = DateTime.Now.ToString(),
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
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, _context.TotalAmount.ToString(), _context.AuthCode, _context?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
                    _journal.TransactionSucceeded(_context.AuthCode);
                    await _printReceiptStep.PrintReceipt(true, receiptData);
                }
                catch (Exception ex)
                {
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    _logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt);
                    SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
                    _workflowCompletionTask.SetResult(false);
                }

                _workflowCompletionTask.TrySetResult(true);
                //FinishTransaciton();
            };

        }

        public async Task<bool> ExecuteAsync()
        {
            _logger?.Info($"Execute Workflow: Cash Deposit To Account - Self Service");
            _journal.TransactionStarted(EJTransactionType.Financial, "Cash Deposit To Account - Offline - ");
            try
            {
                    LoadWaitScreen();
                    await _checkReceiptPrinterStep.CheckPrinterAsync();
                    await _checkCashAcceptorStep.CheckCashAcceptorAsync();
                    var _context = Context.Get<ICashDepositContext>();
                    _context.Accounts = await LoadAccountsOffline();
                    _cashDepositAccountSelectionStep.SelectAccountAsync();
               
            }
            catch (InvalidOperationException ex)
            {
                _journal.TransactionFailed(ex.GetBaseException().Message);
                _logger.Exception(ex);
                await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not get accounts");
                _workflowCompletionTask.SetResult(false);
                return false;
            }

            return await _workflowCompletionTask.Task;
        }
        public override void Dispose()
        {

        }

        private async Task<List<Services.Interface.Entities.Account>> LoadAccountsOffline(){

            TaskCompletionSource<List<Services.Interface.Entities.Account>> _task = new TaskCompletionSource<List<Services.Interface.Entities.Account>>();


            var account = (new Services.Interface.Entities.Account{ 
                Name = TerminalConfiguration.Section?.Id,          // terminal id
                Type = "Offline Deposit",
                Branch = TerminalConfiguration.Section?.BranchId,
                Number = TerminalConfiguration.Section?.MerchantId,        // terminal id
                Currency = TerminalConfiguration.Section?.Currency,
                CurrencyCode = TerminalConfiguration.Section?.CurrencyCode,

            });

            var accounts = new List<Services.Interface.Entities.Account>();
            accounts.Add(account);

            _task.SetResult(accounts);
            return await _task.Task;
        }
    }
}
