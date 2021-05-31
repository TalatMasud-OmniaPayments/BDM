namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System.Linq;
	using System.Threading.Tasks;

	public class GetTransactionHistoryStep : WorkflowStep
	{
		public GetTransactionHistoryStep(IResolver container) : base(container)
		{

		}

		public async Task GetTransactionHistory()
		{
            _logger?.Info($"Execute Step: Get Transaction History");

            LoadWaitScreen();

			var ctx = Context.Get<IStatementPrintingContext>();

			var _customerService = _container.Resolve<ICustomerService>();
			//var items = await _customerService.GetStatementItemAsync(ctx.StartDate, ctx.EndDate, ctx.SelectedAccount.Number, string.Empty);  // Number of Transactions is for Minisatatement
            
                var items = await _customerService.GetUserTransactionsAsync(
                                    _container.Resolve<IAuthDataContext>().Username, ctx.StartDate, ctx.EndDate, ctx.SelectedAccount.Number, string.Empty, "DESC");
            Context.Get<IStatementPrintingContext>().UserTransactions = items;
			//Context.Get<IStatementPrintingContext>().UserTransactions = Context.Get<IStatementPrintingContext>().UserTransactions.OrderBy(x => x.TransactionDateTime?.Date).ThenBy(x => x.TransactionDateTime?.TimeOfDay).ToList();
		}
		public override void Dispose()
		{

		}
	}
}