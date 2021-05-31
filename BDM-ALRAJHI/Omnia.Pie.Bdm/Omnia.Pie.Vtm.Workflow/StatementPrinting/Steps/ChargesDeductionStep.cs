namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System.Threading.Tasks;

	public class ChargesDeductionStep : WorkflowStep
	{
		public ChargesDeductionStep(IResolver container) : base(container)
		{

		}

		public async Task DeductCharges()
		{
            _logger?.Info($"Execute Step: Deduct Charges");

            LoadWaitScreen();

			var ctx = Context.Get<IStatementPrintingContext>();

			if (ctx != null && decimal.Parse(ctx?.StatementCharges?.ChargeAmount) > 0)
			{
				var _transactionService = _container.Resolve<ITransactionService>();
				Context.Get<IStatementPrintingContext>().ApplyCharges = await _transactionService.ApplyStatementChargesAsync(
							ctx.StartDate, ctx.EndDate, ctx?.SelectedAccount.Number,
							_container?.Resolve<ISessionContext>()?.CustomerIdentifier, ctx?.AcountDetail?.BranchId);
			}
		}

		public override void Dispose()
		{

		}
	}
}