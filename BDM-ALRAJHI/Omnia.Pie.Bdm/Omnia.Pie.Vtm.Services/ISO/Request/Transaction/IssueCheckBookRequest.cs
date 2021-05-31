namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class IssueCheckBookRequest : RequestBase
	{
		public string AccountBranch { get; set; }
		public string AccountNumber { get; set; }
		public string CurrencyCode { get; set; }
		public string IssueDate { get; set; }
		public string NumberOfCheques { get; set; }
	}
}