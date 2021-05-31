namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.Exceptions;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Services.ISO.UserManagement;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ChangePasswordStep : WorkflowStep
    {
        private TaskCompletionSource<bool> _workflowCompletionTask;
        public ChangePasswordStep(IResolver container) : base(container)
        {
            //_workflowCompletionTask = new TaskCompletionSource<bool>();
        }
        public async Task<bool> ExecuteAsync()
        {
            _logger?.Info($"Execute Step: Change Password Step");
            _journal.TransactionStarted(EJTransactionType.NonFinancial, "Update password");
            _workflowCompletionTask = new TaskCompletionSource<bool>();
            SetCurrentStep($"{nameof(ChangePasswordStep)}");

            var cancellationToken = new CancellationTokenSource();

            var _userInfoService = _container.Resolve<IUserService>();



            _navigator.RequestNavigationTo<IChangePasswordViewModel>((vm) =>
            {
                vm.DefaultVisibility = vm.CancelVisibility = true;
                vm.DefaultAction = async () =>
                {
                    try
                    {

                        MonitorDeviceStatus();
                        //var _userInfo1 = await _userInfoService.UpdatePasswordAsync("testuser", viewModel.oldPassword, viewModel.newPassword); // Hardcoded values
                        var _userInfo1 = await _userInfoService.UpdatePasswordAsync(Context.Get<IAuthDataContext>().loggedInUserInfo.Username, vm.oldPassword, vm.newPassword);
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        
                        //CancelAction?.Invoke();
                        _journal.TransactionSucceeded("Password Updated");
                        _workflowCompletionTask.TrySetResult(true);
                    //return;
                    }
                    catch (UpdatePasswordException ex)
                    {
                        _journal.TransactionFailed(ex.GetBaseException().Message);
                        _logger.Exception(ex);
                        cancellationToken?.Cancel();
                        cancellationToken = null;
                        //await LoadErrorScreenAsync(ErrorType.InvalidChangePassword);//
                        await LoadErrorScreenAsync(ErrorType.InvalidChangePassword, async () =>
                        {
                            cancellationToken?.Cancel();
                            cancellationToken = null;

                            CancelAction?.Invoke();
                            _workflowCompletionTask.TrySetResult(false);
                        }, false);
                    }
                    catch (System.Exception ex)
                    {
                        _journal.TransactionFailed(ex.GetBaseException().Message);
                        _logger.Exception(ex);
                        cancellationToken?.Cancel();
                        cancellationToken = null;
                        //await LoadErrorScreenAsync(ErrorType.FailUpdatePassword);//ErrorFailUpdatePassword
                        await LoadErrorScreenAsync(ErrorType.FailUpdatePassword, async () =>
                        {
                            cancellationToken?.Cancel();
                            cancellationToken = null;

                            CancelAction?.Invoke();
                            _workflowCompletionTask.TrySetResult(false);
                        }, false);
                    }

                };

                vm.CancelAction = () =>
                {
                    cancellationToken?.Cancel();
                    cancellationToken = null;

                    CancelAction?.Invoke();
                    _workflowCompletionTask.TrySetResult(false);
                    return;

                };

                vm.StartUserActivityTimer(cancellationToken.Token);
                vm.ExpiredAction = () =>
                {
                    _navigator.Push<IActiveConfirmationViewModel>((viewmodel) =>
                    {
                        viewmodel.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                        viewmodel.YesAction = () =>
                        {
                            vm.StartUserActivityTimer(cancellationToken.Token);
                            _navigator.Pop();
                        };
                        viewmodel.NoAction = viewmodel.ExpiredAction = () =>
                        {
                            cancellationToken?.Cancel();
                            cancellationToken = null;

                            CancelAction();
                            _workflowCompletionTask.SetResult(false);
                        };
                    });
                };
            });
                return await _workflowCompletionTask.Task;
            }

        public override void Dispose()
        {

        }

    }
}
