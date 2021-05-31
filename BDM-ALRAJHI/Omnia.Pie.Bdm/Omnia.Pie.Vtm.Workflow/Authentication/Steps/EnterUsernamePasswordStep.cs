namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Services.ISO.UserManagement;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Client.Journal.Interface.Extension;

    internal class EnterUsernamePasswordStep : WorkflowStep
	{
        private TaskCompletionSource<bool> _completion;

        private readonly FingerprintScanningStep _fingerprintScanningStep;
        public EnterUsernamePasswordStep(IResolver container) : base(container)
		{

            _fingerprintScanningStep = _container.Resolve<FingerprintScanningStep>();
            
        }

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Enter Username and Password");
            _completion = new TaskCompletionSource<bool>();
            SetCurrentStep($"{nameof(EnterUsernamePasswordStep)}");
            var cancellationToken = new CancellationTokenSource();
            var ct = Context.Get<IAuthDataContext>();
           

            try
            {
                // TODO: change viewmodel with Login view model
                _navigator.RequestNavigationTo<ILoginViewModel>((viewModel) =>
                {
                    viewModel.DefaultVisibility = viewModel.BackVisibility = true;
                    viewModel.BackAction = () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        BackAction?.Invoke();
                    };
                    viewModel.DefaultAction = async () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                       
                            // TODO: change viewmodel.cif with username and password from viewmodel

                            Context.Get<IAuthDataContext>().Username = viewModel.SelectedUserName;
                            Context.Get<IAuthDataContext>().Password = viewModel.Password;

                           

                            _completion.TrySetResult(true);
                            return;
                        
                    };
                   

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

                                BackAction?.Invoke();
                            };
                        });
                    };
                });

                return await _completion.Task;
            }
            catch (Exception ex)
            {
                _completion.TrySetException(ex);
                throw;
            }

        }

		public override void Dispose()
		{

		}
	}
}