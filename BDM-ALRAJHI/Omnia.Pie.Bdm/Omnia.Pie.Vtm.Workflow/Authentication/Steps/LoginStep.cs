using Omnia.Pie.Vtm.Bootstrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.Interface;

namespace Omnia.Pie.Vtm.Workflow.Authentication.Steps
{
    public class LoginStep : WorkflowStep
    {
        private readonly FingerprintScanningStep _fingerprintScanningStep;

        public LoginStep(IResolver container) : base(container)
        {
        }

        public void Execute()
        {
            _logger?.Info($"Execute Step: Select Service Type");

            _navigator.RequestNavigationTo<ILoginViewModel>((viewModel) =>
            {
                viewModel.BackVisibility = viewModel.CancelVisibility = true;
            });
        }
        public override void Dispose()
        {

        }
    }
}
