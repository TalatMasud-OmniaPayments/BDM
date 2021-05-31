namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class IssueCheckBook
	{
		public string RoutingCode { get; set; }
		public string HostTransCode { get; set; }
		public string StartingChequeNumber { get; set; }
		public string TransactionReferenceNumber { get; set; }
	}

	public class IssueCheckBookResponse : ResponseBase<IssueCheckBook>
	{
		public IssueCheckBook ChequeBookIssuance { get; set; }
	}
}