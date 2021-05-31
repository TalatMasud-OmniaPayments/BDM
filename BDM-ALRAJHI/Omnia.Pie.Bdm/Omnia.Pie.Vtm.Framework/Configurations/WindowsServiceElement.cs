namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class WindowsServiceElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name => (string)base["name"];

		[ConfigurationProperty("timeoutSeconds", DefaultValue = 60)]
		public int TimeoutSeconds => (int)base["timeoutSeconds"];
	}
}