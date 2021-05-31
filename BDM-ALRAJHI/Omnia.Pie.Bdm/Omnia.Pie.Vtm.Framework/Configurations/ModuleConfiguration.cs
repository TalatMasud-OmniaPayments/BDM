namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	public static class ModuleConfiguration
	{
		private static ModulesSection Section => (ModulesSection)ConfigurationManager.GetSection(ModulesSection.Name);

		public static List<string> Modules => Section
			.Elements.Cast<ModuleElement>()
			.Select(m => m.ModuleType)
			.ToList();
	}

	public class ModuleElement : ConfigurationElement
	{
		[ConfigurationProperty("moduleType", IsKey = true, IsRequired = true)]
		public string ModuleType => (string)base["moduleType"];
	}

	[ConfigurationCollection(typeof(ModuleElement), AddItemName = "module")]
	public class ModuleElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new ModuleElement();

		protected override object GetElementKey(ConfigurationElement element) => ((ModuleElement)element).ModuleType;
	}

	public class ModulesSection : ConfigurationSection
	{
		public const string Name = "modules";

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ModuleElementCollection Elements => (ModuleElementCollection)base[""];
	}
}