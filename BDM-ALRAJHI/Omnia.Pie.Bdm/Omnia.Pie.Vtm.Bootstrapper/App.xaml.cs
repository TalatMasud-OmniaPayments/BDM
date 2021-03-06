namespace Omnia.Pie.Vtm.Bootstrapper
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Unity.Configuration;
    using Omnia.Pie.Client.Journal.Interface;
    using Omnia.Pie.Supervisor.Shell.Service;
	using Omnia.Pie.Vtm.Bootstrapper.Configurations;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Module;
	using Omnia.Pie.Vtm.Framework.Logger;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interactivity;
	using System.Windows.Threading;

	public partial class App : Application
	{
		private ILogger _logger;
		private IUnityContainer container;
        private System.Timers.Timer _ejCreationTimer;
        private String _terminalId;
        private String _branch;

        protected override void OnExit(ExitEventArgs e)
		{
            DisposeEJCreationTimer();
            var _eSpace = container.Resolve<IESpaceTerminalCommunication>();
			_eSpace.Logout();
			base.OnExit(e);
		}

		protected async override void OnStartup(StartupEventArgs e)
		{
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
			Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			System.Windows.Forms.Application.ThreadException += Application_ThreadException;

			try
			{
				LoggerContext.SetProperty("ApplicationVersion", Assembly.GetExecutingAssembly().GetName().Version.ToString());
				LoggerContext.SetProperty("TerminalId", TerminalConfiguration.Section.Id);

				var logWriterFactory = new LogWriterFactory();
				var logWriter = logWriterFactory.Create();
				Logger.SetLogWriter(logWriter, throwIfSet: true);

				LogApplicationStartup();

				container = CreateContainer();
				_logger = container.Resolve<ILogger>();

				var serviceManager = container.Resolve<IServiceManager>();
				InitializeServiceManager(serviceManager);
                
                var receiptFormatter = container.Resolve<IReceiptFormatter>();
				InitializeReceipts(receiptFormatter);

				var mainView = container.Resolve<MainWindow>();
				var keyboardEmulationBehavior = container.Resolve<KeyboardBehavior>();
				Interaction.GetBehaviors(mainView).Add(keyboardEmulationBehavior);

				var languageObserver = container.Resolve<ILanguageObserver>();
				languageObserver.Language = Language.English;

				var _navigator = container.Resolve<INavigationObserver>();
				_navigator.RequestNavigationTo<IAnimationViewModel>((viewModel) =>
				{
					viewModel.Type(AnimationType.InitializingApplication);
				});

				LoadModules(container);

				var _devStatus = container.Resolve<ChannelManagementDeviceStatusService>();
				var _channelService = container.Resolve<IChannelManagementService>();
				await _channelService.SendDeviceStatus(_devStatus.GetDevicesStatus());

				container.Resolve<INdcService>();
				container.Resolve<IESpaceTerminalCommunication>();

				_logger?.Info("Initialized Application");

                _terminalId = TerminalConfiguration.Section.Id;
                _branch = TerminalConfiguration.Section.Location;

                ActivateEJCreationTimer();
            }
			catch (Exception ex)
			{
				TryLogException(ex);
				throw;
			}

			base.OnStartup(e);
		}

		private IUnityContainer CreateContainer()
		{
			var container = new UnityContainer();
			container.LoadConfiguration();

			return container;
		}

		private void InitializeServiceManager(IServiceManager serviceManager)
		{
			serviceManager.Acquirer = BuildAcquirer(TerminalConfiguration.Section);
			serviceManager.Terminal = BuildTerminal(TerminalConfiguration.Section);
		}

		private Acquirer BuildAcquirer(TerminalSection terminal) => new Acquirer
		{
			AcquiringInstitutionId = terminal.AcquiringInstitutionId,
			BranchId = terminal.BranchId
		};

		private Terminal BuildTerminal(TerminalSection terminal) => new Terminal
		{
			Id = terminal?.Id,
			BranchId = terminal?.BranchId,
			BranchCode = terminal?.BranchCode,
			Platform = terminal?.Platform,
			Currency = terminal?.Currency,
			LocationCity = terminal?.LocationCity,
			LocationCountry = terminal?.LocationCountry,
			MerchantId = terminal?.MerchantId,
			Type = terminal?.Type,
			OwnerName = terminal?.OwnerName,
			StateName = terminal?.StateName
		};

		private static void InitializeReceipts(IReceiptFormatter receiptFormatter)
		{
			var terminal = TerminalConfiguration.Section;
			receiptFormatter.Metadata = new ReceiptMetadata
			{
				TerminalId = terminal.Id,
				TerminalLocation = terminal.Location,
				OwnerName = terminal.OwnerName
			};
		}

		private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			TryLogException(e.Exception as Exception);
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			TryLogException(e.Exception as Exception);
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			TryLogException(e.Exception);
			e.Handled = true;
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			TryLogException(e.Exception);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			TryLogException(e.ExceptionObject as Exception);
		}

		private void TryLogException(Exception ex)
		{
			try
			{
				_logger?.Exception(ex);
				Logger.Writer.Write(ex, (string)null, -1, 1, TraceEventType.Critical);
			}
			catch (Exception e)
			{
				Debug.Write(e);
			}
		}

		private void LoadModules(IUnityContainer container)
		{
			var Modules = ModuleConfiguration.Modules?
													.Select(m => Type.GetType(m, throwOnError: true))
													.Select(t => container.Resolve(t))
													.Cast<IModule>()
													.ToList();

			Modules.ForEach(m => m.Initialize());
		}

		private void LogApplicationStartup()
		{
			Logger.Writer.Write($"Application Startup (version : {Assembly.GetExecutingAssembly().GetName().Version.ToString()})");
		}

        private void ActivateEJCreationTimer()
        {
            if (_ejCreationTimer == null)
            {
                _ejCreationTimer = new System.Timers.Timer();
                _ejCreationTimer.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteEJCreationTime);
                _ejCreationTimer.Interval = (double)60000; // 1 Min
                _ejCreationTimer.AutoReset = true;
            }
            _ejCreationTimer.Start();
        }

        private void ExecuteEJCreationTime(object sender, EventArgs e)
        {
            var timeNow = DateTime.Now.ToString("HH:mm");
            if (timeNow == "00:06")
            {
                IJournal _journal = container.Resolve<IJournal>();
                _journal.Write($"Terminal: {_terminalId}");
                _journal.Write($"Branch: {_branch}");
            }
        }

        private void DisposeEJCreationTimer()
        {
            if (_ejCreationTimer != null)
            {
                _ejCreationTimer.Enabled = false;
                _ejCreationTimer.Close();
                _ejCreationTimer = null;
            }
        }
    }
}