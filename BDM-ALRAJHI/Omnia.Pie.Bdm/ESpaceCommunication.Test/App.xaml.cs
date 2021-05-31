namespace ESpaceCommunication.Test
{
	using ESpaceCommunication.Test.ViewModels;
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Unity.Configuration;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Windows;
	using System.Windows.Threading;

	public partial class App : Application
	{
		private ILogger _logger;

		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				base.OnStartup(e);
				Dispatcher.UnhandledException += Dispatcher_UnhandledException;

				var _container = new UnityContainer();
				_container.LoadConfiguration();
				Container.Instance = _container;
				_logger = _container.Resolve<ILogger>();

				var logWriterFactory = new LogWriterFactory();
				var logWriter = logWriterFactory.Create();
				Logger.SetLogWriter(logWriter, throwIfSet: true);
				_logger?.Info("Logger Test.");

				MainWindow = _container.Resolve<MainWindow>();
				MainWindow.DataContext = _container.Resolve<MainWindowViewModel>();
				MainWindow.Show();
			}
			catch (Exception ex)
			{
				_logger?.Exception(ex);
				MessageBox.Show($"Failed to start: {ex}.", "Communication Test");
			}
		}

		private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			_logger?.Exception(e.Exception);
		}
	}
}
