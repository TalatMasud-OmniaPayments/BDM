namespace Omnia.Pie.Vtm.Services.ISO.Response.CommunicationService
{
	public class SendEmail
	{
		public string Status { get; set; }
		public string EmailSessionId { get; set; }
		public string ReferenceNumber { get; set; }
	}

	public class SendEmailResponse : ResponseBase<SendEmail>
	{
		public SendEmail EmailResponse { get; set; }
	}
}