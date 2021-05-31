namespace Omnia.Pie.Vtm.Services.ISO.Request.CommunicationService
{
	public class SendSMSRequest : RequestBase
	{
		public string CustomerIdentifier { get; set; }
		public string Type { get; set; }
		public string ReferenceNumber { get; set; }
		public string IBAN { get; set; }
	}
}