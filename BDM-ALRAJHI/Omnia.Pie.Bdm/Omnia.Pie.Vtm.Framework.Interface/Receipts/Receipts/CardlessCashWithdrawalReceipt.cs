namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CardlessCashWithdrawalReceipt
	{
		public string MobileNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string AuthCode { get; set; }
		public string TransactionCurrency { get; set; }
		public double? TransactionAmount { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}