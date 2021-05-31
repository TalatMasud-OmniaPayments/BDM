namespace Omnia.Pie.Vtm.Services
{
    using Omnia.Pie.Vtm.Framework.Configurations;
    using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.ISO.Authentication;
	using Omnia.Pie.Vtm.Services.ISO.Request.Authentication;
	using Omnia.Pie.Vtm.Services.ISO.Request.Transaction;
	using Omnia.Pie.Vtm.Services.ISO.Response.Authentication;
	using Omnia.Pie.Vtm.Services.ISO.Response.Transaction;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class AuthenticationService : ServiceBase, IAuthenticationService
	{
		public AuthenticationService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
		{
            
		}

		#region Pin Verification

		public async Task<PinVerificationResult> VerifyPin(string track2, string pin, string iccRequest)
		{
			if (string.IsNullOrEmpty(track2)) throw new ArgumentNullException(nameof(track2));
			if (string.IsNullOrEmpty(pin)) throw new ArgumentNullException(nameof(pin));

			return await ExecuteFaultHandledOperationAsync<PinVerificationRequest, PinVerificationResult>(async c =>
			{
				var response = await VerifyPinAsync(ToDocumentVTMRetrieveAccountDetailsRequest(track2, pin, iccRequest));
				return ToPinVerification(response);
			});
		}

		private async Task<PinVerificationResponse> VerifyPinAsync(PinVerificationRequest request)
		{
			return await ExecuteServiceAsync<PinVerificationRequest, PinVerificationResponse>(request);
		}

		private PinVerificationRequest ToDocumentVTMRetrieveAccountDetailsRequest(string track2, string pin, string iccRequest) => new PinVerificationRequest
		{
			Track2 = track2,
			Pin = pin,
			IccRequest = iccRequest,
		};

		private PinVerificationResult ToPinVerification(PinVerificationResponse response) => new PinVerificationResult
		{
			CustomerIdentifier = response?.PinVerification?.CustomerIdentifier,
			AuthCode = response?.PinVerification?.AuthCode,
			IccResponse = response?.PinVerification?.IccResponse,
			ResponseCode = response?.ResponseCode,
		};

        #endregion

        #region Get Accounts

        public async Task<List<Interface.Entities.Account>> GetAccounts(string customerIdentifier, string Username, string TransactionNumber, AccountCriterion conditionId)
        {
            if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));

            return await ExecuteFaultHandledOperationAsync<AccountsRequest, List<Interface.Entities.Account>>(async c =>
            {
                ServiceManager.TransactionNumber = TransactionNumber;
                var response = await GetAccounts(ToAccountsRequest(customerIdentifier, Username, conditionId));
                return ToAccountList(response);
            });
        }
        public async Task<List<Interface.Entities.Account>> GetAccounts(string customerIdentifier, string Username, AccountCriterion conditionId)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));

            return await ExecuteFaultHandledOperationAsync<AccountsRequest, List<Interface.Entities.Account>>(async c =>
			{
                ServiceManager.TransactionNumber = "";
                var response = await GetAccounts(ToAccountsRequest(customerIdentifier,Username, conditionId));
				return ToAccountList(response);
			});
		}

		private async Task<AccountsResponse> GetAccounts(AccountsRequest request)
		{
			return await ExecuteServiceAsync<AccountsRequest, AccountsResponse>(request);
		}

		private AccountsRequest ToAccountsRequest(string customerIdentifier, string Username, AccountCriterion conditionId) => new AccountsRequest
		{
			CustomerIdentifier = customerIdentifier,
            Username = Username,
			ConditionId = conditionId,
		};

		public List<Interface.Entities.Account> ToAccountList(AccountsResponse res) => res.Accounts?.ConvertAll(ToAccount);

		private Interface.Entities.Account ToAccount(ISO.Authentication.Account response) => new Interface.Entities.Account
		{
			Name = response?.Nickname,
			Number = response?.AccountNo,
			Type = response?.AccountType,
            Logo = response?.Logo,
            Currency = TerminalConfiguration.Section.Currency,
            CurrencyCode = TerminalConfiguration.Section.CurrencyCode,
            Branch = response?.Branch
        };

		#endregion

		#region Send SMS OTP

		public async Task<SendSMSOTPResult> SendSmsOtp(string customerIdentifier, string sMSExpiryTime)
		{
			if (string.IsNullOrEmpty(customerIdentifier)) throw new ArgumentNullException(nameof(customerIdentifier));
			if (string.IsNullOrEmpty(sMSExpiryTime)) throw new ArgumentNullException(nameof(sMSExpiryTime));

			return await ExecuteFaultHandledOperationAsync<SendSMSOTPRequest, SendSMSOTPResult>(async c =>
			{
				var response = await SendSmsOtpAsync(ToSendSMSOTPDetailsRequest(customerIdentifier, sMSExpiryTime));
				return ToSendSMSOTP(response);
			});
		}

		private async Task<SendSMSOTPResponse> SendSmsOtpAsync(SendSMSOTPRequest request) =>
			await ExecuteServiceAsync<SendSMSOTPRequest, SendSMSOTPResponse>(request);

		private SendSMSOTPRequest ToSendSMSOTPDetailsRequest(string customerIdentifier, string sMSExpiryTime) => new SendSMSOTPRequest
		{
			CustomerIdentifier = customerIdentifier,
			SMSExpiryTime = sMSExpiryTime,
		};

		private SendSMSOTPResult ToSendSMSOTP(SendSMSOTPResponse response) => new SendSMSOTPResult
		{
			Otp = response?.SendSmsOtp?.Otp,
			Uuid = response?.SendSmsOtp?.Uuid,
		};

		#endregion

		#region Validate SMS OTP

		public async Task<ValidateSmsOtpResult> ValidateSmsOtp(string otp, string uuid)
		{
			if (string.IsNullOrEmpty(otp)) throw new ArgumentNullException(nameof(otp));
			if (string.IsNullOrEmpty(uuid)) throw new ArgumentNullException(nameof(uuid));

			return await ExecuteFaultHandledOperationAsync<ValidateSmsOtpRequest, ValidateSmsOtpResult>(async c =>
			{
				var response = await ValidateSmsOtpAsync(ToValidateSMSOTPDetailsRequest(otp, uuid));
				return ToValidateSMSOTP(response);
			});
		}

		private async Task<ValidateSmsOtpResponse> ValidateSmsOtpAsync(ValidateSmsOtpRequest request) =>
			await ExecuteServiceAsync<ValidateSmsOtpRequest, ValidateSmsOtpResponse>(request);

		private ValidateSmsOtpRequest ToValidateSMSOTPDetailsRequest(string otp, string uuid) => new ValidateSmsOtpRequest
		{
			Otp = otp,
			Uuid = uuid
		};

		private ValidateSmsOtpResult ToValidateSMSOTP(ValidateSmsOtpResponse response) => new ValidateSmsOtpResult
		{
			OtpMatched = Convert.ToBoolean(Convert.ToInt32(response?.ValidateSmsOtp?.OtpMatched)),
			OtpMismatchCount = response?.ValidateSmsOtp?.OtpMismatchCount,
		};

		#endregion

		#region EmiratesId Status

		public async Task<EmiratesIdResult> ValidateEmiratesId(string eidNumber, string name, string expiryDate)
		{
			if (string.IsNullOrEmpty(eidNumber)) throw new ArgumentNullException(nameof(eidNumber));
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			if (string.IsNullOrEmpty(expiryDate)) throw new ArgumentNullException(nameof(expiryDate));

			return await ExecuteFaultHandledOperationAsync<EmiratesIdRequest, EmiratesIdResult>(async c =>
			{
				var response = await ValidateEmiratesId(ToEmiratesIdRequest(eidNumber, name, expiryDate));
				return ToEmiratesId(response);
			});
		}

		private async Task<EmiratesIdResponse> ValidateEmiratesId(EmiratesIdRequest request) =>
			await ExecuteServiceAsync<EmiratesIdRequest, EmiratesIdResponse>(request);

		private EmiratesIdRequest ToEmiratesIdRequest(string eidNumber, string name, string expiryDate) => new EmiratesIdRequest
		{
			EidNumber = eidNumber,
			Name = name,
			ExpiryDate = expiryDate
		};

		private bool status;
		private EmiratesIdResult ToEmiratesId(EmiratesIdResponse response) => new EmiratesIdResult
		{
			CustomerIdentifier = response?.EmiratesId?.CustomerIdentifier,
			Status = bool.TryParse(response?.EmiratesId?.Status, out status)
		};

		#endregion

		#region Get Card Images

		public async Task<List<CardImage>> GetCardImage(string cif)
		{
			if (string.IsNullOrEmpty(cif)) throw new ArgumentNullException(nameof(cif));

			return await ExecuteFaultHandledOperationAsync<CardImagesRequest, List<CardImage>>(async c =>
			{
				var response = await GetCardImage(ToCardImageRequest(cif));
				return ToCardImagesList(response);
			});
		}

		private async Task<CardImagesResponse> GetCardImage(CardImagesRequest request) =>
			await ExecuteServiceAsync<CardImagesRequest, CardImagesResponse>(request);

		private CardImagesRequest ToCardImageRequest(string cif) => new CardImagesRequest
		{
			Cif = cif,
		};

		public List<CardImage> ToCardImagesList(CardImagesResponse res) => res.CardImages?.ConvertAll(ToCardImages);

		private CardImage ToCardImages(CardImages response) => new CardImage
		{
			CardImageNo = Convert.ToInt16(response?.CardImageNo),
			ExpiryDate = response?.ExpiryDate,
			Pan = response?.Pan,
			Track2 = response?.Track2,
		};

        #endregion
    };


}