namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Steps;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using System;
	using System.Linq;
	using System.Threading;

	public class BalanceEnquiryWorkFlow : Workflow
	{
		private readonly GetTransactionNoStep _getTransactionNoStep;
		private readonly LoadAccountStep _loadAccountStep;
		private readonly LoadAccountDetailStep _loadAccountDetailStep;
		private readonly ConfirmationBalanceEnquiryStep _confirmationBalanceEnquiryStep;
		private readonly PrintingStep _printingStep;
		private readonly CheckReceiptPrinterStep _checkReceiptPrinterStep;
		CancellationTokenSource cancellationToken;

		public BalanceEnquiryWorkFlow(IResolver container) : base(container)
		{
			_journal.TransactionStarted(EJTransactionType.Financial, "Balance Inquiry-SS");

			cancellationToken = new CancellationTokenSource();
			_loadAccountStep = _container.Resolve<LoadAccountStep>();
			_confirmationBalanceEnquiryStep = _container.Resolve<ConfirmationBalanceEnquiryStep>();
			_loadAccountDetailStep = _container.Resolve<LoadAccountDetailStep>();
			_getTransactionNoStep = _container.Resolve<GetTransactionNoStep>();
			_printingStep = _container.Resolve<PrintingStep>();
			_checkReceiptPrinterStep = _container.Resolve<CheckReceiptPrinterStep>();

			Context = _getTransactionNoStep.Context = _loadAccountStep.Context = _loadAccountDetailStep.Context =
				_confirmationBalanceEnquiryStep.Context = _printingStep.Context = CreateContext(typeof(BalanceEnquiryContext));

			AddSteps($"{Properties.Resources.StepAccountSelection},{Properties.Resources.StepConfirmation},{Properties.Resources.StepPrinting}");

			_checkReceiptPrinterStep.CancelAction =_confirmationBalanceEnquiryStep.ExpiredAction = _confirmationBalanceEnquiryStep.CancelAction = () =>
			{
				cancellationToken?.Cancel();
				cancellationToken = null;

				_journal.TransactionCanceled();
				LoadSelfServiceMenu();
			};

			Context.Get<IBalanceEnquiryContext>().SelfServiceMode = true;
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Balance Enquiry - Self Service");

            try
			{
				await _checkReceiptPrinterStep.CheckPrinterAsync();
				await _loadAccountStep.GetAccounts();
				

				_navigator.Push<IBalanceEnquiryViewModel>((viewModel) =>
				{
					viewModel.Accounts = Context.Get<IBalanceEnquiryContext>()?.Accounts;
					viewModel.SelectedAccount = Context.Get<IBalanceEnquiryContext>()?.Accounts?.FirstOrDefault();

					viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

					viewModel.CancelAction = () =>
					{
						cancellationToken?.Cancel();
						cancellationToken = null;

						LoadSelfServiceMenu();
					};
					viewModel.DefaultAction = async () =>
					{
						LoadWaitScreen();

						Context.Get<IBalanceEnquiryContext>().SelectedAccount = viewModel.SelectedAccount;
						_journal.AccountSelected(viewModel.SelectedAccount.Number);

						await _loadAccountDetailStep.GetAccountDetail();
						await _confirmationBalanceEnquiryStep.Execute();
						await _getTransactionNoStep.GetTSNno();
						await _printingStep.ExecuteAsync();

						cancellationToken?.Cancel();
						cancellationToken = null;

						SendNotification(Services.Interface.TransactionType.SSBalanceInquiry, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier);
						FinishTransaciton();
					};
					
					if (Context.Get<IBalanceEnquiryContext>().SelfServiceMode)
					{
						viewModel.StartUserActivityTimer(cancellationToken.Token);
						viewModel.ExpiredAction = () =>
						{
							_navigator.Push<IActiveConfirmationViewModel>((vm) =>
							{
								vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
								vm.YesAction = () =>
								{
									viewModel.StartUserActivityTimer(cancellationToken.Token);
									_navigator.Pop();
								};
								vm.NoAction = vm.ExpiredAction = () =>
								{
									cancellationToken?.Cancel();
									cancellationToken = null;

									LoadSelfServiceMenu();
								};
							});
						};
					}
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_journal.TransactionFailed(ex.GetBaseException().Message);
				await LoadErrorScreenAsync(ErrorType.NotAvailableService, () => { LoadSelfServiceMenu(); });
				SendNotification(Services.Interface.TransactionType.SSBalanceInquiry, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<IBalanceEnquiryContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Transaction Failed");
			}
		}

		public override void Dispose()
		{

		}
	}
}