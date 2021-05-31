namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class ClearCardsReceipt
	{
		public bool IsView { get; set; }
		public List<CardUnit> Units { get; set; }
	}

	public class CardUnit
	{
		public string Name { get; set; }
		public int? Count { get; set; }
	}
}