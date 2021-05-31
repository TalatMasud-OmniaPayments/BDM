namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public static class TopVideoConfiguration
	{
		private static TopVideoSection Section => (TopVideoSection)ConfigurationManager.GetSection(TopVideoSection.Name);

		public static string SourceFolder => Section.SourceFolder;
	}

	public class TopVideoSection : ConfigurationSection
	{
		public const string Name = "topVideo";

		[ConfigurationProperty("sourceFolder", IsRequired = true)]
		public string SourceFolder => (string)base["sourceFolder"];
	}
}
