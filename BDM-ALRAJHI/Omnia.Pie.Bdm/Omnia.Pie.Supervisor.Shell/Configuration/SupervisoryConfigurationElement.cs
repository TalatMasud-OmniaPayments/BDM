using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	public class SupervisoryConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("id")]
		public string Id => (string)base["id"];

		[ConfigurationProperty("value")]
		public string Value => (string)base["value"];
	}
}
