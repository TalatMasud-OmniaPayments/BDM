namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Framework.Interface.Receipts;
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Vtm.Workflow.Common.Steps;
    using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System;
    using System.Linq;
    using System.Threading;
	using System.Threading.Tasks;

	internal class ConfirmationStep : WorkflowStep
	{
        
        
        //private readonly TaskCompletionSource<bool> _completion;
        public Action<object> PrintAction { get; set; }

        public ConfirmationStep(IResolver container) : base(container)
		{
			//_completion = new TaskCompletionSource<bool>();
           
            

            
        }

        public void ExecuteAsync()
        {
            _logger?.Info($"Execute Step: Statement Print Confirmation");

            SetCurrentStep($"{Properties.Resources.StepChargesConfirmation}");

			var ctx = Context.Get<IStatementPrintingContext>();
			var cancellationToken = new CancellationTokenSource();

			_navigator.RequestNavigationTo<IReprintViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.BackVisibility = viewModel.CancelVisibility = true;
				viewModel.transactions = Context.Get<IStatementPrintingContext>().UserTransactions;
                viewModel.TotalTransactions = Context.Get<IStatementPrintingContext>().UserTransactions.Count();
                viewModel.accountNumber = ctx.SelectedAccount.Number;
                viewModel.StartDate = ctx.StartDate;
                viewModel.EndDate = ctx.EndDate;

                viewModel.PrintAction = async  (userTransaction) =>
                {

                    cancellationToken?.Cancel();
                    cancellationToken = null;
                    PrintAction?.Invoke(userTransaction);
                };
                viewModel.CancelAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					CancelAction?.Invoke();
                    //_completion.TrySetResult(false);
                };
				viewModel.BackAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					BackAction?.Invoke();
                    //_completion.TrySetResult(false);
                };

				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

                    DefaultAction?.Invoke();

                    //_completion.TrySetResult(true);
				};

                if (Context.Get<IStatementPrintingContext>().SelfServiceMode)
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

                                LoadMainScreen();
                                //CancelAction?.Invoke();
                            };
                        });
                    };
                }
            });

			//return await _completion.Task;
		}

		public override void Dispose()
		{

		}
	}
}