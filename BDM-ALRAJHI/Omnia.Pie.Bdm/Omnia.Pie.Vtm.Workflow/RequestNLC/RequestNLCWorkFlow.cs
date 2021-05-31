namespace Omnia.Pie.Vtm.Workflow.RequestNLC
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Context;
	using Omnia.Pie.Vtm.Workflow.RequestNLC.Steps;
	using System;
	using System.Net.Http;

	public class RequestNLCWorkFlow : Workflow
	{
		private readonly LoadCustomerDetailStep _loadCustomerDetailStep;
		private readonly TermsAndConditionsStep _termsAndConditionsStep;
		private readonly CaptureSignatureStep _captureSignatureStep;
		private readonly ConfirmSignatureStep _confirmSignatureStep;
		private readonly ConfirmationNLCStep _confirmationNLCStep;
		private readonly PrintingStep _printingStep;
		private readonly SendSmsStep _sendSmsStep;
		private readonly SendEmailStep _sendEmailStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;

		public RequestNLCWorkFlow(IResolver container) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.NonFinancial, "Non-Liability Certificate Request-SS");

			_loadCustomerDetailStep = _container.Resolve<LoadCustomerDetailStep>();
			_termsAndConditionsStep = _container.Resolve<TermsAndConditionsStep>();
			_captureSignatureStep = _container.Resolve<CaptureSignatureStep>();
			_confirmSignatureStep = _container.Resolve<ConfirmSignatureStep>();
			_confirmationNLCStep = _container.Resolve<ConfirmationNLCStep>();
			_printingStep = _container.Resolve<PrintingStep>();
			_sendSmsStep = _container.Resolve<SendSmsStep>();
			_sendEmailStep = _container.Resolve<SendEmailStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _confirmSignatureStep.Context = _sendEmailStep.Context =
				_termsAndConditionsStep.Context = _loadCustomerDetailStep.Context = _captureSignatureStep.Context =
				_printingStep.Context = _sendSmsStep.Context = _confirmationNLCStep.Context = CreateContext(typeof(RequestNLCContext));

			AddSteps($"{Properties.Resources.StepConfirmation},{Properties.Resources.StepSendingEmail},{Properties.Resources.StepReceiptPrinting}");

			_checkReceiptPrinterStep.CancelAction = _captureSignatureStep.CancelAction = _confirmSignatureStep.CancelAction = _termsAndConditionsStep.CancelAction = _confirmationNLCStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				LoadSelfServiceMenu();
			};

			//_confirmSignatureStep.BackAction = () =>
			//{
			//	_captureSignatureStep.Execute();
			//};
			//_captureSignatureStep.DefaultAction = async () =>
			//{
			//	await _confirmSignatureStep.ExecuteAsync();
			//	await _confirmationNLCStep.Execute();
			//	await _sendEmailStep.SendEmail();
			//	await _printingStep.ExecuteAsync();
			//	FinishTransaciton();
			//};

			Context.Get<IRequestNLCContext>().SelfServiceMode = true;
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Request NLC - Self Service");

            try
			{
				await _checkReceiptPrinterStep.CheckPrinterAsync();
				await _loadCustomerDetailStep.GetCustomerDetail();
				//await _termsAndConditionsStep.ExecuteAsync();
				//_captureSignatureStep.Execute();
				await _confirmationNLCStep.Execute();
				await _sendEmailStep.SendEmail();
				if (Context.Get<IRequestNLCContext>().SendSms)
					await _sendSmsStep.ExecuteAsync();

				await _printingStep.ExecuteAsync();
				SendNotification(Services.Interface.TransactionType.SSNLCRequest, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier);
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.SSNLCRequest, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the request");
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () => { });
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.SSNLCRequest, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the request");
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () => { });
			}
			FinishTransaciton();
		}

		public override void Dispose()
		{

		}
	}
}