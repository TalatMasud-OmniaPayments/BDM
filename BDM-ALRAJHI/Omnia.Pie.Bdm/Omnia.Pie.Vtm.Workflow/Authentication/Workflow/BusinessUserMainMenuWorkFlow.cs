
namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Devices.Interface;
    using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
    using Omnia.Pie.Vtm.Framework.Configurations;
    using Omnia.Pie.Vtm.Framework.Exceptions;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Framework.Interface.Receipts;
    using Omnia.Pie.Vtm.Framework.Interface.Reports;
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.Workflow;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Vtm.Workflow.Common.Steps;
    using Omnia.Pie.Vtm.Workflow.StatementPrinting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using StatementItem = Framework.Interface.Reports.StatementItem;

    internal class BusinessUserMainMenuWorkFlow : Workflow
    {
        private readonly UpdatePasswordSuccessStep _updatePasswordSuccessStep;
        private readonly ChangePasswordStep _changePasswordStep;
        private readonly PrintReceiptStep _printReceiptStep;
        private readonly IReceiptFormatter _receiptFormatter;
        private readonly IReportsManager _reportsManager;
        private readonly TaskCompletionSource<bool> _workflowCompletionTask;
        //private readonly IAuthDataContext _context;
        //private readonly BusinessUserMainMenuStep _businessUserMainMenuStep;
        public BusinessUserMainMenuWorkFlow(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
        {

           // _businessUserMainMenuStep = _container.Resolve<BusinessUserMainMenuStep>();
            _changePasswordStep = _container.Resolve<ChangePasswordStep>();
            _updatePasswordSuccessStep = _container.Resolve<UpdatePasswordSuccessStep>();
            _printReceiptStep = _container.Resolve<PrintReceiptStep>();
            _receiptFormatter = receiptFormatter;
            _reportsManager = container.Resolve<IReportsManager>();
            _workflowCompletionTask = new TaskCompletionSource<bool>();
            //_context = Context.Get<IAuthDataContext>();


            //_updateInfoStep.Context= _updateInfoSuccessStep.Context = _printReceiptStep.Context= _changePasswordStep.Context = Context;


            //Context = _reprintStep.Context = _updatePasswordSuccessStep.Context = _changePasswordStep.Context = CreateContext(typeof(AuthDataContext));

            //Context.Get<IAuthDataContext>().SelfServiceMode = true;

            /*_updateInfoStep.CancelAction = _updateInfoStep.CancelAction = () =>
            {
                Execute(_updateInfoStep.Context);
            };*/
            _changePasswordStep.CancelAction = _updatePasswordSuccessStep.CancelAction =() =>
            {
                Execute();

            };
        }
        public void Execute()
        {
            _logger?.Info($"Execute Step: Business User Main Menu Step");
            Context = _updatePasswordSuccessStep.Context = _printReceiptStep.Context = _changePasswordStep.Context  = CreateContext(typeof(AuthDataContext));
            //_container.Resolve<IAuthDataContext>();
            _logger?.Info($"Is Online Trans: " + _container.Resolve<IAuthDataContext>().isOnlineTran);
            var cancellationToken = new CancellationTokenSource();

            _navigator.RequestNavigationTo<IBusinessUserMainMenuViewModel>((viewModel) =>
            {
                viewModel.CancelVisibility = true;
                

                viewModel.CancelAction = () =>
                {
                    cancellationToken?.Cancel();
                    cancellationToken = null;
                    LoadMainScreen();
                    //CancelAction?.Invoke();
                    _workflowCompletionTask.TrySetResult(true);
                };
                viewModel.DepositAction = async () =>
                {
                    try
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;
                        MonitorDeviceStatus();
                        if (_container.Resolve<IAuthDataContext>().isOnlineTran)
                        {

                            using (var flow = _container.Resolve<CashDeposit.Account.CashDepositToAccountWorkflow>())
                            {
                                //flow.Username = (string)username.Clone();
                                flow.userInfo = GetUserInfo();
                                //flow.UserContext = Context;
                                if (await flow.ExecuteAsync())
                                {
                                    FinishTransaciton();

                                }

                            }
                        }
                        else
                        {
                            using (var flow = _container.Resolve<CashDeposit.Offline.CashDepositOfflineWorkflow>())
                            {
                                //flow.UserContext = Context;
                                flow.userInfo = GetUserInfo();
                                if (await flow.ExecuteAsync())
                                {
                                    FinishTransaciton();
                                    //LoadSelfServiceMenu();
                                }
                            }
                        }

                        _workflowCompletionTask.TrySetResult(true);

                    }
                    catch (DeviceMalfunctionException ex)
                    {
                        _journal.TransactionFailed(ex.GetBaseException().Message);
                        _logger.Exception(ex);
                        // await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                        await LoadErrorScreenAsync(ErrorType.InternetInterrupted, async () =>
                        {
                            //Execute(Context);
                            LoadMainScreen();
                        }, false);
                        //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
                        _workflowCompletionTask.SetResult(false);
                        return;
                    }
                    catch (Exception ex)
                    {
                        _journal.TransactionFailed(ex.GetBaseException().Message);
                        _logger.Exception(ex);
                        // await LoadErrorScreenAsync(ErrorType.InvalidTransaction);
                        await LoadErrorScreenAsync(ErrorType.InternetInterrupted, async () =>
                        {
                            //Execute(Context);
                            LoadMainScreen();
                        }, false);
                        //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account selection failed");
                        _workflowCompletionTask.SetResult(false);
                        return;
                    }

                    
                };
                
                viewModel.ReprintAction = async () =>
                {
                    
                    cancellationToken?.Cancel();
                    cancellationToken = null;
                    if (_container.Resolve<IAuthDataContext>().isOnlineTran)
                    {
                        LoadWaitScreen();
                        MonitorDeviceStatus();

                        var flow = _container.Resolve<StatementPrintingWorkFlow>();
                        //flow.UserContext = Context;
                        flow.Execute();
                        _workflowCompletionTask.TrySetResult(true);
                    }
                    else
                    {
                        await LoadErrorScreenAsync(ErrorType.NoInternet, async () =>
                        {
                            Execute();
                        }, false);
                        _workflowCompletionTask.TrySetResult(false);
                    }

                };
                viewModel.ChangePasswordAction = async () =>
                {
                    
                    if (_container.Resolve<IAuthDataContext>().isOnlineTran)
                    {
                        MonitorDeviceStatus();
                        _workflowCompletionTask.TrySetResult(true);
                        if (await _changePasswordStep.ExecuteAsync())
                        {
                            if (await _updatePasswordSuccessStep.ExecuteAsync())
                            {
                                LoadWaitScreen();

                                var receiptData = await _receiptFormatter.FormatAsync(new UpdateInfoReceipt
                                {
                                    userName = _container.Resolve<IAuthDataContext>().loggedInUserInfo.Username,
                                    name = _container.Resolve<IAuthDataContext>().loggedInUserInfo.Name,
                                    mobile = _container.Resolve<IAuthDataContext>().loggedInUserInfo.Mobile,
                                    email = _container.Resolve<IAuthDataContext>().loggedInUserInfo.Email,
                                });

                                _journal.PrintingReceipt(receiptData);
                                await _printReceiptStep.PrintReceipt(true, receiptData);

                                cancellationToken?.Cancel();
                                cancellationToken = null;

                                Execute();
                            }
                            else
                            {
                                _logger?.Info($"Transaction finished.");

                                cancellationToken?.Cancel();
                                cancellationToken = null;

                                Execute();
                            }

                        }
                        else
                        {
                            _logger?.Info($"Transaction failed.");
                        }
                    }
                    else {
                        await LoadErrorScreenAsync(ErrorType.NoInternet, async () =>
                        {
                            Execute();
                        }, false);
                        _workflowCompletionTask.TrySetResult(false);
                    }
                    //FinishTransaciton();
                    //var t = "x";
                    
                };

                LoadTimeoutScreen(viewModel, cancellationToken, _workflowCompletionTask);
                /*if (Context.Get<IAuthDataContext>().SelfServiceMode)
                {
                    viewModel.StartUserActivityTimer(cancellationToken.Token);
                    viewModel.ExpiredAction = () =>
                    {
                        _navigator.Push<IActiveConfirmationViewModel>((vm) =>
                        {
                            vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                            vm.YesAction = () =>
                            {
                                viewModel.StartUserActivityTimer(cancellationToken.Token);
                                _navigator.Pop();
                            };
                            vm.NoAction = vm.ExpiredAction = () =>
                            {
                                cancellationToken?.Cancel();
                                cancellationToken = null;

                                LoadMainScreen();
                                //CancelAction?.Invoke();
                                _workflowCompletionTask.TrySetResult(true);
                            };
                        });
                    };
                }*/
            });
        }

        private UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Name = Context.Get<IAuthDataContext>().loggedInUserInfo.Name,
                Username = Context.Get<IAuthDataContext>().loggedInUserInfo.Username,
                Email = Context.Get<IAuthDataContext>().loggedInUserInfo.Email,
                Mobile = Context.Get<IAuthDataContext>().loggedInUserInfo.Mobile,
                UserType = Context.Get<IAuthDataContext>().loggedInUserInfo.UserType,
                ResponseCode = Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode,
            };
        }
        private List<Services.Interface.Entities.StatementItem> LoadTempItems()
        {
            var lst = new List<Services.Interface.Entities.StatementItem>();
            DateTime today = DateTime.Today;

            for (int i = 0; i< 10; i++)
            {
                lst.Add(new Services.Interface.Entities.StatementItem()
                {
                    CreditAmount = i,
                    DebitAmount = 10.0,
                    Description = "item?.Description",
                    TransactionDate = today,
                    RunningBalance = 1050.0,
                    ValueDate = today,
                });
            }

            return lst;
        }
        private List<StatementItem> GetItems(List<Services.Interface.Entities.StatementItem> statementItems)
        {
            var lst = new List<StatementItem>();

            if (statementItems != null)
            {
                foreach (var item in statementItems)
                {
                    lst.Add(new StatementItem()
                    {
                        CreditAmount = item?.CreditAmount,
                        DebitAmount = item?.DebitAmount,
                        Description = item?.Description,
                        PostingDate = item?.TransactionDate,
                        //RunningBalance = item?.RunningBalance,
                        ValueDate = item?.ValueDate,
                    });
                }
            }

            return lst;
        }
        private async Task PrintStatementAsync(StatementReportData statementReportData)
        {
            _logger?.Info($"Execute Task: Create and Print Statement");

            using (var report = _reportsManager.CreateReport(statementReportData))
            {
                report.Print();
            }

            await Task.Delay(7000);
        }

        public override void Dispose()
        {

        }
    }
}
