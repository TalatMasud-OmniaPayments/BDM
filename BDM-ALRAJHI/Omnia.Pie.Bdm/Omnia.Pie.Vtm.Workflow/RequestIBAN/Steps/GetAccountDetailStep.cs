namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System.Threading.Tasks;

	public class GetAccountDetailStep : WorkflowStep
	{
		public GetAccountDetailStep(IResolver container) : base(container)
		{

		}

		public async Task GetAccountDetail()
		{
            _logger?.Info($"Execute Step: Get Account Detail");

            LoadWaitScreen();
			await Task.Delay(100);

			var ctx = Context.Get<IRequestIBANContext>();
			var _authenticationService = _container.Resolve<ICustomerService>();

			Context.Get<IRequestIBANContext>().AcountDetail = 
				await _authenticationService.GetAccountDetail(ctx?.SelectedAccount?.Number, _container.Resolve<ISessionContext>().CustomerIdentifier);
		}
		
		public override void Dispose()
		{

		}
	}
}