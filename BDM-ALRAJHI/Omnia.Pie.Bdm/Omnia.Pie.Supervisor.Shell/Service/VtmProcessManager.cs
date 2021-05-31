using Omnia.Pie.Client.Infrastructure.Interface.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.Service
{
	public class VtmProcessManager
	{
		private readonly ILogger logger;
		private readonly DeviceService deviceService;
		private readonly Lazy<string> vtmProcessName;
		private List<string> terminatedVtmProcesses;

		public VtmProcessManager(ILogger logger, DeviceService deviceService)
		{
			this.logger = logger;
			this.deviceService = deviceService;
			this.vtmProcessName = new Lazy<string>(() => ConfigurationManager.AppSettings["VtmProcessName"]);
		}

		public void TerminateVtmProcesses()
		{
			if (terminatedVtmProcesses != null)
			{
				return;
			}

			terminatedVtmProcesses = new List<string>();
			foreach (var vtmProcess in Process.GetProcessesByName(vtmProcessName.Value))
			{
				logger.Info($"Terminating VTM process {vtmProcess.Id}.");
				try
				{
					terminatedVtmProcesses.Add(vtmProcess.MainModule.FileName);
				}
				catch (Win32Exception ex)
				{
					logger.Exception(ex);
				}
				vtmProcess.Kill();
				vtmProcess.WaitForExit();
			}

			logger.Info($"Terminated processes are: {string.Join(Environment.NewLine, terminatedVtmProcesses)}");
		}

		public void RestoreVtmProcesses()
		{
			if (terminatedVtmProcesses != null)
			{
				if (terminatedVtmProcesses.Count > 0)
				{
					deviceService.DeactivateDevices();
				}

				foreach (string vtmProcess in terminatedVtmProcesses)
				{
					try
					{
						logger.Info($"Starting process {vtmProcess}");
						var startInfo = new ProcessStartInfo();
						startInfo.FileName = vtmProcess;
						startInfo.WorkingDirectory = Path.GetDirectoryName(vtmProcess);

						Process.Start(startInfo);
					}
					catch (Exception ex)
					{
						logger.Exception(ex);
					}
				}

				if (terminatedVtmProcesses.Count > 0)
				{
					System.Threading.Thread.Sleep(10 * 1000);
					deviceService.ReactivateDevices();
				}

				terminatedVtmProcesses = null;
			}
		}
	}
}
