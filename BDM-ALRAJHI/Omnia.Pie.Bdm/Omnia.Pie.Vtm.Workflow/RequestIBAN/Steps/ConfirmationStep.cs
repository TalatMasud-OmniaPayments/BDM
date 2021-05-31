namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class ConfirmationStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _completion;

		public ConfirmationStep(IResolver container) : base(container)
		{
			_completion = new TaskCompletionSource<bool>();
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Confirmation for IBAN");

            var cancellationToken = new CancellationTokenSource();
			SetCurrentStep($"{Properties.Resources.StepConfirmation}");

			_navigator.RequestNavigationTo<IConfirmationRequestIBANViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

				viewModel.SelectedAccount = Context.Get<IRequestIBANContext>()?.SelectedAccount;

				if (viewModel.AcountDetail == null)
					viewModel.AcountDetail = new AccountDetailResult();

				viewModel.AcountDetail = Context.Get<IRequestIBANContext>()?.AcountDetail;

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

					Context.Get<IRequestIBANContext>().SendEmail = true;
					_completion.TrySetResult(true);
				};
				viewModel.SendSmsAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IRequestIBANContext>().SendSms = true;
					_completion.TrySetResult(true);
				};
				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

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