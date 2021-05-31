namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class ConfirmationNLCStep : WorkflowStep
	{
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly TaskCompletionSource<bool> _completion;
		private readonly IReportsManager _reportsManager;

		public ConfirmationNLCStep(IResolver container) : base(container)
		{
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_receiptFormatter = _container.Resolve<IReceiptFormatter>();
			_completion = new TaskCompletionSource<bool>();
			_reportsManager = container.Resolve<IReportsManager>();
		}

		public async Task<bool> Execute()
		{
            _logger?.Info($"Execute Step: Confirmation for NLC");

            var cancellationToken = new CancellationTokenSource();
			SetCurrentStep($"{Properties.Resources.StepConfirmation}");

			_navigator.RequestNavigationTo<IConfirmationNLCViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
				viewModel.ReferenceNo = string.Empty;

				viewModel.CancelAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					CancelAction?.Invoke();
				};
				viewModel.BackAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					BackAction?.Invoke();
				};
				viewModel.SendEmailAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IRequestNLCContext>().SendEmail = true;
					_completion.TrySetResult(true);
				};
				viewModel.SendSmsAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IRequestNLCContext>().SendSms = true;
					_completion.TrySetResult(true);
				};
				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					_completion.TrySetResult(true);
				};

				if (Context.Get<IRequestNLCContext>().SelfServiceMode)
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