namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class BalanceInquiryReceipt
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string Currency { get; set; }
		public double? AvailableBalance { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}