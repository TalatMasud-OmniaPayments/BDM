namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal class ConfirmationLCStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _completion;

		public ConfirmationLCStep(IResolver container) : base(container)
		{
			_completion = new TaskCompletionSource<bool>();
		}

		public async Task<bool> Execute()
		{
            _logger?.Info($"Execute Step: Confirmation for LC");

            var cancellationToken = new CancellationTokenSource();
			SetCurrentStep($"{Properties.Resources.StepConfirmation}");

			_navigator.RequestNavigationTo<IConfirmationLCViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
				viewModel.ReferenceNo = string.Empty;

				viewModel.BackAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					BackAction?.Invoke();
				};

				viewModel.CancelAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					CancelAction?.Invoke();
				};
				viewModel.SendEmailAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IRequestLCContext>().SendEmail = true;
					_completion.TrySetResult(true);
				};
				viewModel.SendSmsAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IRequestLCContext>().SendSms = true;
					_completion.TrySetResult(true);
				};
				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					_completion.TrySetResult(true);
				};

				if (Context.Get<IRequestLCContext>().SelfServiceMode)
				{
					viewModel.StartUserActivityTimer(cancellationToken.Token);
					viewModel.ExpiredAction = () =>
					{
						_navigator.Push<IActiveConfirmationViewModel>((vm) =>
						{
							vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
							vm.YesAction = () =>
							{
								cancellationToken = new CancellationTokenSource();
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

		public override void Dispose()
		{

		}
	}
}