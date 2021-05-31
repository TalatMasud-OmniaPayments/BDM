namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System;
	using System.Threading.Tasks;

	public class SendEmailStep : WorkflowStep
	{
		private readonly IReportsManager _reportsManager;
		private readonly ILanguageObserver _languageObserver;

		public SendEmailStep(IResolver container) : base(container)
		{
			_reportsManager = container.Resolve<IReportsManager>();
			_languageObserver = _container.Resolve<ILanguageObserver>();
		}

		public async Task SendEmail()
		{
            _logger?.Info($"Execute Step: Send Email");

            SetCurrentStep($"{Properties.Resources.StepSendingEmail}");

			LoadWaitScreen();
			await Task.Delay(100);

			var lcNlcData = new LCAndNLCReportData
			{
				TypeofRequest = "LC",
				TransactionNo = Context.Get<IRequestLCContext>()?.TSNno,
				CIF = _container.Resolve<ISessionContext>()?.CustomerIdentifier,
				SignatureBase64Content = Context.Get<IRequestLCContext>()?.Signature?.ToBase64String(),
				CustomerName = Context.Get<IRequestLCContext>()?.CustomerDetail?.FullName,
			};

			PopulateAttachment(lcNlcData);

			var toEmail = string.Empty;
			var ctx = Context.Get<IRequestLCContext>();

			if (ctx != null && ctx.SendEmail)
				toEmail = ctx?.CustomerDetail?.Email;

			var _communicationService = _container.Resolve<ICommunicationService>();
			var result = (await _communicationService.SendEmailAsync(
																		ctx.Attachment,
																		EmailType.LC,
																		_container.Resolve<ISessionContext>().CustomerIdentifier,
																		ctx?.CustomerDetail?.FullName,
																		DateTime.Now,
																		toEmail,
																		((int)_languageObserver.Language).ToString(),
																		string.Empty));

			Context.Get<IRequestLCContext>().TSNno = result?.ReferenceNumber;
		}

		private void PopulateAttachment(LCAndNLCReportData statementReportData)
		{
			using (var report = _reportsManager.CreateReport(statementReportData))
			{
				var array = report.ExportToPdf();
				Context.Get<IRequestLCContext>().Attachment = new Attachment() { Content = Convert.ToBase64String(array), FileName = "LC", MimeType = "application/pdf" };
			}
		}

		public override void Dispose()
		{

		}
	}
}