namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.Workflow.Authentication;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System.Threading.Tasks;

	public class GetAccountStep : WorkflowStep
	{
		public GetAccountStep(IResolver container) : base(container)
		{

		}
		public async Task GetAccounts()
		{
            _logger?.Info($"Execute Step: Get Account");

            LoadWaitScreen();
			await Task.Delay(1000);

			var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IRequestIBANContext>().Accounts = await _authenticationService.GetAccounts(_container.Resolve<ISessionContext>().CustomerIdentifier, _container.Resolve<IAuthDataContext>().Username, AccountCriterion.Casa);
		}
		public override void Dispose()
		{

		}
	}
}
