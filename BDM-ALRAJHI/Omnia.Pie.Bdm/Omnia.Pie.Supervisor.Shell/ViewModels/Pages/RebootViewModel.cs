using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Omnia.Pie.Client.Journal.Interface.Extension;
using System.Linq;
using Omnia.Pie.Client.Journal.Interface;
using System.Reflection;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class RebootViewModel : PageViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Reboot == true ? true : false);
        private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();
        //private static IJournal _journal = ServiceLocator.Instance.Resolve<IJournal>();

        public RebootViewModel()
		{
			Yes = new DelegateCommand<object>(
			x =>
				{
					try
					{
						_journal?.RestartMachine();
						_channelManagementService.InsertEventAsync("Restart Machine", "True");

						Process.Start(new ProcessStartInfo()
						{
							WindowStyle = ProcessWindowStyle.Hidden,
							FileName = "cmd",
							Arguments = "/C shutdown -f -r -t 5"
						});
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
					}
				}, x => true);

			YesApplication = new DelegateCommand<object>(
				x =>
				{
					try
					{
                        _journal?.CloseApplication();
                        _channelManagementService.InsertEventAsync("Application closed", "True");

                        var appProcesses = Process.GetProcesses().
								 Where(pr => pr.ProcessName.ToLower().Contains("omnia.pie.bdm.bootstrapper"));

						foreach (var process in appProcesses)
						{
                            //process.Kill();
                            RebootApplication();
                        }

                        
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
					}
				}, x => true);


		}
        private void RebootApplication()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 1 & START \"\" \"" + Assembly.GetEntryAssembly().Location + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Process.GetCurrentProcess().Kill();
        }
        #region Commands

        public ICommand Yes { get; }
		public ICommand YesApplication { get; }

		#endregion
	}
}
