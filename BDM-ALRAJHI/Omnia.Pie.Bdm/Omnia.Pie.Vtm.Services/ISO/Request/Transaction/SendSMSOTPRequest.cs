namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class SendSMSOTPRequest : RequestBase
	{
		public string CustomerIdentifier { get; set; }
		public string SMSExpiryTime { get; set; }
	}
}