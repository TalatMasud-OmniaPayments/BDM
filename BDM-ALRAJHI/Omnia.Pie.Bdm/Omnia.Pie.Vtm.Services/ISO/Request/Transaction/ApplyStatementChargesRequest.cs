namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class ApplyStatementChargesRequest : RequestBase
	{
		public string StatementDateFrom { get; set; }
		public string StatementDateTo { get; set; }
		public string BranchCode { get; set; }
		public string AccountNumber { get; set; }
		public string CustomerId { get; set; }
	}
}