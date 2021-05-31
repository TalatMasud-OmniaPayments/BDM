namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class ChequePrintingReceipt
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string AccountCurrency { get; set; }
		public string AvailableBalance { get; set; }
		public string NumberOfLeaves { get; set; }
		public string Charges { get; set; }
		public string TransactionNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}