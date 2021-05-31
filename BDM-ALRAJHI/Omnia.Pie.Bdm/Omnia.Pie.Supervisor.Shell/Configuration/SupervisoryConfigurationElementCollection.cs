using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	[ConfigurationCollection(typeof(SupervisoryConfigurationElement), AddItemName = "setting")]
	public class SupervisoryConfigurationElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new SupervisoryConfigurationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SupervisoryConfigurationElement)element).Id;
		}
	}

	[ConfigurationCollection(typeof(SupervisoryConfigurationElement), AddItemName = "role")]
	public class SupervisoryConfigurationRolesElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new SupervisoryConfigurationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SupervisoryConfigurationElement)element).Id;
		}
	}

	[ConfigurationCollection(typeof(SupervisoryConfigurationElement), AddItemName = "cassette")]
	public class SupervisoryConfigurationCassettesElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new SupervisoryConfigurationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SupervisoryConfigurationElement)element).Id;
		}
	}
}
