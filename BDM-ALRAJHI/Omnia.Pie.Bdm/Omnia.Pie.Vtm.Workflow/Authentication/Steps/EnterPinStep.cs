namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class EnterPinStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public EnterPinStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Enter PIN");

            _completion = new TaskCompletionSource<bool>();
			var cancellationToken = new CancellationTokenSource();

			SetCurrentStep($"{Properties.Resources.StepEnterPin}");

			_navigator.RequestNavigationTo<IEnterPinViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

				viewModel.CancelAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					CancelAction?.Invoke();
				};
				viewModel.FourDigitLength = viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					try
					{
						Context.Get<IAuthDataContext>().Pin = viewModel.PinBlock;
						_completion.TrySetResult(true);
					}
					catch (Exception e)
					{
						_completion.TrySetException(e);
						throw e;
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

		public override void Dispose()
		{

		}
	}
}