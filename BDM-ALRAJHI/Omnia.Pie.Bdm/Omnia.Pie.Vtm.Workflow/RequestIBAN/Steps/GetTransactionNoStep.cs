namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System.Threading.Tasks;

	public class GetTransactionNoStep : WorkflowStep
	{
		public GetTransactionNoStep(IResolver container) : base(container)
		{

		}

		public async Task GetTSNno()
		{
            _logger?.Info($"Execute Step: Get Transaction No");

            LoadWaitScreen();

			var _communicationService = _container.Resolve<ICommunicationService>();
			Context.Get<IRequestIBANContext>().TSNno = (await _communicationService.GenerateTSNAsync())?.value;
		}

		public override void Dispose()
		{

		}
	}
}