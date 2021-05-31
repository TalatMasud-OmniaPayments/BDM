namespace Omnia.Pie.Vtm.Workflow.RequestLC.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System.Threading.Tasks;

	public class GetChargeStep : WorkflowStep
	{
		public GetChargeStep(IResolver container) : base(container)
		{

		}

		public async Task GetCharges()
		{
			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.Wait);
			});

			var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IRequestLCContext>().Accounts = await _authenticationService.GetAccounts("323456", AccountCriterion.Loan);
			Context.Get<IRequestLCContext>().Amount = 50;
			}

		public override void Dispose()
		{

		}
	}
}