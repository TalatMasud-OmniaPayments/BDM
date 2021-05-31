using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class StatementItem
	{
		public string TransactionReferenceNum { get; set; }
		public string TransactionDate { get; set; }
		public string TransactionNarration { get; set; }
		public string TransactionAmount { get; set; }
		public string TransactionEffect { get; set; }
		public string RunningBalance { get; set; }
		public string ValueDate { get; set; }
	}

	public class StatementItemResponse : ResponseBase<List<StatementItem>>
	{
		public List<StatementItem> TransactionHistory { get; set; }
        public List<StatementItem> Transactions { get; set; }
    }
}