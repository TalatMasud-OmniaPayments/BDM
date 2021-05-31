namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class GetChequePrintingChargeRequest : RequestBase
	{
		public string ChargeIndicator { get; set; }
		public string AccountNumber { get; set; }
		public string NumberOfLeafs { get; set; }
	}
}