namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System.Threading.Tasks;

	public class LoadCustomerDetailStep : WorkflowStep
	{
		public LoadCustomerDetailStep(IResolver container) : base(container)
		{

		}

		public async Task GetCustomerDetail()
		{
            _logger?.Info($"Execute Step: Get Customer Detail");

            LoadWaitScreen();
			await Task.Delay(100);

			var _customerService = _container.Resolve<ICustomerService>();
			Context.Get<IRequestLCContext>().CustomerDetail = await _customerService.GetCustomerDetail(_container.Resolve<ISessionContext>().CustomerIdentifier);
		}

		public override void Dispose()
		{
			
		}
	}
}