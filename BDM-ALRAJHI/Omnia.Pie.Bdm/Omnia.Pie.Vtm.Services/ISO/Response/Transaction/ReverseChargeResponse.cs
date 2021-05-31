namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class ReverseChargesRsp
	{
		public string ReferenceNumber { get; set; }
	}

	public class ReverseChargeResponse : ResponseBase<ReverseChargesRsp>
	{
		public ReverseChargesRsp ReverseChargesRsp { get; set; }
	}
}