namespace Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService
{
	public class SendEmailResult
	{
		public string Status { get; set; }
		public string EmailSessionId { get; set; }
		public string ReferenceNumber { get; set; }
	}
}