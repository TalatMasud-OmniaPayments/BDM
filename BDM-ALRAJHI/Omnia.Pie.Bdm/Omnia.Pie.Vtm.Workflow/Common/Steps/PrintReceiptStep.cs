namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading.Tasks;

	internal class PrintReceiptStep : WorkflowStep
	{
		private readonly IReceiptPrinter _receiptPrinter;

		public PrintReceiptStep(IResolver container) : base(container)
		{
			_receiptPrinter = _container.Resolve<IReceiptPrinter>();
		}

		public async Task PrintReceipt(bool printOpted, string printData)
		{
            _logger?.Info($"Execute Step: Print Receipt");

            SetCurrentStep(Properties.Resources.StepReceiptPrinting);

			try
			{
				printOpted = true;

				if (_container.Resolve<IReceiptPrinter>().GetPrinterStatus() != PrinterStatus.Present)
				{
					return;
				}

				if (printOpted)
				{
					_navigator.RequestNavigationTo<IAnimationViewModel>((vmAnimation) =>
					{
						vmAnimation.Type(AnimationType.PrintingReceipt);
					});

					await _receiptPrinter.PrintAsync(printData);

					_navigator.RequestNavigationTo<IAnimationViewModel>((vmAnimation) =>
					{
						vmAnimation.Type(AnimationType.TakeReceipt);
					});

					await _receiptPrinter.EjectAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				throw new DeviceOperationCanceledException("Receipt failed");
			}
		}

		public override void Dispose()
		{

		}
	}
}