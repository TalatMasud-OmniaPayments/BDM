namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class CashDepositAccountSelectionStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _taskSource;
		public Action _accountSelectedAction;

		public CashDepositAccountSelectionStep(IResolver container) : base(container)
		{
		}

		public async Task<bool> SelectAccountAsync()
		{
            _logger?.Info($"Execute Step: Select Account");

            var cancellationToken = new CancellationTokenSource();
			_taskSource = new TaskCompletionSource<bool>();
			SetCurrentStep(Properties.Resources.StepAccountSelection);
			LoadWaitScreen();

			var _context = Context.Get<ICashDepositContext>();
			var vm = _container.Resolve<ICashDepositAccountSelectionViewModel>();
			vm.Accounts = _context.Accounts;
			vm.SelectedAccount = _context.Accounts[0];
			vm.ManualAccountEntryAction = () =>
			{
				try
				{
					_context.ManualAccount = true;
					LoadWaitScreen();

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetResult(true);
					_accountSelectedAction();
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new InvalidOperationException());
				}
			};

			vm.DefaultVisibility = true;
			vm.DefaultAction = async () =>
			{
				try
				{
					_context.SelectedAccount = vm.SelectedAccount;
					if (!await Validate(vm))
					{
						return;
					}

					LoadWaitScreen();
					_journal.AccountSelected(_context.SelectedAccount.Number);
					_context.ManualAccount = false;

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetResult(true);
					_accountSelectedAction();
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new InvalidOperationException());
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
					_taskSource.SetResult(true);
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new InvalidOperationException());
				}
			};
			if (_context.SelfService)
			{

				vm.StartUserActivityTimer(cancellationToken.Token);
				vm.ExpiredAction = () =>
				{
					_navigator.Push<IActiveConfirmationViewModel>((viewmodel) =>
					{
						viewmodel.StartTimer(new TimeSpan(0, 0, InactivityTimer));
						viewmodel.YesAction = () =>
						{
							vm.StartUserActivityTimer(cancellationToken.Token);
							_navigator.Pop();
						};
						viewmodel.NoAction = viewmodel.ExpiredAction = () =>
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

			return await _taskSource.Task;
		}

		private async Task<bool> Validate(ICashDepositAccountSelectionViewModel vm)
		{
            _logger?.Info($"Execute Task: Validate Account");

            if (vm.SelectedAccount == null)
			{
				await LoadErrorScreenAsync(ErrorType.InvalidAccount);
				_navigator.RequestNavigation(vm);
				return false;
			}

			return true;
		}

		public override void Dispose()
		{

		}
	}
}