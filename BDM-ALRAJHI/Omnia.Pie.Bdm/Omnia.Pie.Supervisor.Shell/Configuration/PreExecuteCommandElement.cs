using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	public class PreExecuteCommandElement : ConfigurationElement
	{
		[ConfigurationProperty("value")]
		public string Command => (string)base["value"];
	}

	[ConfigurationCollection(typeof(PreExecuteCommandElement), AddItemName = "command")]
	public class PreExecuteCommandElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new PreExecuteCommandElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PreExecuteCommandElement)element).Command;
		}
	}
}