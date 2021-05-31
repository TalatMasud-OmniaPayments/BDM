namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	using System;
	using System.Collections.Generic;

	public class StatementReportData
	{
		public string AccountNumber { get; set; }
		public string AccountIban { get; set; }
		public string AccountType { get; set; }
		public string AccountCurrency { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<StatementItem> Items { get; set; }
		public string NumberOfMonths { get; set; }
		public string BranchLocation { get; set; }
		public string CustomerName { get; set; }
		public string POBox { get; set;}
		public string City { get; set; }
	}

	public class StatementItem
	{
		public DateTime? PostingDate { get; set; }
		public DateTime? ValueDate { get; set; }
		public string Description { get; set; }
		public double? DebitAmount { get; set; }
		public double? CreditAmount { get; set; }
		//public double? RunningBalance { get; set; }
	}
}