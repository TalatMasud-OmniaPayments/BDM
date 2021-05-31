namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class ChequeDepositToCreditCardReceiptFooter
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string CustomerName { get; set; }
		public string Micr { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}