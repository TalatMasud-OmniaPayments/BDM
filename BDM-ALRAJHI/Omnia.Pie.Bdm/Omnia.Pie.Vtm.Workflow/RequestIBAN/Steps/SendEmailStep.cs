namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using System;
	using System.Threading.Tasks;

	public class SendEmailStep : WorkflowStep
	{
		private readonly ILanguageObserver _languageObserver;

		public SendEmailStep(IResolver container) : base(container)
		{
			_languageObserver = _container.Resolve<ILanguageObserver>();
		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Send EMail");

            LoadWaitScreen();
			await Task.Delay(100);

			var toEmail = string.Empty;
			var ctx = Context.Get<IRequestIBANContext>();

			if (ctx != null && ctx.SendEmail)
				toEmail = ctx?.CustomerDetail?.Email;

			var _communicationService = _container.Resolve<ICommunicationService>();
			Context.Get<IRequestIBANContext>().TSNno = (await _communicationService.SendEmailAsync(
																		ctx.Attachment,
																		EmailType.IBAN,
																		_container.Resolve<ISessionContext>().CustomerIdentifier,
																		ctx?.CustomerDetail?.FullName,
																		DateTime.Now,
																		toEmail,
																		((int)_languageObserver.Language).ToString(),
																		ctx?.AcountDetail?.IBAN))?.ReferenceNumber;
		}

		public override void Dispose()
		{

		}
	}
}