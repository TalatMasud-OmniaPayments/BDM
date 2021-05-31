namespace Omnia.Pie.Vtm.Services.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IAuthenticationService
	{
		Task<PinVerificationResult> VerifyPin(string track2, string pin, string iccRequest);
		Task<List<Account>> GetAccounts(string customerIdentifier, string Username, AccountCriterion conditionId);
        Task<List<Interface.Entities.Account>> GetAccounts(string customerIdentifier, string Username, string TransactionNumber, AccountCriterion conditionId);


        Task<SendSMSOTPResult> SendSmsOtp(string CustomerIdentifier, string sMSExpiryTime);
		Task<ValidateSmsOtpResult> ValidateSmsOtp(string otp, string Uuid);
		Task<EmiratesIdResult> ValidateEmiratesId(string eidNumber, string name, string expiryDate);
		Task<List<CardImage>> GetCardImage(string cif);

    }
}