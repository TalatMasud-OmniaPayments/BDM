namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System.Collections.Generic;

	public class ClearChequesReceipt
	{
		public bool IsView { get; set; }
		public List<ChequeUnit> Units { get; set; }
	}

	public class ChequeUnit
	{
		public string Name { get; set; }
		public int? Count { get; set; }
	}
}