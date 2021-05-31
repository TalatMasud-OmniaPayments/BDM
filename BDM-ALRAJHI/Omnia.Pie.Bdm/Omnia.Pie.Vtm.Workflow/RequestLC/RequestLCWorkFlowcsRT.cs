namespace Omnia.Pie.Vtm.Workflow.RequestLC
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Steps;
	using System;
	using System.Net.Http;

	public class RequestLCWorkFlowcsRT : Workflow
	{
		private readonly IESpaceTerminalCommunication _communicator;

		private readonly LoadCustomerDetailStep _loadCustomerDetailStep;
		private readonly TermsAndConditionsStep _termsAndConditionsStep;
		private readonly CaptureSignatureStep _captureSignatureStep;
		private readonly ConfirmSignatureStep _confirmSignatureStep;
		private readonly ConfirmationLCStep _confirmationLCStep;
		private readonly SendEmailStep _sendEmailStep;
		private readonly SendSmsStep _sendSmsStep;
		private readonly PrintingStep _printingStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;

		public RequestLCWorkFlowcsRT(IResolver container) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.NonFinancial, "Liability Certificate Request-RT");

			_communicator = _container.Resolve<IESpaceTerminalCommunication>();

			_sendEmailStep = _container.Resolve<SendEmailStep>();
			_loadCustomerDetailStep = _container.Resolve<LoadCustomerDetailStep>();
			_termsAndConditionsStep = _container.Resolve<TermsAndConditionsStep>();
			_captureSignatureStep = _container.Resolve<CaptureSignatureStep>();
			_confirmSignatureStep = _container.Resolve<ConfirmSignatureStep>();
			_confirmationLCStep = _container.Resolve<ConfirmationLCStep>();
			_sendSmsStep = _container.Resolve<SendSmsStep>();
			_printingStep = _container.Resolve<PrintingStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _sendEmailStep.Context = _termsAndConditionsStep.Context =
				_loadCustomerDetailStep.Context = _captureSignatureStep.Context = _confirmSignatureStep.Context =
				_confirmationLCStep.Context = _sendSmsStep.Context = _printingStep.Context = CreateContext(typeof(RequestLCContext));

			AddSteps($"{Properties.Resources.StepConfirmation},{Properties.Resources.StepSendingEmail},{Properties.Resources.StepReceiptPrinting}");

			_checkReceiptPrinterStep.CancelAction = _captureSignatureStep.CancelAction = _confirmationLCStep.CancelAction = _termsAndConditionsStep.CancelAction = _confirmationLCStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);
				LoadStandByRT();
			};
			//_confirmSignatureStep.BackAction = () =>
			//{
			//	_captureSignatureStep.Execute();
			//};
			//_captureSignatureStep.DefaultAction = async () =>
			//{
			//	await _confirmSignatureStep.ExecuteAsync();
			//	await _confirmationLCStep.Execute();
			//	await _printingStep.ExecuteAsync();
			//	await _sendEmailStep.SendEmail();
			//	_communicator.SendStatus(StatusEnum.EndCurrentSession);
			//	_journal.TransactionEnded();
			//	LoadStandByRT();
			//};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Request LC - RT Assisted");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

				await _checkReceiptPrinterStep.CheckPrinterAsync();
				await _loadCustomerDetailStep.GetCustomerDetail();
				//await _termsAndConditionsStep.ExecuteAsync();
				// _captureSignatureStep.Execute();
				await _confirmationLCStep.Execute();
				await _sendEmailStep.SendEmail();
				if (Context.Get<IRequestLCContext>().SendSms)
					await _sendSmsStep.ExecuteAsync();

				await _printingStep.ExecuteAsync();
				SendNotification(Services.Interface.TransactionType.RTLCRequest, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier);
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTLCRequest, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the request");
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () => { });
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTLCRequest, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", "", _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish the request");
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () => { });
			}

			_communicator.SendStatus(StatusEnum.EndCurrentSession);
			_journal.TransactionEnded();
			LoadStandByRT();
		}

		public override void Dispose()
		{

		}

	}
}