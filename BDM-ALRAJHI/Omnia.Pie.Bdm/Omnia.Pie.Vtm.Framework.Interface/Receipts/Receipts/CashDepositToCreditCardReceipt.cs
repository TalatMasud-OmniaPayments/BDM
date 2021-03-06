namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;
	using System.Linq;

	public class CashDepositToCreditCardReceipt
	{
		public string CardNumber { get; set; }
		public string CustomerName { get; set; }
		public string TransactionNumber { get; set; }
		public string AuthCode { get; set; }
		public string Currency { get; set; }
		public List<Denomination> Denominations { get; set; }
		public TransactionStatus TransactionStatus { get; set; }

		public int TotalAmount => Denominations?.Sum(d => d.Value * d.Count) ?? 0;
	}
}