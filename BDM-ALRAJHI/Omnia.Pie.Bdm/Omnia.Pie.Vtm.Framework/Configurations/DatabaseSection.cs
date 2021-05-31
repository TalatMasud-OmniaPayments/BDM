namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class DatabaseSection : ConfigurationSection
	{
		public const string Name = "database";

		[ConfigurationProperty("path", IsRequired = true)]
		public string Path => (string)base["path"];
	}
}