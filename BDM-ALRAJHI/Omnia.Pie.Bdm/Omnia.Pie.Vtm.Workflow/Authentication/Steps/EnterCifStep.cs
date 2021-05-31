namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal class EnterCifStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _completion;

		public EnterCifStep(IResolver container) : base(container)
		{
			_completion = new TaskCompletionSource<bool>();
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Enter CIF");

            SetCurrentStep($"{Properties.Resources.StepEnterCif}");
			var cancellationToken = new CancellationTokenSource();

			try
			{
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
							Context.Get<IAuthDataContext>().Cif = viewModel.Cif;
							_completion.TrySetResult(true);
						}
						catch (Exception ex)
						{
							throw ex;
						}
					};

					if (Context.Get<IAuthDataContext>().SelfServiceMode)
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

									CancelAction?.Invoke();
								};
							});
						};
					}
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