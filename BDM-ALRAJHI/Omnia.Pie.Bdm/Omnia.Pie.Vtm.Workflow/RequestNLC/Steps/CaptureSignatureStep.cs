namespace Omnia.Pie.Vtm.Workflow.RequestNLC
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
	using System;

	public class CaptureSignatureStep : WorkflowStep
	{
		private readonly ISignpadScanner _signpadScanner;
		public Action DefaultAction { get; set; }

		public CaptureSignatureStep(IResolver container) : base(container)
		{
			_signpadScanner = _container.Resolve<ISignpadScanner>();
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Step: Capture Signature");

            await _signpadScanner.ResetAsync();

			try
			{
				_navigator.RequestNavigationTo<Bootstrapper.Interface.ViewModels.RequestNLC.ISignaturesViewModel>((viewModel) =>
				{
					viewModel.CancelVisibility = viewModel.DefaultVisibility = true;

					viewModel.CancelAction = () =>
					{
						CancelAction?.Invoke();
					};
					viewModel.ExpiredAction = async () =>
					{
						var sign = await _signpadScanner.CaptureSignAsync();
						Context.Get<IRequestNLCContext>().Signature = viewModel.Signature = sign.Image;
						_logger?.Info("Sign Captured");

						DefaultAction?.Invoke();
					};
					viewModel.DefaultAction = async () =>
					{
						var sign = await _signpadScanner.CaptureSignAsync();
						Context.Get<IRequestNLCContext>().Signature = viewModel.Signature = sign.Image;
						_logger?.Info("Sign Captured");

						DefaultAction?.Invoke();
					};

					viewModel.StartTimer(new TimeSpan(0, 0, 15));
				});
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