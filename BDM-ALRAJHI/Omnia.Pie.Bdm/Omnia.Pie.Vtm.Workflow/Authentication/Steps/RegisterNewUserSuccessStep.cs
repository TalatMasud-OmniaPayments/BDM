using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    internal class RegisterNewUserSuccessStep : WorkflowStep
    {
        private readonly TaskCompletionSource<bool> _workflowCompletionTask;
        public RegisterNewUserSuccessStep(IResolver container) : base(container)
        {
            _workflowCompletionTask = new TaskCompletionSource<bool>();

        }

        public async Task<bool> ExecuteAsync()
        {
            _logger?.Info($"Execute Step: Register New User Success Step");

            SetCurrentStep($"{nameof(RegisterNewUserSuccessStep)}");

            var cancellationToken = new CancellationTokenSource();

            _navigator.RequestNavigationTo<IRegisterNewUserSuccessViewModel>((vm) =>
            {

                vm.CancelVisibility = vm.DefaultVisibility = true;
                vm.DefaultAction = async () =>
                {
                    try
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        _workflowCompletionTask.TrySetResult(true);
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        _workflowCompletionTask.TrySetResult(false);
                        return;
                    }
                    //CancelAction?.Invoke();
                };

                vm.CancelAction = () =>
                {
                    cancellationToken?.Cancel();
                    cancellationToken = null;

                    //CancelAction?.Invoke();
                    LoadMainScreen();

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
