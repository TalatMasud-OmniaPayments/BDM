namespace Omnia.Pie.Vtm.Workflow.CashDeposit.CreditCard.Steps
{
	using System;
	using System.Threading.Tasks;
	using Framework.Interface;
	using CashDeposit.Context;
	using Bootstrapper.Interface.ViewModels.CashDeposit;
	using System.Threading;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;

	public class CreditCardDetailsStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _taskSource;
		public Action _cardDetailsConfirmedAction;

		public CreditCardDetailsStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> ConfirmCardDetails()
		{
            _logger?.Info($"Execute Step: Get and Confirm Credit Card Details");

            var cancellationToken = new CancellationTokenSource();

			try
			{
				_taskSource = new TaskCompletionSource<bool>();
				SetCurrentStep(Properties.Resources.StepCreditCardDetails);

				var _context = Context.Get<ICashDepositContext>();
				var vm = _container.Resolve<ICreditCardDetailsViewModel>();
				vm.CardUsed = _context.CardUsed;
				vm.DefaultVisibility = true;
				vm.DefaultAction = () =>
				{
					try
					{
						LoadWaitScreen();

						cancellationToken?.Cancel();
						cancellationToken = null;

						_cardDetailsConfirmedAction();
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

						_cardDetailsConfirmedAction();
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

								ExpiredAction?.Invoke();
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