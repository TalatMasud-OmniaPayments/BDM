namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.Workflow.Authentication;
    using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;

	public class SubmitRequestStep : WorkflowStep
	{
		public SubmitRequestStep(IResolver container) : base(container)
		{

		}

		public async Task SubmitRequestAsync()
		{
            _logger?.Info($"Execute Step: Submit Request");

            var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IBalanceEnquiryContext>().Accounts =
				await _authenticationService.GetAccounts(_container?.Resolve<ISessionContext>()?.CustomerIdentifier, _container.Resolve<IAuthDataContext>().Username, AccountCriterion.Casa); // Need to confirm CASA or All
		}

		public override void Dispose()
		{

		}
	}
}