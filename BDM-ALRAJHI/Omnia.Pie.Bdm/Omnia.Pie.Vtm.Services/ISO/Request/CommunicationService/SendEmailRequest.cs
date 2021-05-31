namespace Omnia.Pie.Vtm.Services.ISO.Request.CommunicationService
{
	using Omnia.Pie.Vtm.Services.Interface;
	using System;

	public class SendEmailRequest : RequestBase
	{
		public Attachment Attachment { get; set; }
		public string EmailTypeCode { get; set; }
		public string CustomerIdentifier { get; set; }
		public string CustomerName { get; set; }
		public DateTime RequestDate { get; set; }
		public string ToEmail { get; set; }
		public string Language { get; set; }
		public string Iban { get; set; }
	}
}