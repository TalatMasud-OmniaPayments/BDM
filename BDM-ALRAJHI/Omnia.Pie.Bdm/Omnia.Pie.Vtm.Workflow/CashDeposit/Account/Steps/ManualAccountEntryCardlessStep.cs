using System;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	using Framework.Interface;
	using CashDeposit.Context;
	using Bootstrapper.Interface.ViewModels.CashDeposit;
	using Omnia.Pie.Vtm.Services.Interface;
	using System.Threading;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using System.Configuration;

    internal class ManualAccountEntryCardlessStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _taskSource;
		public Action<bool> _accountEnteredAction;

		public ManualAccountEntryCardlessStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> EnterAccountNumberAsync(bool hasBack = true)
		{
            _logger?.Info($"Execute Step: Account Number Entry");

            _taskSource = new TaskCompletionSource<bool>();
			var cancellationToken = new CancellationTokenSource();
			var _context = Context.Get<ICashDepositContext>();
			var vm = _container.Resolve<IManualAccountEntryViewModel>();
			vm.DefaultVisibility = true;

			vm.DefaultAction = async () =>
			{
				try
				{
                    int accountLength = Int32.Parse(ConfigurationManager.AppSettings["AccountLength"].ToString());
                    if (vm.AccountNumber.Length < accountLength)
                    {
                        throw new Exception("Invalid Account Length Entered.");
                    }
                    
                    LoadWaitScreen();
					var _customerService = _container.Resolve<ICustomerService>();
					var _accountDetails = await _customerService.GetAccountDetail(vm.AccountNumber);
					_context.SelectedAccount = new Services.Interface.Entities.Account()
					{
						Number = _accountDetails.AccountNumber,
						Type = _accountDetails.AccountType,
						Name = _accountDetails.AccountTitle,
						Currency = _accountDetails.AccountCurrency
					};

					cancellationToken?.Cancel();
					cancellationToken = null;

					_accountEnteredAction(true);
					_taskSource.TrySetResult(true);
				}
				catch (Exception ex)
				{
					await LoadErrorScreenAsync(ErrorType.InvalidAccountNumber, false);
					_navigator.RequestNavigation(vm);
					_logger.Exception(ex);
				}
			};

			vm.CancelVisibility = true;
			vm.CancelAction = () =>
			{
				_context.IsCanceled = true;

				cancellationToken?.Cancel();
				cancellationToken = null;

				_accountEnteredAction(true);
			};

			vm.BackVisibility = hasBack;
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

			return await _taskSource.Task;
		}

		public override void Dispose()
		{

		}
	}
}
