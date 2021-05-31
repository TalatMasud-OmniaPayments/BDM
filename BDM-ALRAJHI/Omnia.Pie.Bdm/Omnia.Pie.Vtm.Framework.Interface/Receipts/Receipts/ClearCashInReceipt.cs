namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class ClearCashInReceipt
	{
		public bool IsView { get; set; }
		public List<CashInUnit> CashInUnits { get; set; }
		public List<DenominationRecord> DenominationRecords { get; set; }
	}

	public class CashInUnit
	{
		public string Name { get; set; }
		public string Currency { get; set; }
		public int? Count { get; set; }
	}

	public class DenominationRecord
	{
		public int Value { get; set; }
		public int? Count { get; set; }
		public int? Retracted { get; set; }
		public int? Rejected { get; set; }
		public int? Total { get; set; }
	}
}