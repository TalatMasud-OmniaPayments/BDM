namespace Omnia.Pie.Vtm.Services.ISO.Response.CommunicationService
{
	public class SMS
	{
		public string ReferenceNumber { get; set; }
	}

	public class SendSMSResponse : ResponseBase<SMS>
	{
		public SMS SMS { get; set; }
	}
}