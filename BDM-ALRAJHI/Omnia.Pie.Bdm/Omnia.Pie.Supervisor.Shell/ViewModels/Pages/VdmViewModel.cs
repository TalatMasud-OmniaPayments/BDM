using Microsoft.Practices.Unity;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Supervisor.Shell.Applications;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class VdmViewModel : PageViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.VDM == true ? true : false);

        private readonly ApplicationCoordinator _applicationCoordinator;
		private readonly ILogger logger = ServiceLocator.Instance.Resolve<ILogger>();

		public VdmViewModel(ApplicationCoordinator applicationCoordinator)
		{
			RunVdm = new DelegateCommand(RunVdmApplicationAsync);
			_applicationCoordinator = applicationCoordinator;
			_applicationCoordinator.ProcessExited += _applicationCoordinator_ProcessExited;
		}

		private void _applicationCoordinator_ProcessExited(object sender, EventArgs e)
		{
			IsVdmRunning = false;
			Context.DisplayProgress = false;
			_journal?.ExitedVdmMode();
			_channelManagementService.InsertEventAsync("Exited Vdm Mode", "True");
		}

		public ICommand RunVdm { get; }

		private bool isVdmRunning;
		public bool IsVdmRunning
		{
			get
			{
				return isVdmRunning;
			}
			set
			{
				SetProperty(ref isVdmRunning, value);
			}
		}

		private async void RunVdmApplicationAsync()
		{
			IsVdmRunning = true;
			Context.DisplayProgress = true;

			try
			{
				_journal?.EnteringVdmMode();
				await _channelManagementService.InsertEventAsync("Entering Vdm Mode", "True");
				await _applicationCoordinator.StartApplicationAsync("Vdm");
			}
			catch (Exception ex)
			{
				logger.Exception(ex);
			}
		}
	}
}