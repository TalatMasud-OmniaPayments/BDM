namespace Omnia.Pie.Vtm.Workflow
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Authentication;
    using Omnia.Pie.Vtm.Workflow.Common;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Steps;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	public abstract class Workflow : BaseFlow
	{
		private List<string> _steps;
		public List<string> Steps { get { return _steps; } }
        

        public Workflow(IResolver container) : base(container)
		{
			if (_steps == null)
				_steps = new List<string>();

		}

		public bool AddSteps(string step)
		{
			DisposeSteps();
			_steps = step.Split(',').ToList();

			if (_steps.Any())
				_videoService.Steps = GetSteps(_steps);

			return true;
		}

		private ObservableCollection<Step> GetSteps(List<string> steps)
		{
			var _steps = new ObservableCollection<Step>();
			var i = 1;

			foreach (var item in steps)
			{
				_steps.Add(new Step()
				{
					DisplayName = item,
					StepNumber = i,
					AssociatedAdds = new ObservableCollection<AssociatedAdd>(),
				});

				i++;
			}

			_steps.FirstOrDefault().IsCurrentStep = true;

			return _steps;
		}

		protected void SendNotification(Services.Interface.TransactionType transactionType, string transactionMode, string cardNumber, string amount, string transactionReference, string accountNumber, string cif, Services.Interface.Enums.TransactionStatus transactionStatus = Services.Interface.Enums.TransactionStatus.Successful, string reason = "", string statementMonths = "", string cheaqueLeaves = "", string responseCode = "")
		{
			try
			{
				var _transactionService = _container.Resolve<ITransactionService>();
				_transactionService.TransactionNotificationAsync(transactionType, transactionMode, cardNumber, amount, transactionReference, accountNumber, cif, transactionStatus, reason, statementMonths, "", responseCode);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		protected void FinishTransaciton()
		{
			_journal.TransactionEnded();
			DisposeSteps();

			_navigator.Push<IAnotherTransactionConfirmationViewModel>((vm) =>
			{
				vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
				vm.YesAction = () =>
				{
					LoadSelfServiceMenu();
				};
				vm.NoAction = vm.ExpiredAction = () =>
				{
					LoadMainScreen();
				};
			});
		}

		public void LoadSelfServiceMenu()
		{
            /*switch (_container?.Resolve<ISessionContext>()?.CardUsed?.CardType)
			{
				case Devices.Interface.CardType.CreditCard:
					{
						var authCreditCardStandBy = _container.Resolve<AuthenticatedCreditCardStandbyWorkflow>();
						authCreditCardStandBy.Execute();
						break;
					}
				case Devices.Interface.CardType.EmiratesIdCard:
				case Devices.Interface.CardType.DebitCard:
					{
						var authDebitCardStandBy = _container.Resolve<AuthenticatedDebitCardStandbyWorkflow>();
						authDebitCardStandBy.Execute();
						break;
					}
				case Devices.Interface.CardType.Offus:
					{
						break;
					}
			}


            if (_container.Resolve<IAuthDataContext>().Authenticated && _container.Resolve<IAuthDataContext>().AuthenticatedByFingerprint)
			{
				var authDebitCardStandBy = _container.Resolve<AuthenticatedDebitCardStandbyWorkflow>();
				authDebitCardStandBy.Execute();
			}
            

            if (Context.Get<IAuthDataContext>().Authenticated && Context.Get<IAuthDataContext>().AuthenticatedByFingerprint)
            {
                var authDebitCardStandBy = _container.Resolve<AuthenticationLoginWorkflow>();
                authDebitCardStandBy.Execute();
            }*/

            //if (Context.Get<IAuthDataContext>().loggedInUserInfo.UserType == "Business")
            //{
                var businessMainMenuSelection = _container.Resolve<BusinessUserMainMenuWorkFlow>();
               businessMainMenuSelection.Execute();
            //}
            //else
            //{
                //var businessMainMenuSelection = _container.Resolve<AdminUserMainMenuWorkFlow>();
                //businessMainMenuSelection.Execute(Context);
            //}
        }

		protected void DisposeSteps()
		{
			if (_videoService?.Steps != null)
			{
				_steps = null;
				_videoService.Steps = null;
			}
		}

		public void IgnoreExceptions(Action act)
		{
			try
			{ act.Invoke(); }
			catch { }
		}
	}
}