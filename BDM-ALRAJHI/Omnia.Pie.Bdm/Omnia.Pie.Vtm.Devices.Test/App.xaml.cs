using System;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Omnia.Pie.Vtm.Framework.Logger;
using Omnia.Pie.Vtm.Framework.Interface;

namespace Omnia.Pie.Vtm.Devices.Test
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private ILogger _logger;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			LoggerContext.SetProperty("TerminalId", "AHBV1001");
			Logger.SetLogWriter(new LogWriterFactory().Create(), throwIfSet: true);
			
			Dispatcher.UnhandledException += Dispatcher_UnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			_logger = UnityContainer.Container.Resolve<ILogger>();
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) =>
			_logger.Exception(e.ExceptionObject as Exception);

		private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			_logger?.Exception(e.Exception);
		}
	}
}
