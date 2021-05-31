namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
	public class StatementItemRequest : RequestBase
	{
		public string StatementDateFrom { get; set; }
		public string StatementDateTo { get; set; }
		public string AccountNumber { get; set; }
		public string NumOfTransactions { get; set; }
	}
}