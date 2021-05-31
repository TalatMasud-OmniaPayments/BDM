namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System;
	using System.Configuration;
	using System.Drawing.Printing;
	using System.Linq;

	public class ReportElement : ConfigurationElement
	{
		[ConfigurationProperty("dataType", IsKey = true, IsRequired = true)]
		public string DataType => (string)base["dataType"];

		[ConfigurationProperty("reportType", IsKey = true, IsRequired = true)]
		public string ReportType => (string)base["reportType"];
	}

	[ConfigurationCollection(typeof(ReportElement), AddItemName = "report")]
	public class ReportElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new ReportElement();

		protected override object GetElementKey(ConfigurationElement element) => ((ReportElement)element).ReportType;
	}

	public class ReportsSection : ConfigurationSection
	{
		public const string Name = "reports";

		[ConfigurationProperty("dateFormat", DefaultValue = "dd/MM/yyyy")]
		public string DateFormat => (string)base["dateFormat"];

		[ConfigurationProperty("pageSize", DefaultValue = "8.267in 11.692in")] // Width Height
		public string PageSize => (string)base["pageSize"];

		[ConfigurationProperty("pageMargin", DefaultValue = "0.0in 0.0in 0.0in 0.0in")] // Left Top Right Bottom
		public string PageMargin => (string)base["pageMargin"];

		[ConfigurationProperty("printerName", DefaultValue = "default")]
		public string PrinterName => (string)base["printerName"];

		[ConfigurationProperty("paperKind", DefaultValue = "A4")]
		public string PaperKind => (string)base["paperKind"];

		[ConfigurationProperty("paperMargin", DefaultValue = "0 0 0 0")] // Left Top Right Bottom
		public string PaperMargin => (string)base["paperMargin"];

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ReportElementCollection Elements => (ReportElementCollection)base[""];
	}

	internal static class ReportsConfiguration
	{
		private static ReportsSection ReportsSection => (ReportsSection)ConfigurationManager.GetSection(ReportsSection.Name);

		public static string DateFormat => ReportsSection.DateFormat;

		public static string PageSize => ReportsSection.PageSize;

		public static string PageMargin => ReportsSection.PageMargin;

		public static string PrinterName => ReportsSection.PrinterName;

		public static PaperKind PaperKind => (PaperKind)Enum.Parse(typeof(PaperKind), ReportsSection.PaperKind);

		public static Margins PaperMargin
		{
			get
			{
				var elements = ReportsSection
											.PaperMargin
											.Split()
											.Select(i => int.Parse(i))
											.ToArray();

				return new Margins(elements[0], elements[2], elements[1], elements[3]);
			}
		}

		public static string GetReportTypeName(string dataTypeName)
		{
			return ReportsSection
								.Elements.Cast<ReportElement>()
								.Where(e => e.DataType == dataTypeName)
								.Select(e => e.ReportType)
								.Single();
		}
	}
}