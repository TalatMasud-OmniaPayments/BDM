namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IErrorViewModel : IExpirableBaseViewModel
    {
		void Type(ErrorType errorType);
	}

	public enum ErrorType
	{
		/// <summary>
		/// The cheque book is already requested.
		/// </summary>
		AlreadyRequestedChequeBook,

		/// <summary>
		/// Beneficiary ID already exists. Please enter a new beneficiary ID.
		/// </summary>
		BeneficiaryAlreadyExists,

		/// <summary>
		/// Unauthorised usage. Please take your card.
		/// </summary>
		BlockedCard,

		/// <summary>
		/// Please collect your cash. All cash deposit transactions must be in AED only. The minimum deposit denomination is 10 AED.
		/// </summary>
		CollectCash,

		/// <summary>
		/// Cheque scanner error. Please try later.
		/// </summary>
		ChequeScanner,

		/// <summary>
		/// Your card has been captured. Please contact the bank.
		/// </summary>
		CapturedCard,

		/// <summary>
		/// Your transaction is declined. Please contact the sender.
		/// </summary>
		CardlessTransactionDeclined,

		/// <summary>
		/// Cash deposit is declined. Please take your cash back.
		/// </summary>
		DeclinedCashDeposit,

		/// <summary>
		/// Your cheque book request has been declined. Please try later.
		/// </summary>
		DeclinedChequeBook,

		/// <summary>
		/// Your cheque book request has been declined. Please contact branch or contact centre.
		/// </summary>
		DeclinedChequeBookContact,

		/// <summary>
		/// Your ID card has expired.
		/// </summary>
		ExpiredEmiratesId,

		/// <summary>
		/// Amount cannot exceed the maximum transfer limit.
		/// </summary>
		ExceededAmount,

		/// <summary>
		/// OTP entered is invalid and needs to be resent.
		/// </summary>
		ExceededOtp,

		/// <summary>
		/// Your card has expired.
		/// </summary>
		ExpiredCard,

		/// <summary>
		/// You have exceeded your daily pin attempts. SMS sent to your mobile on how to reset your PIN. Please take your card.
		/// </summary>
		ExceededPin,

		/// <summary>
		/// OTP sent to you has been expired and needs to be resent.
		/// </summary>
		ExpiredOtp,

		/// <summary>
		/// You have exceeded the maximum deposit limit. Please take your cash back.
		/// </summary>
		ExceededDepositLimit,

		/// <summary>
		/// You have exceeded your daily withdrawal limit.
		/// </summary>
		ExceededDailyWithdrawalLimit,

		/// <summary>
		/// You have exceeded the withdrawal limit.
		/// </summary>
		ExceededWithdrawalLimit,

		/// <summary>
		/// You have exceeded the withdrawal frequency limit.
		/// </summary>
		ExceededWithdrawalFrequencyLimit,

		/// <summary>
		/// Your request has failed. Please visit the branch.
		/// </summary>
		FailedRequest,

		/// <summary>
		/// This service is currently unavailable, please visit the other al hilal bank machines or the branch
		/// </summary>
		GenericUnavailableService,

		/// <summary>
		/// Invalid PIN. Please try again.
		/// </summary>
		InvalidPin,

        /// <summary>
		/// Invalid Password. Please try again.
		/// </summary>
		InvalidChangePassword,

        /// <summary>
		/// Invalid password. Please try again.
		/// </summary>
        FailUpdatePassword,

        /// <summary>
		/// Invalid PIN. Please try again.
		/// </summary>
		InvalidUpdateInfo,

        /// <summary>
        /// Invalid card number. Please try later.
        /// </summary>
        InvalidCardNumber,

		/// <summary>
		/// Unable to process the transaction. Please try again later.
		/// </summary>
		InvalidTransaction,

		/// <summary>
		/// Please enter a valid beneficiary account.
		/// </summary>
		InvalidBeneficiaryAccount,

		/// <summary>
		/// OTP entered is invalid. Please enter again.
		/// </summary>
		InvalidOtp,

		/// <summary>
		/// The account is invalid.
		/// </summary>
		InvalidAccount,

        /// <summary>
		/// The username password is invalid.
		/// </summary>
		InvalidLogin,

        /// <summary>
		/// Machine is out of service.
		/// </summary>
		OutOfService,

        /// <summary>
        /// The account is invalid.
        /// </summary>
        InvalidLoginLastTry,

        /// <summary>
		/// A user with this name already exist. Please try again later.
		/// </summary>
		InvalidUserInfo,

        /// <summary>
		/// A user with this name already exist. Please try again later.
		/// </summary>
		UsernameAlreadyExists,

        /// <summary>
		/// A user with this name already exist. Please try again later.
		/// </summary>
		EmailAlreadyExists,

        /// <summary>
		/// A user with this name already exist. Please try again later.
		/// </summary>
		MobileAlreadyExists,

        /// <summary>
		/// The Fingerprint is invalid.
		/// </summary>
        InvalidFingerprint,

        /// <summary>
        /// Invalid account number. Please try again.
        /// </summary>
        InvalidAccountNumber,

		/// <summary>
		/// The entered amount is incorrect. Please re-enter amount in multiple of 100.
		/// </summary>
		IncorrectAmount,

		/// <summary>
		/// Insufficient balance in the account.
		/// </summary>
		InsufficientBalance,

		/// <summary>
		/// Invalid debit card.
		/// </summary>
		InvalidDebitCard,

		/// <summary>
		/// Invalid credit card. Please try a valid credit card.
		/// </summary>
		InvalidCreditCard,

		/// <summary>
		/// Lead already exists in the system. You will be contacted shortly.
		/// </summary>
		LeadExists,

		/// <summary>
		/// Your Emirates ID is not registered. Please contact the bank.
		/// </summary>
		NotRegisteredEmiratesId,

		/// <summary>
		/// Machine does not have the requested amount. Please re-enter amount.
		/// </summary>
		NotHaveRequestedAmount,

		/// <summary>
		/// Sorry for inconvenience. The selected function is currently not available. Please try again later.
		/// </summary>
		NotAvailableFunction,

		/// <summary>
		/// The selected service is currently not available. Please try again later.
		/// </summary>
		NotAvailableService,
		/// <summary>
		/// Sorry for the inconvenience. You did not enter your correct PIN. Maximum number of retries has been exceeded. Please remove your card.
		/// </summary>
		PinTriesLimit,

		/// <summary>
		/// Unable to print the receipt. Please try again later.
		/// </summary>
		UnablePrintReceipt,

		/// <summary>
		/// Unable to dispense the money. Please try later.
		/// </summary>
		UnableDispense,

		/// <summary>
		/// Unable to deposit your cash.
		/// </summary>
		UnableDeposit,

		/// <summary>
		/// Unauthorized usage. Please contact the bank.
		/// </summary>
		UnauthorizedUsage,

		/// <summary>
		/// Unable to provide the information. Please try later.
		/// </summary>
		UnableProvideInformation,

		/// <summary>
		/// We are currently busy serving other customers, please hold and we will be assisting you shortly.
		/// </summary>
		NoTellerAvailable,

		/// <summary>
		/// Our working hours are from 8:00 AM to 10:00 PM) Saturday to Thursday
		/// You can always perform your transactions through this machine or through online or mobile banking
		///For more information, please call 600599992
		/// </summary>
		NoTellerLoggedIn,

		/// <summary>
		/// Authentication has been failed.
		/// </summary>
		AuthenticationFailed,

		/// <summary>
		/// A4 Printer is not available.
		/// </summary>
		A4PrinterNotAvailable,

		/// <summary>
		/// Paper jam in A4 printer.
		/// </summary>
		A4PrinterPaperJam,

		/// <summary>
		/// Invalid Cheque
		/// </summary>
		InvalidCheque,

		/// <summary>
		/// Invalid Date
		/// </summary>
		InvalidDate,

		/// <summary>
		/// No cheques inserted
		/// </summary>
		NoCheques,

		/// <summary>
		/// Cards are not linked to your Emirates ID
		/// </summary>
		CardsNotLinked,

        /// <summary>
		/// Internet is not reachable.
		/// </summary>
        NoInternet,
        /// <summary>
		/// Internet is not reachable in middle of an online transaction.
		/// </summary>
        InternetInterrupted,

        /// <summary>
        /// Finger scanner is not available.
        /// </summary>
        /// 
        FingerScannerNotAvailable,

    }
}