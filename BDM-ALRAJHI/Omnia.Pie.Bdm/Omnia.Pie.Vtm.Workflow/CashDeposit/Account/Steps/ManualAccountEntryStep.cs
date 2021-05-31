namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal class ManualAccountEntryStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _taskSource;
		public Action _accountSelectedAction;

		public ManualAccountEntryStep(IResolver container) : base(container)
		{
		}

		public async Task<bool> EnterAccountNumber()
		{
            _logger?.Info($"Execute Step: Account Number Entry");

            var cancellationToken = new CancellationTokenSource();
			_taskSource = new TaskCompletionSource<bool>();
			try
			{
				var _context = Context.Get<ICashDepositContext>();

				var vm = _container.Resolve<IManualAccountEntryViewModel>();

				vm.DefaultVisibility = true;
				vm.DefaultAction = async () =>
				{
					try
					{
						LoadWaitScreen();
						var _customerService = _container.Resolve<ICustomerService>();
						var _accountDetails = await _customerService.GetAccountDetail(vm.AccountNumber);
						_context.SelectedAccount = new Services.Interface.Entities.Account()
						{
							Number = _accountDetails.AccountNumber,         //vm.AccountNumber,
							Type = _accountDetails.AccountType,             //"Savings Account",
							Name = _accountDetails.AccountTitle,            //"John Doe"
							Currency = _accountDetails.AccountCurrency      //"AED"
						};

						cancellationToken?.Cancel();
						cancellationToken = null;

						_accountSelectedAction();

					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
						await LoadErrorScreenAsync(ErrorType.InvalidAccountNumber, false);
						_navigator.RequestNavigation(vm);
					}
				};

				vm.CancelVisibility = true;
				vm.CancelAction = () =>
				{
					try
					{
						_context.IsCanceled = true;

						cancellationToken?.Cancel();
						cancellationToken = null;

						_accountSelectedAction();
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);

						cancellationToken?.Cancel();
						cancellationToken = null;

						_taskSource.SetException(new InvalidOperationException());
						throw ex;
					}
				};

				vm.BackVisibility = true;
				vm.BackAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					BackAction();
				};

				if (_context.SelfService)
				{

					vm.StartUserActivityTimer(cancellationToken.Token);
					vm.ExpiredAction = () =>
					{
						_navigator.Push<IActiveConfirmationViewModel>((viewmod) =>
						{
							viewmod.StartTimer(new TimeSpan(0, 0, InactivityTimer));
							viewmod.YesAction = () =>
							{
								vm.StartUserActivityTimer(cancellationToken.Token);
								_navigator.Pop();
							};
							viewmod.NoAction = viewmod.ExpiredAction = () =>
							{
								cancellationToken?.Cancel();
								cancellationToken = null;

								CancelAction();
								_taskSource.SetResult(true);
							};
						});
					};
				}

				_navigator.Push(vm);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);

				cancellationToken?.Cancel();
				cancellationToken = null;

				_taskSource.SetException(new InvalidOperationException());
			}

			return await _taskSource.Task;
		}

		public override void Dispose()
		{

		}
	}
}
