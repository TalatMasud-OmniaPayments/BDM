namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using System.Threading;

    internal class PrintUserStep : WorkflowStep
    {

        public PrintUserStep(IResolver container) : base(container)
        {
        }
        public void Execute()
        {
            _logger?.Info($"Execute Step: Create User Step");
            SetCurrentStep($"{nameof(PrintUserStep)}");

            var cancellationToken = new CancellationTokenSource();

            _navigator.RequestNavigationTo<IPrintUserViewModel>((viewModel) =>
            {
                viewModel.BackVisibility = viewModel.CancelVisibility = true;
                viewModel.BackAction = () =>
                {
                    BackAction?.Invoke();
                };

                viewModel.CancelAction = () =>
                {
                    cancellationToken?.Cancel();
                    cancellationToken = null;

                    CancelAction?.Invoke();
                };

            });
        }

        public override void Dispose()
        {

        }

    }
}
