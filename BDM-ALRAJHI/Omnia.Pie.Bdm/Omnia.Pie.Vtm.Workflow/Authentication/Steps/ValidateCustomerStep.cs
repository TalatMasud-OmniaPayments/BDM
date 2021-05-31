namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System.Threading.Tasks;

	internal class ValidateCustomerStep : WorkflowStep
	{
		public ValidateCustomerStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate Customer");

            LoadWaitScreen();
			SetCurrentStep($"{nameof(ValidateCustomerStep)}");

			var _customerService = _container.Resolve<ICustomerService>();
			var _customer = await _customerService.GetCustomerDetail(Context.Get<IAuthDataContext>().Cif);

			Context.Get<IAuthDataContext>().CustomerId = Context.Get<IAuthDataContext>().Cif;
			Context.Get<IAuthDataContext>().EIdNumber = _customer.EmiratesId;
			_journal.CIF(Context.Get<IAuthDataContext>()?.CustomerId);
		}

		public override void Dispose()
		{

		}
	}
}