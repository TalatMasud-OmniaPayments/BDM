namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using System;
	using System.Linq;

	public class BalanceEnquiryWorkFlowRT : Workflow
	{
		private readonly LoadAccountDetailStep _loadAccountDetailStep;
		private readonly LoadAccountStep _loadAccountStep;
		private readonly ConfirmationBalanceEnquiryStep _confirmationBalanceEnquiryStep;
		private readonly GetTransactionNoStep _getTransactionNoStep;
		private readonly PrintingStep _printingStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;

		private readonly IESpaceTerminalCommunication _communicator;

		public BalanceEnquiryWorkFlowRT(IResolver container) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.Financial, "Balance Inquiry-RT");

			_communicator = _container.Resolve<IESpaceTerminalCommunication>();
			_loadAccountStep = _container.Resolve<LoadAccountStep>();
			_confirmationBalanceEnquiryStep = _container.Resolve<ConfirmationBalanceEnquiryStep>();
			_loadAccountDetailStep = _container.Resolve<LoadAccountDetailStep>();
			_getTransactionNoStep = _container.Resolve<GetTransactionNoStep>();
			_printingStep = _container.Resolve<PrintingStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _loadAccountStep.Context = _confirmationBalanceEnquiryStep.Context =
				_loadAccountDetailStep.Context = _printingStep.Context = _getTransactionNoStep.Context = CreateContext(typeof(BalanceEnquiryContext));

			_checkReceiptPrinterStep.CancelAction = _confirmationBalanceEnquiryStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);
				LoadStandByRT();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Balance Enquiry - RT Assisted");

            try
			{
				await _sendRTStatusStep.ExecuteAsync();

				await _checkReceiptPrinterStep.CheckPrinterAsync();
				await _loadAccountStep?.GetAccounts();

				_navigator.RequestNavigationTo<IBalanceEnquiryViewModel>((viewModel) =>
				{
					viewModel.Accounts = Context.Get<IBalanceEnquiryContext>()?.Accounts;
					viewModel.SelectedAccount = Context.Get<IBalanceEnquiryContext>()?.Accounts?.FirstOrDefault();

					viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
					viewModel.CancelAction = () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
					};
					viewModel.DefaultAction = async () =>
					{
						Context.Get<IBalanceEnquiryContext>().SelectedAccount = viewModel?.SelectedAccount;

						_journal.AccountSelected(viewModel.SelectedAccount.Number);

						await _loadAccountDetailStep?.GetAccountDetail();
						await _confirmationBalanceEnquiryStep?.Execute();
						await _getTransactionNoStep.GetTSNno();
						await _printingStep.ExecuteAsync();
						SendNotification(Services.Interface.TransactionType.RTBalanceInquiry, "Remote Teller", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
					};
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					SendNotification(Services.Interface.TransactionType.SSBalanceInquiry, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Transaction Failed");
					LoadStandByRT();
				});
			}
		}

		public override void Dispose()
		{

		}
	}
}