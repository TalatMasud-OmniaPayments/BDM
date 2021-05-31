using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	public class ControlPanelSection : ConfigurationSection
	{
		public const string Name = "controlPanel";

		[ConfigurationProperty("applications", IsRequired = true)]
		public ApplicationElementCollection Applications => (ApplicationElementCollection)base["applications"];

		[ConfigurationProperty("preExecuteCommands", IsRequired = true)]
		public PreExecuteCommandElementCollection PreExecuteCommands => (PreExecuteCommandElementCollection)base["preExecuteCommands"];
	}
}
