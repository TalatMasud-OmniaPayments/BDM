namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class StatementPrintingStep : WorkflowStep
	{
		private readonly PrintReceiptStep _printReceiptStep;
		private readonly IReceiptFormatter _receiptFormatter;
		private readonly TaskCompletionSource<bool> _completion;
		private readonly IReportsManager _reportsManager;

		public StatementPrintingStep(IResolver container) : base(container)
		{
			_printReceiptStep = _container.Resolve<PrintReceiptStep>();
			_receiptFormatter = _container.Resolve<IReceiptFormatter>();
			_completion = new TaskCompletionSource<bool>();
			_reportsManager = container.Resolve<IReportsManager>();
		}

		public async Task Execute()
		{
            _logger?.Info($"Execute Step: Printing Statement...");

            SetCurrentStep($"{Properties.Resources.StepPrinting}");
			LoadWaitScreen();

			var ctx = Context.Get<IStatementPrintingContext>();

			var receiptData = await _receiptFormatter.FormatAsync(new StatementPrintDeductionReceipt
			{
				CardNumber = ctx?.SelectedAccount?.Number,
				AccountNumber = ctx?.SelectedAccount?.Number,
				TransactionNumber = "NA",
				TransactionStatus = TransactionStatus.Succeeded,
				AuthCode = _container.Resolve<ISessionContext>()?.CustomerIdentifier,
				AccountCurrency = ctx?.SelectedAccount?.Currency,
				AvailableBalance = 0,
				ChargeAmount = ctx?.StatementCharges?.ChargeAmount,
				StatementPeriod = ctx?.NumberofMonths
			});

			_journal.TransactionSucceeded(ctx?.ApplyCharges?.ReferenceNum);
			_journal.PrintingReceipt(receiptData);

			await _printReceiptStep.PrintReceipt(true, receiptData);

			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.PrintingStatement);
			});

			/*var statementReportData = new StatementReportData
			{
				AccountCurrency = ctx?.SelectedAccount?.Currency,
				AccountIban = ctx?.SelectedAccount?.Number,
				AccountNumber = ctx?.SelectedAccount?.Number,
				AccountType = ctx?.SelectedAccount?.Type,
				StartDate = ctx?.StartDate,
				EndDate = ctx?.EndDate,
				NumberOfMonths = ctx?.NumberofMonths,
				BranchLocation = TerminalConfiguration.Section.Location,
				CustomerName = ctx?.SelectedAccount?.Name,
				Items = GetItems(ctx.UserTransactions),
				City = TerminalConfiguration.Section.LocationCity,
				POBox = "63111"
			};

			await PrintStatementAsync(statementReportData);*/

			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.CollectStatement);
			});

			await Task.Delay(4000);

            _logger?.Info($"Execute Step: Printing Statement...Done!");
        }

		private List<StatementItem> GetItems(List<Services.Interface.Entities.UserTransaction> statementItems)
		{
			var lst = new List<StatementItem>();

			if (statementItems != null)
			{
				foreach (var item in statementItems)
				{
					lst.Add(new StatementItem()
					{
						CreditAmount = ToDouble(item?.Amount),
						DebitAmount = 0.0,
						Description = item?.BagSerialNo,
						PostingDate = item?.TransactionDateTime,
						//RunningBalance = 0,
						ValueDate = item?.RequestDateTime,
					});
				}
			}

			return lst;
		}

        public double ToDouble(string Value)
        {
            if (Value == null)
            {
                return 0;
            }
            else
            {
                double OutVal;
                double.TryParse(Value, out OutVal);

                if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                {
                    return 0;
                }
                return OutVal;
            }

        }
        private async Task PrintStatementAsync(StatementReportData statementReportData)
		{
            _logger?.Info($"Execute Task: Create and Print Statement");

            using (var report = _reportsManager.CreateReport(statementReportData))
			{
				report.Print();
			}

			await Task.Delay(7000);
		}

		public override void Dispose()
		{

		}
	}
}