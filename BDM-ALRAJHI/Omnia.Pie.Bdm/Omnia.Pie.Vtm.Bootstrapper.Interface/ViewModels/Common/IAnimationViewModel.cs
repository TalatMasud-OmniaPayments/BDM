namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IAnimationViewModel : IExpirableBaseViewModel
    {
		void Type(AnimationType type);
	}

	public enum AnimationType
	{
		CallingForAssistance,
		CallEnded,
		CancelledDepositTransaction,
		CollectCheques,
		CollectRemainingCheques,
		CollectStatement,
		CollectIBAN,
		ChequesProcessing,
		InitializingApplication,
		InsertCheque,
		InsertCashMax,
		InsertCash,
		InsertCard,
		InsertEmiratesId,
		NotifyActivatedAccount,
		NoTellerAvailable,
		NotEligibleTransaction,
		PrintingReceipt,
		PrintingStatement,
		PrintingIBAN,
		PrintingCheque,
		RetractingCard,
		RetractingCash,
		RetractingEmiratesId,
		RetractingCheques,
		SuccessfulRequest,
		TakeCard,
		TakeReceipt,
		TakeCash,
		TakeCashEncashment,
		TakeCoins,
		TakeEmiratesId,
		UsingATMNetwork,
		Wait,
        FingerperintScan,
        FingerperintRegister,
    }
}