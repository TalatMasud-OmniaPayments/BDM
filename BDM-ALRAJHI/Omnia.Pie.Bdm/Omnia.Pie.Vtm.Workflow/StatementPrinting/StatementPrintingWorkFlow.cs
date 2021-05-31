using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Configurations;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.Common.Steps;
using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
using Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps;
using System;
using System.Timers;
using System.Linq;

namespace Omnia.Pie.Vtm.Workflow.StatementPrinting
{
	internal class StatementPrintingWorkFlow : Workflow
	{
		//private readonly IStatementPrinter _statementPrinter;

        //private readonly LoadCustomerDetailStep _loadCustomerDetailStep;
        private readonly IReceiptFormatter _receiptFormatter;
        private readonly PrintReceiptStep _printReceiptStep;
        private readonly LoadAccountDetailStep _loadAccountDetailStep;
		//private readonly GetChargesStep _getCharges;
		private readonly StatementPrintingStep _statementPrintingStep;
		private readonly ConfirmationStep _confirmationStep;
		private readonly LoadAccountStep _loadAccountStep;
		private readonly AccountSelectionStep _accountSelectionStep;
		private readonly GetTransactionHistoryStep _getTransactionHistoryStep;
		private readonly ChargesDeductionStep _chargesDeductionStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
        //public IDataContext businessContext { get; set; }
        //private Timer _executeTimer;
        //private bool _allowExecution;

        public StatementPrintingWorkFlow(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.Financial, "Statement Print Request-SS");
			//_statementPrinter = _container.Resolve<IStatementPrinter>();

			//_loadCustomerDetailStep = _container.Resolve<LoadCustomerDetailStep>();
			_loadAccountStep = _container.Resolve<LoadAccountStep>();
			//_getCharges = _container.Resolve<GetChargesStep>();
			_accountSelectionStep = _container.Resolve<AccountSelectionStep>();
			_statementPrintingStep = _container.Resolve<StatementPrintingStep>();
			_confirmationStep = _container.Resolve<ConfirmationStep>();
			_loadAccountDetailStep = _container.Resolve<LoadAccountDetailStep>();
			_getTransactionHistoryStep = _container.Resolve<GetTransactionHistoryStep>();
			_chargesDeductionStep = _container.Resolve<ChargesDeductionStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();
            _receiptFormatter = receiptFormatter;
            _printReceiptStep = _container.Resolve<PrintReceiptStep>();

            Context = _loadAccountDetailStep.Context = _getTransactionHistoryStep.Context = _confirmationStep.Context = _accountSelectionStep.Context = _loadAccountStep.Context = _chargesDeductionStep.Context = _statementPrintingStep.Context = CreateContext(typeof(StatementPrintingContext));
			Context.Get<IStatementPrintingContext>().SelfServiceMode = true;

			AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepChargesConfirmation},{Properties.Resources.StepPrinting}");
			//AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepChargesConfirmation},{Properties.Resources.StepPrinting}");

            //_allowExecution = true;

            _confirmationStep.PrintAction = async (userTransaction) =>
            {
                var transaction = (UserTransaction)userTransaction;
                var _context = Context.Get<IStatementPrintingContext>();
                try
                {
                    LoadWaitScreen();
                    MonitorDeviceStatus();
                    var receiptData = await _receiptFormatter.FormatAsync(new CashDepositToAccountReceipt
                    {
                        AccountNumber = transaction.AccountNo,
                        AuthCode = transaction.TransactionId,
                        UserId = transaction.UserId,
                        TransactionDate = transaction.TransactionDateTime.ToString(),
                        Currency = transaction.CurrencyCode,
                        Denominations = transaction.DenominationList.Select(x => new Framework.Interface.Receipts.Denomination()
                        {
                            Count = x.Count,
                            Value = x.Type
                        })
                        .ToList(),

                        TransactionStatus = TransactionStatus.Succeeded,
                        CustomerName = transaction.UserId
                    }, new ReceiptFormattingOptions() { IsMarkupEnabled = true });
                    _journal.TransactionSucceeded();
                    await _printReceiptStep.PrintReceipt(true, receiptData);

                    _confirmationStep.ExecuteAsync();
                    FinishTransaciton();
                }
                catch (Exception ex)
                {
                    _journal.TransactionFailed(ex.GetBaseException().Message);
                    _logger.Exception(ex);
                    await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt);
                    //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountDebitCard, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
                    //_workflowCompletionTask.SetResult(false);
                }
            };
                _confirmationStep.DefaultAction = async () =>
            {
                _logger.Info($"Print Statement selected.");

                await _chargesDeductionStep.DeductCharges();

                try
                {
                    //_statementPrinter.TurnOnGuideLights();
                    await _statementPrintingStep.Execute();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //_statementPrinter.TurnOffGuideLights();
                }
                SendNotification(Services.Interface.TransactionType.SSStatementPrint, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Successful, "", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
                FinishTransaciton();
            };

            _accountSelectionStep.DefaultAction = async () =>
			{

                try
				{
					await _getTransactionHistoryStep.GetTransactionHistory();
					await _loadAccountDetailStep.GetAccountDetail();
                    Context.Get<IStatementPrintingContext>().StatementCharges = new GetStatementChargesResult
                    {
                        ChargeAmount = "0",
                        ChargeCurrency = TerminalConfiguration.Section?.Currency
                    };
                    //await _getCharges.GetCharges();
                    _confirmationStep.ExecuteAsync();

                }
				catch (Exception ex)
				{
					_logger.Exception(ex);
					_journal.TransactionFailed(ex.GetBaseException().Message);
					SendNotification(Services.Interface.TransactionType.SSStatementPrint, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "Could not finish the request", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");
					await LoadErrorScreenAsync(ErrorType.NotAvailableService, () => { FinishTransaciton(); });
                }
            };
  

            _confirmationStep.CancelAction = _checkReceiptPrinterStep.CancelAction = _accountSelectionStep.CancelAction = _printReceiptStep.CancelAction = () =>
			 {
                 _logger.Info($"Step Cancelled. Cancel Button pressed.");
                 _journal.TransactionCanceled();
				 LoadSelfServiceMenu();
			 };
			_confirmationStep.BackAction = () =>
			{
                _logger.Info($"Screen Cancelled. Back Button pressed.");
                _accountSelectionStep.ExecuteAsync();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Statement Printing - Self Service");

            try
			{
                //var pprStatus = _statementPrinter.GetPaperStatus();
                //var lowerpprStatus = _statementPrinter.GetChequePaperStatus();

                //_logger?.Info($"A4 Printer - Statement Paper status : {pprStatus}");
                //_logger?.Info($"A4 Printer - Cheque Paper status : {lowerpprStatus}");
                _journal.TransactionStarted(EJTransactionType.NonFinancial, "Statement Printing");

                if (await _checkReceiptPrinterStep.CheckPrinterAsync()) {
                        await _loadAccountStep.GetAccounts();

                        _accountSelectionStep.ExecuteAsync();
                    }

			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				SendNotification(Services.Interface.TransactionType.SSStatementPrint, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IStatementPrintingContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, Services.Interface.Enums.TransactionStatus.Failure, "", Context.Get<IStatementPrintingContext>()?.NumberofMonths, "", "");

				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
				{
					FinishTransaciton();
				});
			}
		}

        

        public override void Dispose()
		{
            //_executeTimer = null;
        }
	}
}