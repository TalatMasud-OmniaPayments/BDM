namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class DeliverChequeBookRequest : RequestBase
	{
		public string HostTransCode { get; set; }
		public string AccountBranchCode { get; set; }
	}
}