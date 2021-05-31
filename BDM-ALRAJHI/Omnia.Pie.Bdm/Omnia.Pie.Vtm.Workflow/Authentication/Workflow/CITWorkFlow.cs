using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Workflow.Common.Context;

namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    internal class CITWorkFlow : Workflow
    {//private readonly FingerprintScanRegisterStep _fingerprintScanRegisterStep;


        IBaseViewModel vm;
        public CITWorkFlow(IResolver container) : base(container)
        {

            AddSteps($"{Properties.Resources.StepUpdateInfoSuccess}");

            Screens.OnLoggoutSupervisoryUser += Screens_OnLoggoutSupervisoryUser;
            Screens.OnDoorAccessed += Screens_OnDoorAccessed;
        }

        private void Screens_OnDoorAccessed()
        {


            vm.CancelVisibility = false;
        }

        public void Execute(IDataContext _context)
        {
            _logger?.Info($"Execute Step: Business User Main Menu Step");
            Context = _context;
            var cancellationToken = new CancellationTokenSource();

            _navigator.RequestNavigationTo<ISupervisorLoginSuccessViewModel>((viewModel) =>
            {
                vm = viewModel;
                viewModel.CancelVisibility = true;

                viewModel.CancelAction = async () =>
                {
                    Screens.SetLogoutFromMainApp();
                    LoadMainScreen();
                };
            });
        }

        private void Screens_OnLoggoutSupervisoryUser()
        {
            Console.WriteLine("Event fired");
            
            _journal.userLogout(Context.Get<IAuthDataContext>().loggedInUserInfo.UserType);
            LoadMainScreen();
            Screens.OnLoggoutSupervisoryUser -= Screens_OnLoggoutSupervisoryUser;
        }

        public override void Dispose()
        {

        }
    }
}
