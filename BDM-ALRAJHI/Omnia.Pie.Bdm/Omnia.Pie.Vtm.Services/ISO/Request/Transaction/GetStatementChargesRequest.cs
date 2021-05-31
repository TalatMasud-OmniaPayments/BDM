namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class GetStatementChargesRequest : RequestBase
	{
		public string StatementDateFrom { get; set; }
		public string StatementDateTo { get; set; }
		public string ChargeIndicator { get; set; }
		public string AccountNumber { get; set; }
		public string Category { get; set; }
		public string NumberOfMonths { get; set; }
	}
}