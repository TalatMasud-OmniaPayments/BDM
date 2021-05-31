namespace Omnia.PIE.VTA
{
	using Microsoft.Practices.Unity;
	using Omnia.PIE.VTA.Common;
	using Omnia.PIE.VTA.Views;
	using Omnia.PIE.VTA.Views.MsgBoxes;
	using System.Windows;
	using System.Windows.Threading;
	using Microsoft.Practices.Unity.Configuration;
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Reflection;
	using Omnia.Pie.Vtm.Services.Interface;

	public partial class App : Application
	{
		private IUnityContainer UnityContainer { get; set; }
		public MainWindow Shell { get; set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			TellerAppSingleInstance.Make();

			var logWriterFactory = new LogWriterFactory();
			var logWriter = logWriterFactory.Create();
			Logger.SetLogWriter(logWriter, throwIfSet: true);
			LogApplicationStartup();

			UnityContainer = new UnityContainer();
			UnityContainer.LoadConfiguration();

			var serviceManager = UnityContainer.Resolve<IServiceManager>();
			serviceManager.Acquirer = new Acquirer
			{
				AcquiringInstitutionId = "519855",
				BranchId = "000112"
			};
			serviceManager.Terminal = new Terminal
			{
				Id = "AHBV1001",
				BranchId = "852528",
				BranchCode = "SF012",
				Currency = "784",
				LocationCity = "DUB",
				LocationCountry = "UAE",
				MerchantId = "00012785652",
				Platform = "RT002",
				Type = "01",
				OwnerName = "AHB",
				StateName = "DUB"
			};

			MainWindow = UnityContainer.Resolve<LogOnWindow>();
			MainWindow.Show();
			base.OnStartup(e);
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
			WpfMessageBox.Show(errorMessage, "Error", WpfMessageBoxButton.OK, WpfMessageBoxImage.Error);

			Logger.Writer.Exception(e.Exception);
			e.Handled = true;
		}

		private void LogApplicationStartup()
		{
			Logger.Writer.Write($"STARTUP (application version {Assembly.GetExecutingAssembly().GetName().Version.ToString()})");
		}
	}
}