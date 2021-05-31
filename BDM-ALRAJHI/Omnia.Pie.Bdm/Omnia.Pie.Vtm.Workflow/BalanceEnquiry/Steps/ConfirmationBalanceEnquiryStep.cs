namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class ConfirmationBalanceEnquiryStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public ConfirmationBalanceEnquiryStep(IResolver container) : base(container)
		{
			
		}

		public async Task<bool> Execute()
		{
            _logger?.Info($"Execute Step: Balance Confirmation");

            _completion = new TaskCompletionSource<bool>();
			var cancellationToken = new CancellationTokenSource();

			_navigator.Push<IConfirmationBalanceEnquiryViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
				viewModel.SelectedAccount = Context.Get<IBalanceEnquiryContext>().SelectedAccount;

				if (viewModel.AcountDetail == null)
					viewModel.AcountDetail = new AccountDetailResult();
				viewModel.AcountDetail = Context.Get<IBalanceEnquiryContext>()?.AcountDetail;

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

					_completion.TrySetResult(true);
				};

				if (Context.Get<IBalanceEnquiryContext>().SelfServiceMode)
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