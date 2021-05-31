namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System.Threading.Tasks;

	internal class GetCardsStep : WorkflowStep
	{
		public GetCardsStep(IResolver container) : base(container)
		{
			
		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Get Card");

            LoadWaitScreen();

			var _ndcService = _container.Resolve<INdcService>();
			Context.Get<IAuthDataContext>().Cards = await _ndcService
				.GetEIDACardListAsync(Context.Get<IAuthDataContext>().EIdNumber);
		}

		public override void Dispose()
		{

		}
	}
}