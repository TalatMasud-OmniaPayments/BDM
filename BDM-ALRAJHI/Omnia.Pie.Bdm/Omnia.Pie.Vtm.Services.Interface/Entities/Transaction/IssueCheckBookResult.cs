namespace Omnia.Pie.Vtm.Services.Interface.Entities.Transaction
{
	public class IssueCheckBookResult
	{
		public string RoutingCode { get; set; }
		public string HostTransCode { get; set; }
		public string StartingChequeNumber { get; set; }
		public string ReferenceNumber { get; set; }
	}
}