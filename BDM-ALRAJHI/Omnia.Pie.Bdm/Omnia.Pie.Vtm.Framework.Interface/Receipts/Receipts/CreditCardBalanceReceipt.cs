namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CreditCardBalanceReceipt
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string Currency { get; set; }
		public double? AvailableLimit { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}