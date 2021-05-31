namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System.Threading.Tasks;

	public class GetChargesStep : WorkflowStep
	{
		public GetChargesStep(IResolver container) : base(container)
		{

		}

		public async Task GetCharges()
		{
            _logger?.Info($"Execute Step: Get Charges Information");

            LoadWaitScreen();

			var ctx = Context.Get<IStatementPrintingContext>();

			var _transactionService = _container.Resolve<ITransactionService>();
			Context.Get<IStatementPrintingContext>().StatementCharges = await _transactionService.GetStatementChargesAsync(
									ctx.StartDate,ctx.EndDate,"0",ctx?.SelectedAccount?.Number,"1", ctx?.NumberofMonths);
		}

		public override void Dispose()
		{

		}
	}
}