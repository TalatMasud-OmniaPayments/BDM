namespace Omnia.Pie.Vtm.Workflow.MainMenu.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.Account;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class ServiceTypeSelectionStep : WorkflowStep
	{
		private readonly ICallTellerViewModel _callTellerViewModel;
		public Action ProductInfoAction { get; set; }

		public ServiceTypeSelectionStep(IResolver container) : base(container)
		{
			_callTellerViewModel = _container.Resolve<ICallTellerViewModel>();
		}

		public void Execute()
		{
            _logger?.Info($"Execute Step: Select Service Type");

            _navigator.RequestNavigationTo<IServiceTypeSelectionViewModel>((viewModel) =>
			{
				viewModel.BackVisibility = viewModel.CancelVisibility = true;
				viewModel.TellerAssistanceVisibility = true;
				viewModel.TellerAssistedAction = (x) =>
				{
					_navigator.RequestNavigationTo<ICallRecordingConfirmationViewModel>((vm) =>
					{
						vm.YesAction = async () =>
						{
							var tokenSource = new CancellationTokenSource();
							_navigator.RequestNavigationTo<IAnimationViewModel>((vmm) =>
							{
								vmm.Type(AnimationType.CallingForAssistance);
								vmm.CancelVisibility = true;

								vmm.CancelAction = () =>
								{
									_callTellerViewModel.CancelCall(tokenSource);
									Execute();
								};
							});

							await Task.Delay(1000);
							var result = await CallTellerAction(x, tokenSource);
							switch (result)
							{
								case CallResult.NetWorkError:
								case CallResult.LoginFailed:
									{
										await LoadErrorScreenAsync(ErrorType.FailedRequest, () =>
										{
											Execute();
										});

										break;
									}
								case CallResult.NoTellerLoggedIn:
									{
										if (IsWorkingHours())
										{
											await LoadErrorScreenAsync(ErrorType.NoTellerAvailable, () =>
											{
												Execute();
											});
										}
										else
										{
											await LoadErrorScreenAsync(ErrorType.NoTellerLoggedIn, () =>
											{
												Execute();
											});
										}

										break;
									}
								case CallResult.NoTellerAvailable:
									{
										await LoadErrorScreenAsync(ErrorType.NoTellerAvailable, () =>
										{
											Execute();
										});

										break;
									}
								case CallResult.Success:
									{
										if (GetCallMod())
										{
											Execute();
										}
										break;
									}
							}
						};
						vm.NoAction = vm.ExpiredAction = () =>
						{
							Execute();
						};

						vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
					});
				};

				viewModel.BankingServicesAction = () =>
				{
					using (var flow = _container.Resolve<Authentication.AuthenticationWorkflow>())
					{
						flow.Execute();
					}
				};

				viewModel.ProductInfoAction = () =>
				{
					ProductInfoAction?.Invoke();
				};

				viewModel.BackAction = () =>
				{
					BackAction?.Invoke();
				};

				viewModel.CancelAction = () =>
				{
					CancelAction?.Invoke();
				};

				viewModel.CardlessDepositAction = () =>
				{
					_navigator.RequestNavigationTo<ICardlessDepositViewModel>((vm) =>
					{
						vm.BackVisibility = vm.CancelVisibility = true;
						vm.CancelAction = () =>
						{
							LoadMainScreen();
						};
						vm.BackAction = () =>
						{
							Execute();
						};
						vm.CashDepositAction = async () =>
						{
							try
							{
								using (var _flow = _container.Resolve<CashDepositToAccountCardLessWorkflow>())
								{
									await _flow.ExecuteAsync();
								}
								_journal.TransactionEnded();
							}
							catch (Exception ex)
							{
								_logger.Exception(ex);
							}
							finally
							{
								LoadMainScreen();
							}
						};

						vm.ChequeDepositAction = async () =>
						{
							// Temparory, needs to be removed later on.
							using (var _flow = _container.Resolve<ChequeDeposit.ChequeDepositToAccountCardlessWorkflow>())
							{
								await _flow.ExecuteAsync();
							}
							LoadMainScreen();
						};
					});
				};
			});
		}

		public override void Dispose()
		{

		}
	}
}