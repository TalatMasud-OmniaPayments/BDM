namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System.Threading.Tasks;

	public class GetCustomerDetailStep : WorkflowStep
	{
		public GetCustomerDetailStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Load Customer Details");

            LoadWaitScreen();
			await Task.Delay(100);

			var _customerService = _container.Resolve<ICustomerService>();
			Context.Get<IRequestIBANContext>().CustomerDetail = await _customerService.GetCustomerDetail(_container.Resolve<ISessionContext>().CustomerIdentifier);
		}

		public override void Dispose()
		{

		}
	}
}