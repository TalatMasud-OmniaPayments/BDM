namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using System.Threading.Tasks;

	public class PrintingStep : WorkflowStep
	{
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly IReportsManager _reportsManager;

		public PrintingStep(IResolver container) : base(container)
		{
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_receiptFormatter = _container.Resolve<IReceiptFormatter>();
			_reportsManager = container.Resolve<IReportsManager>();
		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Print Receipt");

            LoadWaitScreen();
			await Task.Delay(1000);

			var receiptData = await _receiptFormatter.FormatAsync(new BalanceInquiryReceipt
			{
				CardNumber = _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber,
				AccountNumber = Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Number,
				TransactionNumber = Context.Get<IBalanceEnquiryContext>()?.TSNno?.value,
				TransactionStatus = TransactionStatus.Succeeded,
				Currency = Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Currency,
				AvailableBalance = 0.0,
			});

			_journal.PrintingReceipt(receiptData);
			await _printReceiptStep.PrintReceipt(true, receiptData);

		}

		public override void Dispose()
		{

		}
	}
}