namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	public class LCAndNLCReportData
	{
		public string TypeofRequest { get; set; }
		public string TransactionNo { get; set; }
		public string CIF { get; set; }
		public string SignatureBase64Content { get; set; }
		public string CustomerName { get; set; }
	}
}