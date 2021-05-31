namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class ClearAddCashReceipt
	{
		public List<ClearAddCashUnit> Units { get; set; }
	}

	public class ClearAddCashUnit
	{
		public string Name { get; set; }
		public string Currency { get; set; }
		public int? Denomination { get; set; }
		public int? Count { get; set; }
	}

	public class ClearAddCoinReceipt
	{
		public List<ClearAddCoinUnit> Units { get; set; }
	}

	public class ClearAddCoinUnit
	{
		public string Name { get; set; }
		public string Currency { get; set; }
		public int? Denomination { get; set; }
		public int? Count { get; set; }
	}
}