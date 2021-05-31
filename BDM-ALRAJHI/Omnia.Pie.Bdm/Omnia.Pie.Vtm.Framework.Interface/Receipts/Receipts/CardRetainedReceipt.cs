namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class CardRetainedReceipt
	{
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public bool IsCardCaptured { get; set; }
	}
}