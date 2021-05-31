namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CashWithdrawalUsingCreditCardReceipt
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public string AuthCode { get; set; }
		public string TransactionCurrency { get; set; }
		public double? TransactionAmount { get; set; }
	}
}