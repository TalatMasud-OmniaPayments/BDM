namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;
	using System.Linq;

	public class CashInReceipt
	{
		public string Type { get; set; }
		public List<OldCashInUnit> Units { get; set; }

		public int TotalAmount => Units.Sum(u => u.TotalAmount);
	}

	public class OldCashInUnit
	{
		public int Index { get; set; }
		public string Currency { get; set; }
		public int Denomination { get; set; }
		public int Count { get; set; }
		public int Retracted { get; set; }

		public int Total => Count + Retracted;
		public int TotalAmount => (Count + Retracted) * Denomination;
	}
}