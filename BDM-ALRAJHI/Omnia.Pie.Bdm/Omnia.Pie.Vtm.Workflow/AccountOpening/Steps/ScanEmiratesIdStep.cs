namespace Omnia.Pie.Vtm.Workflow.AccountOpening.Steps
{
	using System;
	using Omnia.Pie.Vtm.Framework.Interface;

	public class ScanEmiratesIdStep : WorkflowStep
	{
		public ScanEmiratesIdStep(IResolver container) : base(container)
		{
		}

		public void Execute()
		{

		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}