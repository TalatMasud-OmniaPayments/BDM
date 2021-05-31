namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.StatementPrinting.Context;
	using System;
	using System.Linq;
	using System.Threading;

	internal class AccountSelectionStep : WorkflowStep
	{
		public Action DefaultAction { get; set; }
		//private readonly GetChargesStep _getCharges;

		public AccountSelectionStep(IResolver container) : base(container)
		{
			//_getCharges = _container.Resolve<GetChargesStep>();
			//_getCharges.Context = Context;
		}

		public void ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Account Selection");

            SetCurrentStep($"{Properties.Resources.StepAccountSelection}");

			var cancellationToken = new CancellationTokenSource();
			var ctx = Context.Get<IStatementPrintingContext>();

			_navigator.RequestNavigationTo<IStatementPrintingViewModel>((viewModel) =>
			{
				if (ctx.SelectedAccount == null)
					ctx.SelectedAccount = ctx.Accounts.FirstOrDefault();

				viewModel.SelectedAccount = ctx.SelectedAccount;
				viewModel.StartDate = ctx.StartDate;
				viewModel.EndDate = ctx.EndDate;

				viewModel.Accounts = ctx.Accounts;
				viewModel.Amount = ctx.Amount = 25;

				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;

				viewModel.CancelAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					CancelAction?.Invoke();
				};
				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					Context.Get<IStatementPrintingContext>().StartDate = viewModel.StartDate;
					Context.Get<IStatementPrintingContext>().EndDate = viewModel.EndDate;
					Context.Get<IStatementPrintingContext>().NumberofMonths = viewModel.NumberofMonths.ToString();
					Context.Get<IStatementPrintingContext>().SelectedAccount = viewModel.SelectedAccount;
					Context.Get<IStatementPrintingContext>().Period = viewModel.NumberofMonths;
					_journal.AccountSelected(viewModel.SelectedAccount.Number);

					DefaultAction?.Invoke();
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
		}

		public override void Dispose()
		{

		}
	}
}