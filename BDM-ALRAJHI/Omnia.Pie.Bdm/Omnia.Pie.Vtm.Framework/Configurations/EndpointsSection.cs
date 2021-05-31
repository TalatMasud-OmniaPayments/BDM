namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class EndpointElement : ConfigurationElement
	{
		[ConfigurationProperty("contract", IsKey = true, IsRequired = true)]
		public string Contract => (string)base["contract"];

		[ConfigurationProperty("address", IsKey = true, IsRequired = true)]
		public string Address => (string)base["address"];
	}

	[ConfigurationCollection(typeof(EndpointElement), AddItemName = "endpoint")]
	public class EndpointElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new EndpointElement();

		protected override object GetElementKey(ConfigurationElement element) => ((EndpointElement)element).Contract;
	}

	public class EndpointsSection : ConfigurationSection
	{
		public const string Name = "endpoints";

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public EndpointElementCollection Elements => (EndpointElementCollection)base[""];

		[ConfigurationProperty("baseAddress")]
		public string BaseAddress => (string)base["baseAddress"];

		[ConfigurationProperty("timeoutSeconds", DefaultValue = 60)]
		public int TimeoutSeconds => (int)base["timeoutSeconds"];

	}
}
