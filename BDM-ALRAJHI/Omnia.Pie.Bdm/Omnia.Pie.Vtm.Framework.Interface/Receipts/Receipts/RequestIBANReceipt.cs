namespace Omnia.Pie.Vtm.Framework.Interface.Receipts.Receipts
{
	public class RequestIBANReceipt
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string Currency { get; set; }
		public double? AvailableBalance { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public string ReferenceNo { get; set; }
		public string VTMID { get; set; }
	}
}