namespace Omnia.Pie.Vtm.Framework.Interface
{
	using System.Globalization;

	public class ReceiptFormattingOptions
	{
		public CultureInfo Culture { get; set; }
		public bool IsMarkupEnabled { get; set; }
	}
}