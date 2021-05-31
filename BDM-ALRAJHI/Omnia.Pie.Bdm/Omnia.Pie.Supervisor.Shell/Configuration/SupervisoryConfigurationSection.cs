using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	public class SupervisoryConfigurationSection : ConfigurationSection
	{
		public const string Name = "supervisor";

		[ConfigurationProperty("settings", IsRequired = true)]
		public SupervisoryConfigurationElementCollection SupervisoryConfigurations => (SupervisoryConfigurationElementCollection)base["settings"];

		[ConfigurationProperty("roles", IsRequired = true)]
		public SupervisoryConfigurationRolesElementCollection SupervisoryConfigurationRoles => (SupervisoryConfigurationRolesElementCollection)base["roles"];

		[ConfigurationProperty("cassettes", IsRequired = true)]
		public SupervisoryConfigurationCassettesElementCollection SupervisoryConfigurationCassettes => (SupervisoryConfigurationCassettesElementCollection)base["cassettes"];
	}
}
