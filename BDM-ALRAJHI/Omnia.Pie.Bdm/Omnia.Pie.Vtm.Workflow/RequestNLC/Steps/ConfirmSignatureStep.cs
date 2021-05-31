using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Steps
{
	public class ConfirmSignatureStep : WorkflowStep
	{
		private readonly ISignpadScanner _signpadScanner;
		private TaskCompletionSource<bool> _completion;

		public ConfirmSignatureStep(IResolver container) : base(container)
		{
			_signpadScanner = _container.Resolve<ISignpadScanner>();
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Confirm Signature");

            _completion = new TaskCompletionSource<bool>();
			await _signpadScanner.ResetAsync();

			try
			{
				_navigator.RequestNavigationTo<Bootstrapper.Interface.ViewModels.RequestNLC.ISignatureConfirmationViewModel>((viewModel) =>
				{
					viewModel.CancelVisibility = viewModel.BackVisibility = viewModel.DefaultVisibility = true;
					viewModel.Signature = Context.Get<IRequestNLCContext>().Signature;

					viewModel.BackAction = () =>
					{
						BackAction?.Invoke();
					};
					viewModel.CancelAction = () =>
					{
						CancelAction?.Invoke();
					};
					viewModel.DefaultAction = () =>
					{
						if (viewModel?.Signature != null)
						{
							_completion.TrySetResult(true);
						}
					};
				});

				return await _completion.Task;
			}
			finally
			{
				await _signpadScanner.ResetAsync();
			}
		}

		public override void Dispose()
		{

		}
	}
}