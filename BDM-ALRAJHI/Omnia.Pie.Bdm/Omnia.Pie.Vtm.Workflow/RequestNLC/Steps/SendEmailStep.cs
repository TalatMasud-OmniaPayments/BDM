namespace Omnia.Pie.Vtm.Workflow.RequestNLC.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
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
            _logger?.Info($"Execute Step: Send EMail");

            SetCurrentStep($"{Properties.Resources.StepSendingEmail}");

			LoadWaitScreen();
			await Task.Delay(100);

			var statementReportData = new LCAndNLCReportData
			{
				TypeofRequest = "NLC",
				TransactionNo = Context.Get<IRequestNLCContext>()?.TSNno,
				CIF = _container.Resolve<ISessionContext>()?.CustomerIdentifier,
				SignatureBase64Content = Context.Get<IRequestNLCContext>().Signature?.ToBase64String(),
				CustomerName = Context.Get<IRequestNLCContext>()?.CustomerDetail?.FullName,
			};

			PopulateAttachment(statementReportData);

			var toEmail = string.Empty;
			var ctx = Context.Get<IRequestNLCContext>();

			if (ctx != null && ctx.SendEmail)
				toEmail = ctx?.CustomerDetail?.Email;

			var _communicationService = _container.Resolve<ICommunicationService>();
			Context.Get<IRequestNLCContext>().TSNno = (await _communicationService.SendEmailAsync(
																			ctx.Attachment,
																			EmailType.NLC,
																			_container.Resolve<ISessionContext>().CustomerIdentifier,
																			ctx?.CustomerDetail?.FullName,
																			DateTime.Now,
																			toEmail,
																			((int)_languageObserver.Language).ToString(),
																			string.Empty))?.ReferenceNumber;
		}

		private void PopulateAttachment(LCAndNLCReportData statementReportData)
		{
			using (var report = _reportsManager.CreateReport(statementReportData))
			{
				var array = report.ExportToPdf();
				Context.Get<IRequestNLCContext>().Attachment = new Attachment() { Content = Convert.ToBase64String(array), FileName = "NLC", MimeType = "application/pdf" };
			}
		}

		public override void Dispose()
		{

		}
	}
}