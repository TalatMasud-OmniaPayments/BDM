namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	internal class ScanFingerprintStep : WorkflowStep
	{
        private readonly TaskCompletionSource<bool> _completion;

        public ScanFingerprintStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Scan Fingerprint");

            SetCurrentStep($"{nameof(ScanFingerprintStep)}");
            
            var cancellationToken = new CancellationTokenSource();

            try
            {
                // TODO: change viewmodel with Scan Fingerprint view model
                _navigator.RequestNavigationTo<IEnterCifViewModel>((viewModel) =>
                {
                    viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
                    viewModel.CancelAction = () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        CancelAction?.Invoke();
                    };
                    viewModel.DefaultAction = () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        try
                        {
                            // TODO: change viewmodel.cif with fingerprint value from viewmodel
                            Context.Get<IAuthDataContext>().Fingerprint = viewModel.Cif;
                            _completion.TrySetResult(true);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
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

                                CancelAction?.Invoke();
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