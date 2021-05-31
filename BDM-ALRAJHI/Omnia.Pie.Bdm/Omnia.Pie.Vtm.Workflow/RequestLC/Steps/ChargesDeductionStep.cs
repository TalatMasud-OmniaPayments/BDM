using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.RequestLC.Steps
{
	public class ChargesDeductionStep : WorkflowStep
	{
		public ChargesDeductionStep(IResolver container) : base(container)
		{

		}
		
		public async Task DeductCharges()
		{
			var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IRequestLCContext>().Accounts = await _authenticationService.GetAccounts("329887", AccountCriterion.Loan);
		}

		public override void Dispose()
		{

		}
	}
}
