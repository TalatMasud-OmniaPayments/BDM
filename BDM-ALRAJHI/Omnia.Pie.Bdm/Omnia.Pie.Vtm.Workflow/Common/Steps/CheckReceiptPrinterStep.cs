namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Threading.Tasks;

	public class CheckReceiptPrinterStep : WorkflowStep
	{
		IReceiptPrinter _receiptPrinter;
		TaskCompletionSource<bool> completionSource;

		public CheckReceiptPrinterStep(IResolver container) : base(container)
		{
			completionSource = new TaskCompletionSource<bool>();
			_receiptPrinter = container.Resolve<IReceiptPrinter>();
		}

		public async Task<bool> CheckPrinterAsync()
		{
			LoadWaitScreen();

            _logger?.Info($"Execute Step: Check Printer");

            if (_receiptPrinter.GetPrinterStatus() != PrinterStatus.Present)
			{
				var _deviceNotAvailableViewModel = _container.Resolve<IDeviceNotAvailableViewModel>();

				_deviceNotAvailableViewModel.YesAction = () =>
				{
					completionSource.SetResult(true);
				};
				_deviceNotAvailableViewModel.NoAction = () =>
				{
					CancelAction();
					completionSource.SetResult(false);
				};

				_navigator.RequestNavigation(_deviceNotAvailableViewModel);
			}
			else
			{
                completionSource.SetResult(true);
            }

			return await completionSource.Task;
		}

		public override void Dispose()
		{

		}
	}
}