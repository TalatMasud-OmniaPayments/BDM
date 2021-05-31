namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CreditCardPaymentReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CreditCardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string ReferenceNumber { get; set; }
		public string Currency { get; set; }
		public double? Amount { get; set; }
		public double? AvailableBalance { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}