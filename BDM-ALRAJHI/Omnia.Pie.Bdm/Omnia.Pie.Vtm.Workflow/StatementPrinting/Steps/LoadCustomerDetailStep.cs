namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;

	internal class LoadCustomerDetailStep : WorkflowStep
	{
		public LoadCustomerDetailStep(IResolver container) : base(container)
		{

		}
		public async Task GetCustomerDetail()
		{
            _logger?.Info($"Execute Step: Load Customer Detail");

            _navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.Wait);
			});

			var _customerService = _container.Resolve<ICustomerService>();
            //Context.Get<IStatementPrintingContext>().CustomerDetail = await _customerService.GetCustomerDetail(_container.Resolve<IAuthDataContext>().Username, _container.Resolve<IAuthDataContext>().Username);
            Context.Get<IStatementPrintingContext>().CustomerDetail = await _customerService.GetCustomerDetail("testuser", _container.Resolve<IAuthDataContext>().Username);

        }
        public override void Dispose()
		{

		}
	}
}