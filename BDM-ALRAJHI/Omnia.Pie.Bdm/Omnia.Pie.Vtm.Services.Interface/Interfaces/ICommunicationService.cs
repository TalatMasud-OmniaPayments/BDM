namespace Omnia.Pie.Vtm.Services.Interface.Interfaces
{
	using Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService;
	using System;
	using System.Threading.Tasks;

	public interface ICommunicationService
	{
		Task<RegisterCallResult> RegisterCallAsync(string callId, string callStartTime);
		Task<UpdateCallRecordResult> UpdateCallRecordAsync(string customerIdentifier);
		Task<SendEmailResult> SendEmailAsync(Attachment attachment, EmailType emailType, string cutomerId, string customerName, DateTime requestDate, string toEmail, string language, string Iban);
		Task<GenerateTSNResult> GenerateTSNAsync();
		Task<SendSMSResult> SendSmsAsync(string customerIdentifier, SmsType type, string referenceNumber, string Iban);
	}
}