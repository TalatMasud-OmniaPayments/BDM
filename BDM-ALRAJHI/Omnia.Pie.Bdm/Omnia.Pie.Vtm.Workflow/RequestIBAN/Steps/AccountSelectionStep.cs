namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class AccountSelectionStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public AccountSelectionStep(IResolver container) : base(container)
		{
			SetCurrentStep($"{Properties.Resources.StepAccountSelection}");
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Account Selection");

            SetCurrentStep($"{Properties.Resources.StepAccountSelection}");
			_completion = new TaskCompletionSource<bool>();
			var cancellationToken = new CancellationTokenSource();

			_navigator.RequestNavigationTo<IRequestIBANViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

				viewModel.Accounts = Context.Get<IRequestIBANContext>()?.Accounts;
				viewModel.SelectedAccount = Context.Get<IRequestIBANContext>()?.SelectedAccount;
				viewModel.IBANNo = Context.Get<IRequestIBANContext>()?.AcountDetail?.IBAN;

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

					Context.Get<IRequestIBANContext>().SelectedAccount = viewModel.SelectedAccount;
					_completion.TrySetResult(true);
				};

				if (Context.Get<IRequestIBANContext>().SelfServiceMode)
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