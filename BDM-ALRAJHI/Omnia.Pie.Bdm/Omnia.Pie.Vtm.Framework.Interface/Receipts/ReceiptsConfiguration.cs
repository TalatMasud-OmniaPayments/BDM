namespace Omnia.Pie.Vtm.Framework.Interface.Configuration
{
	using System.Configuration;

	public static class ReceiptsConfiguration
	{
		private static ReceiptsSection ReceiptsSection => (ReceiptsSection)ConfigurationManager.GetSection(ReceiptsSection.Name);

		public static int OutputWidth => ReceiptsSection.OutputWidth;
		public static string DateFormat => ReceiptsSection.DateFormat;
	}

	public class ReceiptsSection : ConfigurationSection
	{
		public const string Name = "receipts";

		[ConfigurationProperty("outputWidth", DefaultValue = 40)]
		public int OutputWidth => (int)base["outputWidth"];

		[ConfigurationProperty("dateFormat", DefaultValue = "dd/MM/yyyy")]
		public string DateFormat => (string)base["dateFormat"];
	}
}