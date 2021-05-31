namespace Omnia.Pie.Supervisor.Shell.Applications
{
	using Omnia.Pie.Supervisor.Shell.Configuration;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	public class ProcessManager
	{
		private const int ExitTimeout = 15000;

		private readonly ILogger _logger;

		public ProcessManager(ILogger logger)
		{
			_logger = logger;
		}

		public event EventHandler ProcessExited;

		public ApplicationElement Configuration { get; private set; }
		private string ProcessName { get; set; }

		public void Configure(ApplicationElement configuration)
		{
			if (Configuration != null)
			{
				throw new InvalidOperationException($"Can't configure process manager for application [{configuration.ApplicationName}]. It is already configured for application [{Configuration.ApplicationName}].");
			}

			Configuration = configuration;
			ProcessName = Path.GetFileNameWithoutExtension(Configuration.ExecutableFilePath);
		}

		public void StartProcess()
		{
			if (IsProcessRunning())
			{
				throw new InvalidOperationException($"Can't start process [{ProcessName}], because there's already running instance of the process.");
			}

			_logger.Info($"[{this}]: Starting process [{ProcessName}] (ExecutableFilePath=[{Configuration.ExecutableFilePath}])");
			var startInfo = new ProcessStartInfo();
			startInfo.FileName = Configuration.ExecutableFilePath;
			startInfo.WorkingDirectory = Path.GetDirectoryName(Configuration.ExecutableFilePath);

			Process process = Process.Start(startInfo);

			process.EnableRaisingEvents = true;
			process.Exited += (sender, e) => { OnProcessExited(process, EventArgs.Empty); };

			_logger.Info($"[{this}]: Started process [{ProcessName}] (ExecutableFilePath=[{Configuration.ExecutableFilePath}])");
		}

		public bool IsProcessRunning()
		{
			Process[] processes = Process.GetProcessesByName(ProcessName);
			return processes.Any();
		}

		public async Task ExitProcessAsync()
		{
			_logger.Info($"[{this}]: Beginning exit for process [{ProcessName}]");
			while (true)
			{
				Process[] processes = Process.GetProcessesByName(ProcessName);
				if (processes.Length == 0)
				{
					_logger.Info($"[{this}]: No processes [{ProcessName}] are running");
					break;
				}

				Process process = processes.First();
				await ExitProcessAsync(process);
			}
			_logger.Info($"[{this}]: Completed exit for process [{ProcessName}]");
		}

		private async Task ExitProcessAsync(Process process)
		{
			try
			{
				_logger.Info($"[{this}]: Waiting for process [{ProcessName}] (PID={process.Id} to exit");
				bool exited = await Task.Run(() => process.WaitForExit(ExitTimeout));
				if (exited)
				{
					_logger.Info($"[{this}]: Process [{ProcessName}] (PID={process.Id} exited");
				}
				else
				{
					_logger.Info($"[{this}]: Process [{ProcessName}] (PID={process.Id} didn't exit within timeout. Killing the process");
					process.Kill();
				}
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		private void OnProcessExited(object sender, EventArgs e)
		{
			_logger.Info($"[{this}]: Process [{ProcessName}] exited");
			ProcessExited?.Invoke(this, EventArgs.Empty);
		}

		public void ExecuteCmdCommand(string command)
		{
			_logger.Info($"[{this}]: Process [{ProcessName}] execute command [{command}]");
			try
			{
				const string cmdFileName = "cmd.exe";
				const string commandTemplate = "/C {0}";

				var startInfo = new ProcessStartInfo(cmdFileName)
				{
					Arguments = string.Format(commandTemplate, command)
				};
				var process = Process.Start(startInfo);
				_logger.Info($"[{this}]: Process [{ProcessName}] execute command [{command}] - started");
				process?.WaitForExit();
				_logger.Info($"[{this}]: Process [{ProcessName}] execute command [{command}] - finished");
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}
	}
}
