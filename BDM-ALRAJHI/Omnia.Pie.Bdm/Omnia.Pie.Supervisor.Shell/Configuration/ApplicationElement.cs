using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	public class ApplicationElement : ConfigurationElement
	{
		[ConfigurationProperty("applicationName")]
		public string ApplicationName => (string)base["applicationName"];

		[ConfigurationProperty("executableFilePath")]
		public string ExecutableFilePath => (string)base["executableFilePath"];

		[ConfigurationProperty("applicationToStartOnExit")]
		public string ApplicationToStartOnExit => (string)base["applicationToStartOnExit"];
	}

	[ConfigurationCollection(typeof(ApplicationElement), AddItemName = "application")]
	public class ApplicationElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ApplicationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ApplicationElement)element).ApplicationName;
		}
	}
}
