using Omnia.Pie.Supervisor.Shell.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Applications
{
	public class ApplicationInfo
	{
		public ApplicationElement Configuration { get; set; }
		public ProcessManager ProcessManager { get; set; }
		public RequestListener RequestListener { get; set; }
	}
}
