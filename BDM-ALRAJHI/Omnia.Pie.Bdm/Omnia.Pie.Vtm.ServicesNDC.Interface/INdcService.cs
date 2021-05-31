namespace Omnia.Pie.Vtm.ServicesNdc.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface INdcService
	{
		Task<CashWithdrawal> CashWithdrawalDebitCardAsync(string track2, string pin, string iccRequest, string amount, string transactionCode, bool HasMultipleAccounts = false, string AccountNo = "");
		Task<bool> ValidatePinAsync(string track2, string pin);
		Task<string> CashWithdrawalReversalAsync();
		Task<CashDeposit> CashDepositCCAsync(string track2, List<DepositDenomination> amount, string totalAmount = "");
		Task<List<NdcCard>> GetEIDACardListAsync(string eidNumber);
		Task<bool> CardSelectedAsync(string eidNumber, string cardFDK, bool waitForResponse = true);
		Task<bool> ValidateEidPinAsync(string eidNumber, string cardFDK, string pinBlock, bool waitForReponse = true);
		Task<bool> PreCashWithdrawalEIDAsync(string eidNumber, string cardFDK, string pinBlock);
		Task<CashWithdrawal> ActualCashWithdrawalEIDAsync(string pin, string amount, string eidNumber, string cardFDK);
		Task<bool> GetReadyAsync();
        Task CashWithrawalAdviceConfirmationAsync();
	}
}