namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class ViewAddCashReceipt
	{
		public List<CashOutUnit> Units { get; set; }
	}

	public class CashOutUnit
	{
		public int Index { get; set; }
		public string Currency { get; set; }
		public int Denomination { get; set; }
		public int Count { get; set; }
		public int Rejected { get; set; }
		public int Remaining { get; set; }
		public int Dispensed { get; set; }
		public int Total { get; set; }
	}

	public class ViewAddCoinReceipt
	{
		public List<CoinOutUnit> Units { get; set; }
	}

	public class CoinOutUnit
	{
		public int Index { get; set; }
		public string Currency { get; set; }
		public int Denomination { get; set; }
		public int Count { get; set; }
		public int Rejected { get; set; }
		public int Remaining { get; set; }
		public int Dispensed { get; set; }
		public int Total { get; set; }
	}
}