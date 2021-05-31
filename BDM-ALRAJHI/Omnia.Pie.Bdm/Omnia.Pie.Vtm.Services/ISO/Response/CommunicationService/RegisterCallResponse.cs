namespace Omnia.Pie.Vtm.Services.ISO.Response.CommunicationService
{
	public class RegisterCall
	{
		public string LogStatusMessage { get; set; }
		public string LogStatusCode { get; set; }
		public string SessionId { get; set; }
	}

	public class RegisterCallResponse : ResponseBase<RegisterCall>
	{
		public RegisterCall RegisterCall { get; set; }
	}
}