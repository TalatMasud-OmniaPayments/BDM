namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class RequestLCReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public string AuthCode { get; set; }
		public string AccountCurrency { get; set; }
		public double? AvailableBalance { get; set; }
		public string TransactionCurrency { get; set; }
		public double? TransactionAmount { get; set; }
		public string ReferenceNo { get; set; }
		public string VTMID { get; set; }
	}
}
