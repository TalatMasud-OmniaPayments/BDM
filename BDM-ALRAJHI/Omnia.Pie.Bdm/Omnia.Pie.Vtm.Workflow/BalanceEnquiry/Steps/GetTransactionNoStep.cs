namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using System.Threading.Tasks;

	internal class GetTransactionNoStep : WorkflowStep
	{
		public GetTransactionNoStep(IResolver container) : base(container)
		{

		}

		public async Task GetTSNno()
		{
            _logger?.Info($"Execute Step: Get Transaction Serial No");

            LoadWaitScreen();
			await Task.Delay(1000);

			var _communicationService = _container.Resolve<ICommunicationService>();
			Context.Get<IBalanceEnquiryContext>().TSNno = await _communicationService.GenerateTSNAsync();
		}

		public override void Dispose()
		{

		}
	}
}