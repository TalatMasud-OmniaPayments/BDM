namespace Omnia.Pie.Vtm.Services.ISO.Request.CommunicationService
{
	public class RegisterCallRequest : RequestBase
	{
		public string CallId { get; set; }
		public string CallStartTime { get; set; }
	}
}