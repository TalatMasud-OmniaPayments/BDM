namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class TransferFundsReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string CustomerName { get; set; }
		public string TransactionNumber { get; set; }
		public string ReferenceNumber { get; set; }
		public string SourceAccountCurrency { get; set; }
		public string SourceAccountNumber { get; set; }
		public string TargetAccountNumber { get; set; }
		public double? TransactionAmount { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}
