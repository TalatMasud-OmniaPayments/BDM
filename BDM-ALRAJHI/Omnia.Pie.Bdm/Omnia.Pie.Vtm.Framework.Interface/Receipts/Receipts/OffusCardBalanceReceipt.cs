namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class OffusCardBalanceReceipt
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string Currency { get; set; }
		public double? AvailableBalance { get; set; }
		public string ApplicationId { get; set; }
		public string ApplicationLabel { get; set; }
		public bool? IsUaeSwitchTransaction { get; set; }
		public bool IsCardCaptured { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}