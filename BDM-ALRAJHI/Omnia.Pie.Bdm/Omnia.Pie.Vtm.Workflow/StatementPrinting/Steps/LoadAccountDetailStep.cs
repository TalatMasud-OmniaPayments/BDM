using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	class LoadAccountDetailStep : WorkflowStep
	{
		public LoadAccountDetailStep(IResolver container) : base(container)
		{

		}
		public async Task GetAccountDetail()
		{
            _logger?.Info($"Execute Step: Get Account Detail");

            LoadWaitScreen();

			var ctx = Context.Get<IStatementPrintingContext>();

			var _authenticationService = _container.Resolve<ICustomerService>();
            //Context.Get<IStatementPrintingContext>().AcountDetail = await _authenticationService.GetAccountDetail(ctx?.SelectedAccount?.Number, _container.Resolve<ISessionContext>().CustomerIdentifier);
            Context.Get<IStatementPrintingContext>().AcountDetail =
                await _authenticationService.GetAccountDetail(_container.Resolve<IAuthDataContext>().Username, _container?.Resolve<ISessionContext>()?.CustomerIdentifier, "1");
        }

        public override void Dispose()
		{

		}
	}
}