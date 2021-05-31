namespace Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService
{
	public class RegisterCallResult
	{
		public string CallId { get; set; }
		public string CallStartTime { get; set; }
		public string LogStatusMessage { get; set; }
		public string LogStatusCode { get; set; }
		public string SessionId { get; set; }
	}
}