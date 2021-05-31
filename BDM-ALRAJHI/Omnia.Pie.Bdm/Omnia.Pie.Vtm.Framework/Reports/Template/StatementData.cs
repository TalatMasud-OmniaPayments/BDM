namespace Omnia.Pie.Vtm.Framework.Reports.Template
{
	using System;

	public class StatementData
	{
		public string AccountNumber { get; set; }
		public string AccountIban { get; set; }
		public string AccountType { get; set; }
		public string AccountCurrency { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string NumberOfMonths { get; set; }
		public string BranchLocation { get; set; }
		public string CustomerName { get; set; }
		public string POBox { get; set; }
		public string City { get; set; }
	}
}