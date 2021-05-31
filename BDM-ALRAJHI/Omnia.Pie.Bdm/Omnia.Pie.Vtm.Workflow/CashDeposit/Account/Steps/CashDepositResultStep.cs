namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
	using System;
	using System.Threading.Tasks;

	public class CashDepositResultStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _taskSource;
		public Action _receiptSelected;

		public CashDepositResultStep(IResolver container) : base(container)
		{
			_taskSource = new TaskCompletionSource<bool>();
		}

		public async Task<bool> SelectReceipt()
		{
			try
			{
                //SetCurrentStep(nameof(CashDepositResultStep));
                _logger?.Info($"Execute Step: Select Receipt");

                var _context = Context.Get<ICashDepositContext>();
				var vm = _container.Resolve<ICashDepositResultViewModel>();
				vm.SelectedAccount = _context.SelectedAccount;
				vm.TotalAmount = _context.TotalAmount;

				vm.PrintReceipt = (printReceipt, lang) =>
				{
					try
					{
						_context.PrintReceipt = printReceipt;
						_context.ReceiptLanguage = lang;
						LoadWaitScreen();
						_receiptSelected();
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
						_taskSource.SetException(new InvalidOperationException());
					}
				};

				_navigator.RequestNavigation(vm);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				_taskSource.SetException(new InvalidOperationException());
			}
			return await _taskSource.Task;
		}

		public override void Dispose()
		{

		}
	}
}