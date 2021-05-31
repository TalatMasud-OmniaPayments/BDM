namespace Omnia.Pie.Supervisor.Shell.Applications
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Supervisor.Shell.Configuration;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Configuration;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class ApplicationCoordinator
	{
		private const int StartDelay = 3000;
		private ILogger _logger { get; }
		private IUnityContainer Container { get; }
		public event EventHandler ProcessExited;
		private List<ApplicationInfo> _applications;
		private List<string> _preExecuteCommands;
		private readonly SynchronizationContext syncContext;

		public ApplicationCoordinator(ILogger logger, IUnityContainer container)
		{
			_logger = logger;
			Container = container;
			syncContext = AsyncOperationManager.SynchronizationContext;
		}

		public void BeginCoordination()
		{
			var controlPanelSection = LoadControlPanelSection();
			var applications = LoadAppications(Container, controlPanelSection);
			var preExecuteCommands = LoadPreExecuteCommands(controlPanelSection);

			if (_applications != null)
			{
				throw new InvalidOperationException($"Can't start coordination of applications. Coordination is already in progress.");
			}

			_applications = applications.ToList();
			foreach (ApplicationInfo application in _applications)
			{
				application.RequestListener.ApplicationStartRequested += RequestListener_ApplicationStartRequested;
				application.ProcessManager.ProcessExited += ProcessManager_ProcessExited;
			}
			_preExecuteCommands = preExecuteCommands.ToList();
		}

		public async Task StartApplicationAsync(string applicationName)
		{
			var applicationToStart = _applications.Find(a => a.Configuration.ApplicationName.Equals(applicationName));

			if (applicationToStart == null)
			{
				_logger.Error($"[{this}]: Can't find application by name [{applicationName}].");
				return;
			}

			if (applicationToStart.ProcessManager.IsProcessRunning())
			{
				_logger.Info($"[{this}]: Application [{applicationName}] won't be started, because it is already running.");
				return;
			}

			if (_preExecuteCommands?.Count > 0)
			{
				_logger.Info($"[{this}]: Application [{applicationName}] starting preexecute command.");
				foreach (string command in _preExecuteCommands)
				{
					if (!string.IsNullOrWhiteSpace(command))
					{
						applicationToStart.ProcessManager.ExecuteCmdCommand(command);
					}
				}
			}

			foreach (ApplicationInfo application in _applications)
			{
				if (application != applicationToStart)
				{
					await application.ProcessManager.ExitProcessAsync();
				}
			}

			await Task.Delay(StartDelay); // Safety delay to give more time to shutdown activex controls
			applicationToStart.ProcessManager.StartProcess();
		}

		private async void RequestListener_ApplicationStartRequested(object sender, ApplicationStartRequestedEventArgs e)
		{
			try
			{
				_logger.Info($"[{this}]: ApplicationStartRequested: e.ProcessName=[{e.ApplicationName}].");
				await StartApplicationAsync(e.ApplicationName);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		private void ProcessManager_ProcessExited(object sender, EventArgs e)
		{
			var processManager = (ProcessManager)sender;
			try
			{
				syncContext.Post(es => ProcessExited?.Invoke(this, EventArgs.Empty), EventArgs.Empty);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		#region Configuration

		private ControlPanelSection LoadControlPanelSection()
		{
			return (ControlPanelSection)ConfigurationManager.GetSection(ControlPanelSection.Name);
		}

		private IEnumerable<ApplicationInfo> LoadAppications(IUnityContainer container, ControlPanelSection configuration)
		{
			var applications = new List<ApplicationInfo>();

			foreach (ApplicationElement applicationConfiguration in configuration.Applications)
			{
				var application = new ApplicationInfo()
				{
					Configuration = applicationConfiguration,
					ProcessManager = container.Resolve<ProcessManager>(),
					RequestListener = container.Resolve<RequestListener>(),
				};

				application.ProcessManager.Configure(applicationConfiguration);
				application.RequestListener.Configure(applicationConfiguration.ApplicationName);

				applications.Add(application);
			}

			return applications;
		}

		private IEnumerable<string> LoadPreExecuteCommands(ControlPanelSection configuration)
		{
			return configuration.PreExecuteCommands.Cast<PreExecuteCommandElement>().Select(e => e.Command);
		}

		#endregion Configuration
	}
}