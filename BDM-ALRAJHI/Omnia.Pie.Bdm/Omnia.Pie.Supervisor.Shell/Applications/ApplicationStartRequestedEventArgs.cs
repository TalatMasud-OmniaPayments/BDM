using System;

namespace Omnia.Pie.Supervisor.Shell.Applications
{
	public class ApplicationStartRequestedEventArgs : EventArgs
	{
		public ApplicationStartRequestedEventArgs(string applicationName)
		{
			ApplicationName = applicationName;
		}

		public string ApplicationName { get; }
	}
}
