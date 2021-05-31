namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;
	using System.Linq;

	public static class BootstrapperConfiguration
	{
		private static BootstrapperSection BootstrapperSection => (BootstrapperSection)ConfigurationManager.GetSection(BootstrapperSection.Name);

		public static string DateFormat => BootstrapperSection.DateFormat;

		public static string GetViewTypeName(string viewModelTypeName)
		{
			return BootstrapperSection.Elements.Cast<MapElement>()
			.Where(e => e.ViewModel == viewModelTypeName)
			.Select(e => e.View)
			.Single();
		}
	}

	public class MapElement : ConfigurationElement
	{
		[ConfigurationProperty("viewModel", IsKey = true, IsRequired = true)]
		public string ViewModel => (string)base["viewModel"];

		[ConfigurationProperty("view", IsRequired = true)]
		public string View => (string)base["view"];
	}

	[ConfigurationCollection(typeof(MapElement), AddItemName = "map")]
	public class MapElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new MapElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MapElement)element).ViewModel;
		}
	}

	public class BootstrapperSection : ConfigurationSection
	{
		public const string Name = "bootstrapper";

		[ConfigurationProperty("dateFormat", DefaultValue = "yyyy/MM/dd")]
		public string DateFormat => (string)base["dateFormat"];

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public MapElementCollection Elements => (MapElementCollection)base[""];
	}
}