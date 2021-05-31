namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class AddCashReceipt
	{
		public List<AddCashUnit> Units { get; set; }
	}

	public class AddCashUnit
	{
		public string Name { get; set; }
		public string Currency { get; set; }
		public int? Denomination { get; set; }
		public int? Count { get; set; }
	}

	public class AddCoinReceipt
	{
		public List<AddCoinUnit> Units { get; set; }
	}

	public class AddCoinUnit
	{
		public string Name { get; set; }
		public string Currency { get; set; }
		public int? Denomination { get; set; }
		public int? Count { get; set; }
	}
}