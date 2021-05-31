namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CashWithdrawalUsingOffusCardReceipt
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
		public string AuthCode { get; set; }
		public string TransactionCurrency { get; set; }
		public double? TransactionAmount { get; set; }
		public string AvailableBalanceCurrency { get; set; }
		public double? AvailableBalance { get; set; }
		public string ApplicationId { get; set; }
		public string ApplicationLabel { get; set; }
		public bool? IsUaeSwitchTransaction { get; set; }
		public bool IsInsufficientBalance { get; set; }
		public bool IsCardCaptured { get; set; }
	}
}