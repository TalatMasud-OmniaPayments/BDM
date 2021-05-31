namespace Omnia.Pie.Vtm.Workflow.AccountOpening
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.AccountOpening.Steps;

	public class AccountOpeningWorkflow : Workflow
	{
		public AccountOpeningWorkflow(IResolver container) : base(container)
		{
			
		}

		public ScanEmiratesIdStep scanEmiratesIdStep { get; }

		public override void Dispose()
		{
			
		}
	}
}