namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts.Receipts;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System;
	using System.Threading.Tasks;

	public class PrintStep : WorkflowStep
	{
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly IReportsManager _reportsManager;

		public PrintStep(IResolver container) : base(container)
		{
			_receiptFormatter = _container.Resolve<IReceiptFormatter>();
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_reportsManager = container.Resolve<IReportsManager>();
		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Printing IBAN");

            SetCurrentStep($"{Properties.Resources.StepPrinting}");
			LoadWaitScreen();

			await Task.Delay(1000);

			var ctx = Context.Get<IRequestIBANContext>();

			var receiptData = await _receiptFormatter.FormatAsync(new RequestIBANReceipt
			{
				CardNumber = _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber,
				AccountNumber = ctx?.SelectedAccount?.Number,
				TransactionNumber = ctx?.TSNno,
				TransactionStatus = TransactionStatus.Succeeded,
				Currency = ctx?.SelectedAccount?.Currency,
				AvailableBalance = 0.0,
				ReferenceNo = ctx?.TSNno,
				VTMID = TerminalConfiguration.Section?.Id
			});

			_journal.TransactionSucceeded(ctx?.TSNno);
			await _printReceiptStep.PrintReceipt(true, receiptData);
			_journal.PrintingReceipt(receiptData);

			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.PrintingIBAN);
			});

			var ibanReportData = new AccountDetailReportData
			{
				AccountNumber = ctx?.AcountDetail?.AccountNumber,
				IBAN = ctx?.AcountDetail?.IBAN,
				AccountType = ctx?.AcountDetail?.AccountType,
				AccountCurrency = ctx?.AcountDetail?.AccountCurrency,
				AccountOpenedSince = ctx?.AcountDetail?.AccountOpenDate,
				AccountStatus = ctx?.AcountDetail?.AccountStatus,
				AccountTitle = ctx?.AcountDetail?.AccountTitle,
				BranchName = TerminalConfiguration.Section?.Location,
				BranchNameArabic = TerminalConfiguration.Section?.Location,
				AvailableBalance = ctx?.AcountDetail?.AvailableBalance?.ToString(),
				ReferenceNo = Context.Get<IRequestIBANContext>().TSNno
			};

			await PrintIBANAsync(ibanReportData);

			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.CollectIBAN);
			});

			await Task.Delay(4000);
		}

		private async Task PrintIBANAsync(AccountDetailReportData statementReportData)
		{
			using (var report = _reportsManager.CreateReport(statementReportData))
			{
				var array = report.ExportToPdf();
				Context.Get<IRequestIBANContext>().Attachment = new Attachment() { Content = Convert.ToBase64String(array), FileName = "IBAN", MimeType = "application/pdf" };
				report.Print();
			}

			await Task.Delay(7000);
		}

		public override void Dispose()
		{

		}
	}
}