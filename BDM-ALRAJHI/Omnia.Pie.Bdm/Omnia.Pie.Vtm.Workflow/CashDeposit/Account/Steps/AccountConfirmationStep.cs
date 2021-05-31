
namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	using System;
	using System.Threading.Tasks;
	using Framework.Interface;
	using CashDeposit.Context;
	using Bootstrapper.Interface.ViewModels.CashDeposit;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.Threading;

	public class AccountConfirmationStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _taskSource;
		public Action _accountDetailsConfirmedAction;
		public AccountConfirmationStep(IResolver container) : base(container)
		{
		}

		public async Task<bool> ConfirmAccountDetails()
		{
            _logger?.Info($"Execute Step: Confirm Account Details");

            var cancellationToken = new CancellationTokenSource();
			try
			{
				_taskSource = new TaskCompletionSource<bool>();
				SetCurrentStep(Properties.Resources.StepConfirmation);

				var _context = Context.Get<ICashDepositContext>();
				var vm = _container.Resolve<IAccountConfirmationViewModel>();
				vm.SelectedAccount = _context.SelectedAccount;
				vm.DefaultVisibility = true;
				vm.DefaultAction = () =>
				{
					try
					{
						LoadWaitScreen();

						cancellationToken?.Cancel();
						cancellationToken = null;

						_accountDetailsConfirmedAction();
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

						_accountDetailsConfirmedAction();
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);

						cancellationToken?.Cancel();
						cancellationToken = null;

						_taskSource.SetException(new InvalidOperationException());
					}
				};

				vm.BackVisibility = true;
				vm.BackAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					BackAction();
					_taskSource.SetResult(true);
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
