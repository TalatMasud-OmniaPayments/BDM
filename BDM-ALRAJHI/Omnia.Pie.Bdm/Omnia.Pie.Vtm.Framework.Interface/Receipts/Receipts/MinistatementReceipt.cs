namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System;
	using System.Collections.Generic;

	public class MinistatementReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string CustomerName { get; set; }
		public string TransactionNumber { get; set; }
		public string Currency { get; set; }
		public List<MinistatementItem> Items { get; set; }
		public double? AvailableBalance { get; set; }
	}

	public class MinistatementItem
	{
		public DateTime? PostingDate { get; set; }
		public string Description { get; set; }
		public double? CreditAmount { get; set; }
		public double? DebitAmount { get; set; }
	}
}