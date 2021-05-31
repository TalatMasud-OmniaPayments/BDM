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
    internal class UpdatePasswordSuccessStep : WorkflowStep
    {
        private TaskCompletionSource<bool> _workflowCompletionTask;
        public UpdatePasswordSuccessStep(IResolver container) : base(container)
        {
            

        }

        public async Task<bool> ExecuteAsync()
        {
            _logger?.Info($"Execute Step: Update Info Step");
            _workflowCompletionTask = new TaskCompletionSource<bool>();
            SetCurrentStep($"{nameof(UpdatePasswordSuccessStep)}");

            var cancellationToken = new CancellationTokenSource();

            /*var UserAccount1 = new UpdateAccount
            {
                UserAccount = "testuser",
            };*/


            _navigator.RequestNavigationTo<IUpdatePasswordSuccessViewModel>((viewModel) =>
            {

                viewModel.CancelVisibility = viewModel.DefaultVisibility = true;
                viewModel.DefaultAction = async () =>
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

                viewModel.CancelAction = () =>
                {
                    cancellationToken?.Cancel();
                    cancellationToken = null;

                    _workflowCompletionTask.TrySetResult(false);
                    return;
                };

            });

            return await _workflowCompletionTask.Task;

        }
        public override void Dispose()
        {

        }
    }
}
