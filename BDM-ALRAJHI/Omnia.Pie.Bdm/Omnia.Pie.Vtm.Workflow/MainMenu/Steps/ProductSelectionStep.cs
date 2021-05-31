namespace Omnia.Pie.Vtm.Workflow.MainMenu.Steps
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;

	public class ProductSelectionStep : WorkflowStep
	{
		public Action<string, string, string> SendMoreInfoAction { get; set; }

		public ProductSelectionStep(IResolver container) : base(container)
		{

		}

		public void Execute()
		{
            _logger?.Info($"Execute Step: Product Selection");

            _navigator.RequestNavigationTo<IProductsInfoViewModel>((viewModel) =>
			{
				viewModel.BackVisibility = viewModel.CancelVisibility = true;

				viewModel.CancelAction = () =>
				{
					LoadMainScreen();
				};
				viewModel.BackAction = () =>
				{
					BackAction?.Invoke();
				};
				viewModel.ProductSelectedAction = (product) =>
				{
					_journal.TransactionStarted(EJTransactionType.NonFinancial, "Product Information");
					_navigator.RequestNavigationTo<ISendMoreInfoViewModel>((vm) =>
					{
						vm.BackVisibility = vm.DefaultVisibility = vm.CancelVisibility = true;
						vm.ProductType = product;

						vm.CancelAction = () =>
						{
							_journal.TransactionCanceled();
							LoadMainScreen();
						};
						vm.BackAction = () =>
						{
							Execute();
						};
						vm.SendMoreInfoAction = (email, mobile) =>
						{
							_journal.TransactionSucceeded("");
							SendMoreInfoAction?.Invoke(product, email, mobile);
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