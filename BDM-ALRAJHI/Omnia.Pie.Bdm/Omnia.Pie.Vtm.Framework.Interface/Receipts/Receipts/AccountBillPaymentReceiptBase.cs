namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public abstract class AccountBillPaymentReceiptBase
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string ConsumerNumber { get; set; }
		public string Currency { get; set; }
		public double? AmountDue { get; set; }
		public double? TransactionAmount { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}