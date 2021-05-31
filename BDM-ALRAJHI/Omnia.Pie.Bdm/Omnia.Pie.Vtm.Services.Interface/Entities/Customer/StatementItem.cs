namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	using System;

	public class StatementItem
	{
		public DateTime? TransactionDate { get; set; }
		public DateTime? ValueDate { get; set; }
		public string ReferenceNumber { get; set; }
		public string Description { get; set; }
		public double? CreditAmount { get; set; }
		public double? DebitAmount { get; set; }
		public double? RunningBalance { get; set; }
	}
}