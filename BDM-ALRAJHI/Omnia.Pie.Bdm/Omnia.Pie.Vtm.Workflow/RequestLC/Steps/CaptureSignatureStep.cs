namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System;

	internal class CaptureSignatureStep : WorkflowStep
	{
		public Action DefaultAction { get; set; }
		private readonly ISignpadScanner _signpadScanner;

		public CaptureSignatureStep(IResolver container) : base(container)
		{
			_signpadScanner = _container.Resolve<ISignpadScanner>();
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Step: Signature Capture");

            await _signpadScanner.ResetAsync();

			try
			{
				_navigator.RequestNavigationTo<ISignaturesViewModel>((viewModel) =>
				{
					viewModel.CancelVisibility = viewModel.DefaultVisibility = true;

					viewModel.BackAction = () =>
					{
						BackAction?.Invoke();
					};

					viewModel.CancelAction = () =>
					{
						CancelAction?.Invoke();
					};
					viewModel.ExpiredAction = async () =>
					{
						var sign = await _signpadScanner.CaptureSignAsync();
						Context.Get<IRequestLCContext>().Signature = viewModel.Signature = sign.Image;
						_logger?.Info("Sign Captured");

						DefaultAction?.Invoke();
					};
					viewModel.DefaultAction = async () =>
					{
						var sign = await _signpadScanner.CaptureSignAsync();
						Context.Get<IRequestLCContext>().Signature = viewModel.Signature = sign.Image;
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