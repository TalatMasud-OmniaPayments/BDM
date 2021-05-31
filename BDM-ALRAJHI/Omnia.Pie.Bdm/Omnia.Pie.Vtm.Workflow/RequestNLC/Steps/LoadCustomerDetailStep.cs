using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Steps
{
	public class LoadCustomerDetailStep : WorkflowStep
	{
		public LoadCustomerDetailStep(IResolver container) : base(container)
		{

		}
		public async Task GetCustomerDetail()
		{
            _logger?.Info($"Execute Step: Get Customer Detail");

            _navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
			{
				vm.Type(AnimationType.Wait);
			});

			var _customerService = _container.Resolve<ICustomerService>();
			Context.Get<IRequestNLCContext>().CustomerDetail = await _customerService.GetCustomerDetail(_container.Resolve<ISessionContext>().CustomerIdentifier);
		}
		public override void Dispose()
		{

		}
	}
}