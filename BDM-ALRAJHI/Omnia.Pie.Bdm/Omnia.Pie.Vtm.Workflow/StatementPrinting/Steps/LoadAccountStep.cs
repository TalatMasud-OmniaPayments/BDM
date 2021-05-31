using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Workflow.Authentication;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	public class LoadAccountStep : WorkflowStep
	{
		public LoadAccountStep(IResolver container) : base(container)
		{

		}
		public async Task GetAccounts()
		{
            _logger?.Info($"Execute Step: Load Account");

            _navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.Wait);
			});

			var _authenticationService = _container.Resolve<IAuthenticationService>();
			Context.Get<IStatementPrintingContext>().Accounts = await _authenticationService.GetAccounts(_container.Resolve<ISessionContext>().CustomerIdentifier, _container.Resolve<IAuthDataContext>().Username, AccountCriterion.Casa);
		}
		public override void Dispose()
		{

		}
	}
}