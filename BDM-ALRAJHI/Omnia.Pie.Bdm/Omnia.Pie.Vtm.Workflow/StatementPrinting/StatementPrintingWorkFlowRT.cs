namespace Omnia.Pie.Vtm.Workflow.StatementPrinting
{
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps;
	using System;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System.Timers;

    internal class StatementPrintingWorkFlowRT : Workflow
	{
		private readonly IESpaceTerminalCommunication _communicator;
		private readonly IStatementPrinter _statementPrinter;

		private readonly LoadCustomerDetailStep _loadCustomerDetailStep;
		private readonly LoadAccountDetailStep _loadAccountDetailStep;
		private readonly GetChargesStep _getCharges;
		private readonly StatementPrintingStep _statementPrintingStep;
		private readonly LoadAccountStep _loadAccountStep;
		private readonly ConfirmationStep _confirmationStep;
		private readonly AccountSelectionStep _accountSelectionStep;
		private readonly GetTransactionHistoryStep _getTransactionHistoryStep;
		private readonly ChargesDeductionStep _chargesDeductionStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
        private Timer _executeTimer;
        private bool _allowExecution;

        public StatementPrintingWorkFlowRT(IResolver container) : base(container)
		{
			_communicator = _container.Resolve<IESpaceTerminalCommunication>();
			_statementPrinter = _container.Resolve<IStatementPrinter>();

			_journal.TransactionStarted(EJTransactionType.Financial, "Statement Print Request-RT");

			_loadCustomerDetailStep = _container.Resolve<LoadCustomerDetailStep>();
			_loadAccountStep = _container.Resolve<LoadAccountStep>();
			_getCharges = _container.Resolve<GetChargesStep>();
			_accountSelectionStep = _container.Resolve<AccountSelectionStep>();
			_statementPrintingStep = _container.Resolve<StatementPrintingStep>();
			_confirmationStep = _container.Resolve<ConfirmationStep>();
			_loadAccountDetailStep = _container.Resolve<LoadAccountDetailStep>();
			_getTransactionHistoryStep = _container.Resolve<GetTransactionHistoryStep>();
			_chargesDeductionStep = _container.Resolve<ChargesDeductionStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _loadAccountDetailStep.Context = _loadCustomerDetailStep.Context =
				_getTransactionHistoryStep.Context = _confirmationStep.Context = _accountSelectionStep.Context =
				_loadAccountStep.Context = _getCharges.Context = _chargesDeductionStep.Context =
				_statementPrintingStep.Context = CreateContext(typeof(StatementPrintingContext));

			AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepChargesConfirmation},{Properties.Resources.StepPrinting}");

            _allowExecution = true;

            _accountSelectionStep.DefaultAction = async () =>
			{
                if (_allowExecution)
                {
                    ActivateExecuteTimer();
                    _allowExecution = false;
                }
                else
                {
                    _logger.Info("Statement Workflow has been invoked again in less than 10 secs");
                    return;
                }

                try
				{
					await _getTransactionHistoryStep.GetTransactionHistory();
					await _loadAccountDetailStep.GetAccountDetail();
					await _getCharges.GetCharges();
					await _confirmationStep.Execute();
					await _chargesDeductionStep.DeductCharges();

					try
					{
						_statementPrinter.TurnOnGuideLights();
						await _statementPrintingStep.Execute();
					}
					catch (Exception e)
					{
						throw e;
					}
					finally
					{
						_statementPrinter.TurnOffGuideLights();
					}

					SendNotification(Services.Interface.TransactionType.RTStatementPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>().SelectedAccount.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Successful, "", Context.Get<IStatementPrintingContext>().NumberofMonths, "", "");

					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.RTStatementPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "Could not finish the request", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
					await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
					});
				}

                DisposeExecutionTimer();
            };
			_confirmationStep.CancelAction = _checkReceiptPrinterStep.CancelAction = _accountSelectionStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);
				SendNotification(Services.Interface.TransactionType.RTStatementPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
				LoadStandByRT();
                DisposeExecutionTimer();
            };
			_confirmationStep.BackAction = () =>
			{
                DisposeExecutionTimer();
                _accountSelectionStep.ExecuteAsync();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Statement Printing - RT Assisted");

            var pprStatus = _statementPrinter.GetPaperStatus();
            var lowerpprStatus = _statementPrinter.GetChequePaperStatus();

            _logger?.Info($"A4 Printer - Statement Paper status : {pprStatus}");
            _logger?.Info($"A4 Printer - Cheque Paper status : {lowerpprStatus}");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

				if (_statementPrinter.Status == DeviceStatus.Online && _statementPrinter.GetPrinterStatus() == PrinterStatus.Present)
				{
					await _checkReceiptPrinterStep.CheckPrinterAsync();
					await _loadAccountStep.GetAccounts();
					await _loadCustomerDetailStep.GetCustomerDetail();
					_accountSelectionStep.ExecuteAsync();
				}
				else
				{
					SendNotification(Services.Interface.TransactionType.RTStatementPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
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
				SendNotification(Services.Interface.TransactionType.RTStatementPrint, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				});
			}
		}

        private void ActivateExecuteTimer()
        {
            if (_executeTimer == null)
            {
                _executeTimer = new Timer();
                _executeTimer.Elapsed += new ElapsedEventHandler(IncrementExecutionTime);
                _executeTimer.Interval = (double)10000; // 10 sec
                _executeTimer.AutoReset = false;
            }
            _executeTimer.Start();
            _logger.Info($"Single execution timer started.");
        }

        private void IncrementExecutionTime(object sender, EventArgs e)
        {
            _allowExecution = true;
            _logger.Info($"Single execution timer reached.");
            _executeTimer.Stop();
        }

        private void DisposeExecutionTimer()
        {
            if (_executeTimer != null)
            {
                _allowExecution = true;
                _executeTimer.Enabled = false;
                _executeTimer.Close();
                _executeTimer = null;
            }

        }

        public override void Dispose()
		{
            _executeTimer = null;
            // Need to unsubscribe the events that we subscribe in this class
        }
	}
}