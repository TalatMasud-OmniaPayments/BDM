namespace Omnia.Pie.Vtm.Workflow.MainMenu.Steps
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;

	public class ProductSubmitSuccessStep : WorkflowStep
	{
		public Action YesAction { get; set; }
		public Action NoAction { get; set; }

		public ProductSubmitSuccessStep(IResolver container) : base(container)
		{

		}

		public void Execute()
		{
            _logger?.Info($"Execute Step: Product Inquiry Success");

            _navigator.RequestNavigationTo<IProductInfoSubmitSuccessViewModel>((viewModel) =>
			{
				viewModel.YesAction = () =>
				{
					YesAction?.Invoke();
				};
				viewModel.NoAction = viewModel.ExpiredAction = () =>
				{
					NoAction?.Invoke();
				};

				viewModel.StartTimer(new TimeSpan(0, 0, InactivityTimer));
			});
		}

		public override void Dispose()
		{

		}
	}
}