namespace Omnia.Pie.Supervisor.Shell
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Supervisor.Shell.Applications;
	using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Supervisor.Shell.Utilities;
    using Omnia.Pie.Supervisor.Shell.ViewModels;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Module;
	using System;

	internal class Module : IModule
	{
		private ApplicationCoordinator ApplicationCoordinator { get; }
		private MainWindow MainWindow { get;}
        private ILogger _logger;

		public Module(IUnityContainer container, ApplicationCoordinator applicationCoordinator, MainWindow mainWindow)
		{
            mainWindow.Title = "supervisory";

            ServiceLocator.Instance = container ?? throw new ArgumentNullException(nameof(container));
			_logger = ServiceLocator.Instance.Resolve<ILogger>();
			ApplicationCoordinator = applicationCoordinator ?? throw new ArgumentNullException(nameof(applicationCoordinator));
			MainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
		}

		public void Initialize()
		{
			ApplicationCoordinator.BeginCoordination();

            SupervisorViewModel supervisory = new SupervisorViewModel(ApplicationCoordinator);

            MainWindow.DataContext = supervisory;

            MainWindow.Show();

            var opStatus = supervisory.Context._deviceSensors.GetOperatorStatus();
            if (opStatus == Vtm.Devices.Interface.SensorsStatus.Supervisor)
            {
                Screens.UpdateToMainApp(false);
            }
            else
            {
                Screens.UpdateToMainApp(true);
            }
            
            Screens.supervisoryWindow = supervisory;


            //_logger.Info("Supervisor.Shell Initialized");
		}
	}
}