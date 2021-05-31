namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class ChequeBookRequestReceipt
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}