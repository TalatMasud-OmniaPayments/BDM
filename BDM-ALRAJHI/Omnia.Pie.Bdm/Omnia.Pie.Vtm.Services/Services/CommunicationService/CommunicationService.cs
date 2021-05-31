using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService;
using Omnia.Pie.Vtm.Services.Interface.Interfaces;
using Omnia.Pie.Vtm.Services.ISO.Request.CommunicationService;
using Omnia.Pie.Vtm.Services.ISO.Response.CommunicationService;
using System;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services
{
	public class CommunicationService : ServiceBase, ICommunicationService
	{
		public CommunicationService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
		{

		}

		#region "Get Register Call"

		public async Task<RegisterCallResult> RegisterCallAsync(string callId, string callStartTime)
		{
			if (string.IsNullOrEmpty(callId)) throw new ArgumentNullException(nameof(callId));
			if (string.IsNullOrEmpty(callStartTime)) throw new ArgumentNullException(nameof(callStartTime));

			return await ExecuteFaultHandledOperationAsync<RegisterCallRequest, RegisterCallResult>(async c =>
			{
				var response = await RegisterCallAsync(ToRegisterCallDetailsRequest(callId, callStartTime));
				return ToRegisterCallResponse(response);
			});
		}

		private async Task<RegisterCallResponse> RegisterCallAsync(RegisterCallRequest request)
			=> await ExecuteServiceAsync<RegisterCallRequest, RegisterCallResponse>(request);

		private RegisterCallRequest ToRegisterCallDetailsRequest(string callId, string callStartTime) => new RegisterCallRequest
		{
			CallId = callId,
			CallStartTime = callStartTime,
		};

		private RegisterCallResult ToRegisterCallResponse(RegisterCallResponse response) => new RegisterCallResult
		{
			SessionId = response?.RegisterCall?.SessionId,
			LogStatusCode = response?.RegisterCall?.LogStatusCode,
			LogStatusMessage = response?.RegisterCall?.LogStatusMessage,
		};

		#endregion

		#region "Get Update Call Record"

		public async Task<UpdateCallRecordResult> UpdateCallRecordAsync(string customerIdentifier)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));

			return await ExecuteFaultHandledOperationAsync<UpdateCallRecordRequest, UpdateCallRecordResult>(async c =>
			{
				var response = await UpdateCallRecordAsync(ToUpdateCallDetailsRequest(customerIdentifier));
				return ToUpdateCallRecord(response);
			});
		}

		private async Task<UpdateCallRecordResponse> UpdateCallRecordAsync(UpdateCallRecordRequest request)
			=> await ExecuteServiceAsync<UpdateCallRecordRequest, UpdateCallRecordResponse>(request);

		private UpdateCallRecordRequest ToUpdateCallDetailsRequest(string customerIdentifier) => new UpdateCallRecordRequest
		{
			CustomerIdentifier = customerIdentifier,
		};

		private UpdateCallRecordResult ToUpdateCallRecord(UpdateCallRecordResponse response) => new UpdateCallRecordResult
		{
			ResponseCode = response?.ResponseCode,
		};

		#endregion

		#region "Send Email With Attachment"

		public async Task<SendEmailResult> SendEmailAsync(Attachment attachment, EmailType emailType, string cutomerId, string customerName, DateTime requestDate, string toEmail, string language, string iban)
		{
			if (attachment == null) throw new ArgumentNullException(nameof(attachment));
			if (string.IsNullOrEmpty(cutomerId)) throw new ArgumentNullException(nameof(cutomerId));
			if (string.IsNullOrEmpty(customerName)) throw new ArgumentNullException(nameof(customerName));
			if (DateTime.MinValue.Equals(requestDate)) throw new ArgumentNullException(nameof(requestDate));

			return await ExecuteFaultHandledOperationAsync<SendEmailRequest, SendEmailResult>(async c =>
			{
				var response = await SendEmailAsync(ToSendEmailRequest(attachment, emailType, cutomerId, customerName, requestDate, toEmail, language, iban));
				return ToSendEmail(response);
			});
		}

		private async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request) =>
			await ExecuteServiceAsync<SendEmailRequest, SendEmailResponse>(request);

		private SendEmailRequest ToSendEmailRequest(Attachment attachment, EmailType emailType, string cutomerId, string customerName, DateTime requestDate, string toEmail, string language, string iban) => new SendEmailRequest
		{
			Attachment = attachment,
			CustomerIdentifier = cutomerId,
			CustomerName = customerName,
			EmailTypeCode = ((int)emailType).ToString(),
			RequestDate = requestDate,
			ToEmail = toEmail,
			Language = language,
			Iban = iban,
		};

		private SendEmailResult ToSendEmail(SendEmailResponse response) => new SendEmailResult
		{
			EmailSessionId = response?.EmailResponse?.EmailSessionId,
			Status = response?.EmailResponse?.Status,
			ReferenceNumber = response?.EmailResponse?.ReferenceNumber,
		};

		#endregion

		#region "Generate TSN"

		public async Task<GenerateTSNResult> GenerateTSNAsync()
		{
			return await ExecuteFaultHandledOperationAsync<GenerateTSNRequest, GenerateTSNResult>(async c =>
			{
				var response = await GenerateTSNAsync(ToGenerateTSNRequest());
				return ToGenerateTSN(response);
			});
		}

		private async Task<GenerateTSNResponse> GenerateTSNAsync(GenerateTSNRequest request) =>
			await ExecuteServiceAsync<GenerateTSNRequest, GenerateTSNResponse>(request);

		private GenerateTSNRequest ToGenerateTSNRequest() => new GenerateTSNRequest
		{

		};

		private GenerateTSNResult ToGenerateTSN(GenerateTSNResponse response) => new GenerateTSNResult
		{
			value = response?.TSN?.value
		};

		#endregion

		#region "Send SMS"

		public async Task<SendSMSResult> SendSmsAsync(string customerIdentifier, SmsType type, string referenceNumber, string IBAN)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));
			if (string.IsNullOrEmpty(referenceNumber)) throw new ArgumentNullException(nameof(referenceNumber));
			if (type == SmsType.IbanSms && string.IsNullOrEmpty(IBAN)) throw new ArgumentNullException(nameof(IBAN));

			return await ExecuteFaultHandledOperationAsync<SendSMSRequest, SendSMSResult>(async c =>
			{
				var response = await SendSMSAsync(ToSendSMSRequest(customerIdentifier, type, referenceNumber, IBAN));
				return ToSendSMS(response);
			});
		}

		private async Task<SendSMSResponse> SendSMSAsync(SendSMSRequest request)
			=> await ExecuteServiceAsync<SendSMSRequest, SendSMSResponse>(request);

		private SendSMSRequest ToSendSMSRequest(string customerIdentifier, SmsType type, string referenceNumber, string IBAN) => new SendSMSRequest
		{
			CustomerIdentifier = customerIdentifier,
			Type = ((int)type).ToString(),
			ReferenceNumber = referenceNumber,
			IBAN = IBAN
		};

		private SendSMSResult ToSendSMS(SendSMSResponse response) => new SendSMSResult
		{
			ReferenceNumber = response?.SMS?.ReferenceNumber
		};

		#endregion

	}
}