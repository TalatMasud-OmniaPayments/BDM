namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System.Threading.Tasks;

	internal class TermsAndConditionsStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _task;

		public TermsAndConditionsStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Terms and Condition");

            _task = new TaskCompletionSource<bool>();

			_navigator.RequestNavigationTo<ITermsAndConditionsViewModel>((viewModel) =>
			{
				viewModel.CustomerName = Context.Get<IRequestLCContext>().CustomerDetail.FullName;
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

				viewModel.CancelAction = () =>
				{
					CancelAction?.Invoke();
				};
				viewModel.AcceptAction = () =>
				{
					_task.TrySetResult(true);
				};
			});

			return await _task.Task;
		}

		public override void Dispose()
		{

		}
	}
}