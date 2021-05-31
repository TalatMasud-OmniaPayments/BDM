using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.RequestLC.Steps
{
	public class LoadAccountStep : WorkflowStep
	{
		public LoadAccountStep(IResolver container) : base(container)
		{

		}

		public override void Dispose()
		{
			
		}

		public async Task GetAccounts()
		{
			_navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.Wait);
			});

			var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IRequestLCContext>().Accounts = await _authenticationService.GetAccounts("329887", AccountCriterion.Loan);

		}
	}
}