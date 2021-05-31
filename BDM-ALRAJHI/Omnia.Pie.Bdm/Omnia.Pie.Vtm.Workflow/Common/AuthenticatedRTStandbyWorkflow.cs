namespace Omnia.Pie.Vtm.Workflow.Common
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Authentication;
	using Omnia.Pie.Vtm.Workflow.Authentication.Cif;
	using Omnia.Pie.Vtm.Workflow.BalanceEnquiry;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.Account;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard;
    using Omnia.Pie.Vtm.Workflow.CashWithdrawal.CreditCard;
    using Omnia.Pie.Vtm.Workflow.CashWithdrawal.DebitCard;
	using Omnia.Pie.Vtm.Workflow.ChequePrinting;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestIBAN;
	using Omnia.Pie.Vtm.Workflow.RequestLC;
	using Omnia.Pie.Vtm.Workflow.RequestNLC;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Threading;

	internal class AuthenticatedRTStandbyWorkflow : Workflow
	{
		private DispatcherTimer Timer { get; set; }
		private readonly IESpaceTerminalCommunication _communicator;
		private TaskCompletionSource<bool> _completion;
		private readonly ICardReader _cardReader;

        public AuthenticatedRTStandbyWorkflow(IResolver container) : base(container)
		{
			_completion = new TaskCompletionSource<bool>();

			_communicator = _container.Resolve<IESpaceTerminalCommunication>();
			_cardReader = _container.Resolve<ICardReader>();
			_communicator.RTMessageReceived += _communicator_RTMessageReceivedAsync;
			_communicator.CallEnded += _communicator_CallEnded;

			Timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 10000) };
			Timer.Tick += Timer_Tick;
			Timer.Start();
            

        }

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (!_container.Resolve<ISessionContext>().InCall)
			{
				LoadMainScreen();
				Timer.Stop();
			}
		}

		private void _communicator_CallEnded(object sender, CallEventArgs e)
		{
			_cardReader.CancelReadCard();
			_completion.TrySetResult(true);
		}

		private async void _communicator_RTMessageReceivedAsync(object sender, RTMessageEventArgs e)
		{
			if (e.MessageCode.Code == (int)StatusEnum.CashWithdrawal)
			{

                if (_container.Resolve<ISessionContext>()?.CardUsed.CardType == CardType.CreditCard)
                {
                    using (var _cashWithdrawal = _container.Resolve<CashWithdrawalCreditCardWorkflowRT>())
                    {
                        await _cashWithdrawal.ExecuteAsync();
                    }
                }
                else
                {
                    using (var _cashWithdrawal = _container.Resolve<CashWithdrawalWorkflowRT>())
                    {
                        await _cashWithdrawal.ExecuteAsync();
                    }
                }
				
				_journal.TransactionEnded();
			}
            else if (e.MessageCode.Code == (int)StatusEnum.CashWithdrawalCreditCard)
            {
                using (var _cashWithdrawal = _container.Resolve<CashWithdrawalCreditCardWorkflowRT>())
                {
                    await _cashWithdrawal.ExecuteAsync();
                }
                _journal.TransactionEnded();
            }
            else if (e.MessageCode.Code == (int)StatusEnum.AuthenticateDebitCreditCard)
			{
				_container.Resolve<AuthenticationWorkflowRT>().Execute();
			}
			else if (e.MessageCode.Code == (int)StatusEnum.CashDepositAccount)
			{
				using (var _cashDeposit = _container.Resolve<CashDepositToAccountWorkflowRT>())
				{
					await _cashDeposit.ExecuteAsync();
				}

				_journal.TransactionEnded();
			}
			else if (e.MessageCode.Code == (int)StatusEnum.CreditCardPaymentWithCash)
			{
				using (var _cashDeposit = _container.Resolve<CashDepositToCreditCardWorkflowRT>())
				{
					await _cashDeposit.ExecuteAsync();
				}

				_journal.TransactionEnded();
			}
			else if (e.MessageCode.Code == (int)StatusEnum.AuthenticateCif)
			{
				using (var flow = _container.Resolve<CifAuthenticationWorkflowRT>())
				{
					flow.Execute();
				}
			}
			else if (e.MessageCode.Code == (int)StatusEnum.AccountInquiryAuthenticateCard)
			{
				_navigator.RequestNavigationTo<IAuthenticatedEnquiryViewModel>((vm) =>
				{
					vm.CancelVisibility = true;
					vm.CancelAction = () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						_navigator.RequestNavigationTo<IAuthenticatedRTStandbyViewModel>();
					};
					vm.StatementPrintAction = () =>
					{
						var flow = _container.Resolve<StatementPrintingWorkFlowRT>();
						flow.Execute();
					};
					vm.BalanceEnquiryAction = () =>
					{
						var flow = _container.Resolve<BalanceEnquiryWorkFlowRT>();
						flow.Execute();
					};
				});
			}
			else if (e.MessageCode.Code == (int)StatusEnum.Documents)
			{
				_navigator.RequestNavigationTo<IAuthenticatedDocumentsViewModel>((vm) =>
				{
					vm.CancelVisibility = true;
					vm.CancelAction = () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						_navigator.RequestNavigationTo<IAuthenticatedRTStandbyViewModel>();
					};
					vm.LCRequestAction = () =>
					{
						var flow = _container.Resolve<RequestLCWorkFlowcsRT>();
						flow.Execute();
					};
					vm.NLCRequestAction = () =>
					{
						var flow = _container.Resolve<RequestNLCWorkFlowRT>();
						flow.Execute();
					};
					vm.IBANLetterAction = () =>
					{
						var flow = _container.Resolve<RequestIBANWorkFlowRT>();
						flow.Execute();
					};
				});
			}
			else if (e.MessageCode.Code == (int)StatusEnum.ChequePrintingStart)
			{
				using (var flow = _container.Resolve<ChequePrintingWorkflowRT>())
				{
					await flow.Execute();
				}
			}
			else if (e.MessageCode.Code == (int)StatusEnum.ChequeDepositAccount)
			{
				using (var _chequeDeposit = _container.Resolve<ChequeDeposit.ChequeDepositToAccountWorkflowRT>())
				{
					await _chequeDeposit.ExecuteAsync();
				}

				_journal.TransactionEnded();
			}
			else if (e.MessageCode.Code == (int)StatusEnum.HoldCall)
			{
				_videoService.OnHold = true;
			}
			else if (e.MessageCode.Code == (int)StatusEnum.UnholdCall)
			{
				_videoService.OnHold = false;
			}
		}

		public async Task Execute()
		{
            _logger?.Info($"Execute Workflow: Authenticated - RT Assisted");

            Timer.Stop();
			Timer.Start();
			_navigator.RequestNavigationTo<IAuthenticatedRTStandbyViewModel>();
			_communicator.SendStatus(StatusEnum.StartSession);
			await _completion.Task;
		}

		public override void Dispose()
		{
			//_communicator.CallEnded -= _communicator_CallEnded;
			//_communicator.RTMessageReceived -= _communicator_RTMessageReceivedAsync;
			GC.SuppressFinalize(this);
		}
	}
}