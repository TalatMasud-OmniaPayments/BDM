namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;

	public class LoadAccountDetailStep : WorkflowStep
	{
		public LoadAccountDetailStep(IResolver container) : base(container)
		{

		}

		public async Task GetAccountDetail()
		{
            _logger?.Info($"Execute Step: Get Account Details");

            var ctx = Context.Get<IBalanceEnquiryContext>();
			var _authenticationService = _container.Resolve<ICustomerService>();

			Context.Get<IBalanceEnquiryContext>().AcountDetail =
				await _authenticationService.GetAccountDetail(ctx?.SelectedAccount?.Number, _container?.Resolve<ISessionContext>()?.CustomerIdentifier);
		}

		public override void Dispose()
		{

		}
	}
}