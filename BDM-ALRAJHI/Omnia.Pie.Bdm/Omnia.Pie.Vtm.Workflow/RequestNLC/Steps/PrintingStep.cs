namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts.Receipts;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
	using System.Threading.Tasks;

	public class PrintingStep : WorkflowStep
	{
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly IReceiptFormatter _receiptFormatter;
		
		public PrintingStep(IResolver container) : base(container)
		{
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_receiptFormatter = _container.Resolve<IReceiptFormatter>();	
		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Print NLC");

            SetCurrentStep($"{Properties.Resources.StepReceiptPrinting}");

			LoadWaitScreen();
			await Task.Delay(1000);

			var receipt = new RequestNLCReceipt
			{
				CardNumber = _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber,
				AccountNumber = _container.Resolve<ISessionContext>()?.AccountNumber,
				TransactionNumber = Context.Get<IRequestNLCContext>()?.TSNno,
				TransactionStatus = TransactionStatus.Succeeded,
				TransactionCurrency = TerminalConfiguration.Section?.Currency,
                ReferenceNo = Context.Get<IRequestNLCContext>()?.TSNno,
				VTMID = TerminalConfiguration.Section?.Id
			};

			_journal.TransactionSucceeded(Context.Get<IRequestNLCContext>()?.TSNno);
			var receiptData = await _receiptFormatter.FormatAsync(receipt);
			_journal.PrintingReceipt(receiptData);

			await _printReceiptStep.PrintReceipt(true, receiptData);
		}

		public override void Dispose()
		{
			
		}
	}
}