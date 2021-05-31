namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class ReverseChargeRequest : RequestBase
	{
		public string TransactionReferenceNumber { get; set; }
		public string ChargeType { get; set; }
	}
}