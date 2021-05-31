namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class ChequeDepositToAccountReceiptFooter
	{
		public string AccountNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string CustomerName { get; set; }
		public string Micr { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public bool SelfService { get; set; }
		public string SourceAccountNumber { get; set; }
	}
}