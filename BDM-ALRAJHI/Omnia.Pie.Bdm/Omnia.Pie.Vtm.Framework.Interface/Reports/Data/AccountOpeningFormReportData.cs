namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	using System.Windows.Media.Imaging;

	public class AccountOpeningFormReportData
	{
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string AccountType { get; set; }
		public string AccountCurrency { get; set; }
		public bool IsChequeBook { get; set; }
		public BitmapSource Signature1 { get; set; }
		public BitmapSource Signature2 { get; set; }
		public string CheckedById { get; set; }
		public string CheckedByName { get; set; }
	}
}