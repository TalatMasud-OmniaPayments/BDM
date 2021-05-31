namespace Omnia.Pie.Vtm.Workflow.RequestIBAN
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN.Steps;
	using System;
	using System.Linq;

	public class RequestIBANWorkFlowRT : Workflow
	{
		private readonly IESpaceTerminalCommunication _communicator;
		private readonly IStatementPrinter _statementPrinter;

		private readonly AccountSelectionStep _accountSelectionStep;
		private readonly GetAccountDetailStep _loadAccountDetailStep;
		private readonly GetAccountStep _loadAccountStep;
		private readonly ConfirmationStep _confirmationStep;
		private readonly GetTransactionNoStep _getTransactionNoStep;
		private readonly GetCustomerDetailStep _getCustomerDetailStep;
		private readonly SendSmsStep _sendSmsStep;
		private readonly SendEmailStep _sendEmailStep;
		private readonly PrintStep _printStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;

		public RequestIBANWorkFlowRT(IResolver container) : base(container)
		{
			_communicator = _container.Resolve<IESpaceTerminalCommunication>();
			_statementPrinter = _container.Resolve<IStatementPrinter>();

			_journal.TransactionStarted(EJTransactionType.NonFinancial, "IBAN Letter Request-RT");

			_accountSelectionStep = _container.Resolve<AccountSelectionStep>();
			_loadAccountStep = _container.Resolve<GetAccountStep>();
			_confirmationStep = _container.Resolve<ConfirmationStep>();
			_loadAccountDetailStep = _container.Resolve<GetAccountDetailStep>();
			_getTransactionNoStep = _container.Resolve<GetTransactionNoStep>();
			_getCustomerDetailStep = _container.Resolve<GetCustomerDetailStep>();
			_sendSmsStep = _container.Resolve<SendSmsStep>();
			_sendEmailStep = _container.Resolve<SendEmailStep>();
			_printStep = _container.Resolve<PrintStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _accountSelectionStep.Context = _loadAccountStep.Context = _confirmationStep.Context =
				_printStep.Context = _getTransactionNoStep.Context = _loadAccountDetailStep.Context = _getCustomerDetailStep.Context =
				_sendSmsStep.Context = _sendEmailStep.Context = CreateContext(typeof(RequestIBANContext));

			AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepConfirmation},{Properties.Resources.StepPrinting}");

			_checkReceiptPrinterStep.CancelAction = _accountSelectionStep.CancelAction = _confirmationStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);
				LoadStandByRT();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Request IBAN - RT Assisted");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

				if (_statementPrinter.Status == DeviceStatus.Online && _statementPrinter.GetPrinterStatus() == PrinterStatus.Present)
				{
					await _checkReceiptPrinterStep.CheckPrinterAsync();

					await _loadAccountStep.GetAccounts();

					Context.Get<IRequestIBANContext>().SelectedAccount = Context.Get<IRequestIBANContext>()?.Accounts?.FirstOrDefault();
					_journal.AccountSelected(Context.Get<IRequestIBANContext>()?.SelectedAccount?.Number);

					await _accountSelectionStep.ExecuteAsync();
					await _loadAccountDetailStep.GetAccountDetail();
					await _getTransactionNoStep.GetTSNno();
					await _confirmationStep.ExecuteAsync();

					try
					{
						_statementPrinter.TurnOnGuideLights();
						await _printStep.ExecuteAsync();
					}
					catch (Exception e)
					{
						throw e;
					}
					finally
					{
						_statementPrinter.TurnOffGuideLights();
					}

					if (Context.Get<IRequestIBANContext>().SendSms)
						await _sendSmsStep.ExecuteAsync();

					if (Context.Get<IRequestIBANContext>().SendEmail)
					{
						await _getCustomerDetailStep.ExecuteAsync();
						await _sendEmailStep.ExecuteAsync();
					}
					SendNotification(Services.Interface.TransactionType.RTIbanPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IRequestIBANContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					_journal.TransactionEnded();
					LoadStandByRT();
				}
				else
				{
					SendNotification(Services.Interface.TransactionType.RTIbanPrint, "Remote Telelr", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IRequestIBANContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Printer out of service");
					await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);

						_logger?.Info($"A4 Printer Status : {_statementPrinter.GetPrinterStatus()}");
						_journal.TransactionFailed($"A4 Printer Status : {_statementPrinter.GetPrinterStatus()}");
						_journal.TransactionEnded();

						LoadStandByRT();
					});
				}
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.RTIbanPrint, "Remote Telelr", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IRequestIBANContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, reason: "Could not finish printing");
				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					_journal.TransactionEnded();
					LoadStandByRT();
				});
			}
		}

		public override void Dispose()
		{

		}
	}
}