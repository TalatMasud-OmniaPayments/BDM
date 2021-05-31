namespace Omnia.Pie.Vtm.Workflow.MainMenu.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using System;
	using System.Threading.Tasks;

	public class SendMoreInfoStep : WorkflowStep
	{
		private readonly ILanguageObserver _languageObserver;

		public SendMoreInfoStep(IResolver container) : base(container)
		{
			_languageObserver = _container.Resolve<ILanguageObserver>();
		}

		public async Task ExecuteAsync(string type, string email, string mobile)
		{
            _logger?.Info($"Execute Step: Send More Information");

            try
			{
				LoadWaitScreen();

				var _customerService = _container.Resolve<ICustomerService>();
				await _customerService.ProductInfoAsync(type, mobile, email, ((int)_languageObserver.Language).ToString());
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () => { });
			}
		}

		public override void Dispose()
		{

		}
	}
}