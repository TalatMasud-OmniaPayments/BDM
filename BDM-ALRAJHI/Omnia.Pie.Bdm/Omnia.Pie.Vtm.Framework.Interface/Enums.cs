namespace Omnia.Pie.Vtm.Framework.Interface
{
	public enum StatusEnum
	{
		Main = 000,
		AuthenticateDebitCreditCard = 1,
		AuthenticateWithEmiratesId = 2,
		AuthenticateWaitingForPin = 3,
		AuthenticateWaitingForAuthentication = 4,
		AuthenticateCardAuthenticated = 5,
		AuthenticateCustomerValidated = 6,
		AuthenticateActivateOTP = 7,
		AuthenticateOTPActivated = 8,
		AuthenticateEmiratesIdAuthenticated = 9,
		AuthenticateIlliterateCustomer = 10,
		AuthenticateEIDDetails = 11,
		AuthenticateInvalidPin = 12,
		AuthenticateCif = 13,
		AuthenticateOffUsCard = 14,
		AuthenticateTerminalId = 15,
		AuthenticateBranchId = 16,

		#region "Cash Deposit Account"

		CashDepositAccount = 100,
		CashDepositAccountCardNumber = 101,
		CashDepositAccountCardNumberConfirmed = 102,
		CashDepositAccountActivateCashDeposit = 103,
		CashDepositAccountAmountEntered = 104,
		CashDepositAccountConfirmed = 105,
		CashDepositAccountConfirmCashDeposit = 106,
		CashDepositAccountCancelCashDeposit = 107,
		CashDepositAccountPaymentSuccess = 108,
		CashDepositAccountPrintReceipt = 109,
		CashDepositAccountReceiptPrinted = 110,
		CashDepositAccountAddMore = 111,
		CashDepositAccountSourceOfFund = 112,
		CashDepositAccountLimitExceeded = 113,
		CashDepositAccountConfirmEID = 114,
		CashDepositAccountEIDConfirmed = 115,
		CashDepositAccountEmiratesIdTaken = 116,
		CashDepositAccountLinkedAccounts = 117,
		CashDepositAccountActivateEIDScanner = 118,
		CashDepositAccountEIDScannerActivated = 119,
		CashDepositAccountSourceOfFundConfirmed = 120,
		CashDepositAccountEIDUploaded = 121,

		#endregion

		#region "Cash Deposit Credit Card"

		CashDepositCreditCard = 200,
		CashDepositCreditCardUseCreditCard = 201,
		CashDepositCreditCardAuthenticateCard = 202,
		CashDepositCreditCardCancelAuthentication = 203,
		CashDepositCreditCardSendAccountInformation = 205,
		CashDepositCreditCardActivateCashDeposit = 206,
		CashDepositCreditCardCancelCashDeposit = 207,
		CashDepositCreditCardConfirmCashDeposit = 208,
		CashDepositCreditCardSendTransactionInformation = 209,
		CashDepositCreditCardManualEntry = 250,

		#endregion

		#region "ChequeDepositAccount"

		ChequeDepositAccount = 300,
		ChequeDepositAccountAccountNumber = 301,
		ChequeDepositAccountCardNumberConfirmed = 302,
		ChequeDepositAccountActivateAmountEntry = 303,
		ChequeDepositAccountAmountEntered = 304,
		ChequeDepositAccountActivateDeposit = 306,
		ChequeDepositAccountConfirmDeposit = 307,
		ChequeDepositAccountDepositConfirmed = 308,
		ChequeDepositAccountCancelDeposit = 309,
		ChequeDepositAccountPrintReceipt = 310,
		ChequeDepositAccountReceiptPrinted = 311,
		ChequeDepositAccountChequeInserted = 312,
		ChequeDepositAccountChequeScanning = 313,
		ChequeDepositAccountChequeScanningDone = 314,
		ChequeDepositAccountDepositSuccess = 315,

		#endregion

		#region "ChequeDepositCreditCard"

		ChequeDepositCreditCard = 400,
		ChequeDepositCreditCardCardNumber = 401,
		ChequeDepositCreditCardCardNumberConfirmed = 402,
		ChequeDepositCreditCardActivateAmountEntry = 403,
		ChequeDepositCreditCardAmountEntered = 404,
		ChequeDepositCreditCardActivateDeposit = 406,
		ChequeDepositCreditCardConfirmDeposit = 407,
		ChequeDepositCreditCardDepositConfirmed = 408,
		ChequeDepositCreditCardCancelDeposit = 409,
		ChequeDepositCreditCardPrintReceipt = 410,
		ChequeDepositCreditCardReceiptPrinted = 411,
		ChequeDepositCreditCardChequeInserted = 412,
		ChequeDepositCreditCardChequeScanning = 413,
		ChequeDepositCreditCardChequeScanningDone = 414,
		ChequeDepositCreditCardDepositSuccess = 415,
		ChequeDepositEncashCheque = 416,
		ChequeDepositEncashChequeSuccess = 417,
		ChequeDepositAccountVerifySignature = 418,
		ChequeDepositAccountVerifySecurityImage = 419,
		#endregion

		#region "Cash Withdrawal"

		CashWithdrawal = 500,

		CashWithdrawalDispenseCash = 501,
		CashWithdrawalDispenseCashSuccess = 503,

		CashWithdrawalCancelAuthentication = 504,
		CashWithdrawalSendTransactionInformation = 505,
		CashWithdrawalInitiateCashDispense = 506,
		CashWithdrawalActivatePrinter = 507,

		CashWithdrawalUseEmiratesId = 502,
		CashWithdrawalAuthenticateEmiratesIdCard = 510,
		CashWithdrawalCancelAuthenticateEmiratesIdCard = 511,
		CashWithdrawalUseEmiratesIdActivateOTPInput = 512,
		CashWithdrawalUseEmiratesIdSendAccountInformation = 513,
		CashWithdrawalUseEmiratesIdAmountEntered = 514,
		CashWithdrawalUseEmiratesIdSendTransactionInformation = 515,

		CashWithdrawalCardAccounts = 516,
		CashWithdrawalCardAuthenticated = 532,
		CashWithdrawalSelectedAccount = 517,
		CashWithdrawalEnteredAmount = 518,
		CashWithdrawalEnteredAmountConfirmed = 519,

		CashWithdrawalWaitingForPin = 520,
		CashWithdrawalWaitingForAuthentication = 521,

		CashWithdrawalReceiptPrinted = 522,
		CashWithdrawalTransactionInformationReceived = 523,

		CashWithdrawalUseEmiratesIdOTPConfirmed = 524,
		CashWithdrawalUseEmiratesIdSelectedAccount = 525,
		CashWithdrawalUseEmiratesIdEnteredOTP = 526,
		CashWithdrawalUseEmiratesIdAuthenticated = 527,
		CashWithdrawalUseEmiratesIdInitiateCashDispense = 528,
		CashWithdrawalUseEmiratesIdCashDispensedSucess = 529,
		CashWithdrawalUseEmiratesIdPrintReceipt = 530,
		CashWithdrawalUseEmiratesIdReceiptPrintSuccess = 531,

		CashWithdrawalSendAccountInformation = 534,
		CashWithdrawalManuallyEnteredAccount = 535,
		CashWithdrawalAccountConfirmed = 536,

		CashWithdrawalPrintReceipt = 540,
		CashWithdrawalAccountVerbalAccount = 541,
		CashWithdrawalReceiptPrinting = 542,

        CashWithdrawalCreditCard = 550,
        CashWithdrawalCreditCardNumberConfirm = 551,
        CashWithdrawalCreditCardEnteredAmountConfirmed = 552,
        CashWithdrawalCreditCardDispenseCashSuccess = 553,
        CashWithdrawalCreditCardReceiptPrinted = 554,

        #endregion

        #region "FundsTransferOtherAccount"

        FundsTransferOtherAccount = 600,
		FundsTransferOtherAccountUseDebitCard = 601,

		FundsTransferOtherAccountUseDebitCardAuthenticateCard = 602,
		FundsTransferOtherAccountUseDebitCardWaitingForPIN = 603,
		FundsTransferOtherAccountUseDebitCardWaitingForAuthentication = 604,

		FundsTransferOtherAccountLoadTransferAccounts = 605,
		FundsTransferOtherAccountTransactionSuccess = 606,
		FundsTransferOtherAccountPrintReceipt = 607,
		FundsTransferOtherAccountReceiptPrinted = 608,
		FundsTransferOtherAccountUseDebitCardLinkedAccounts = 609,
		FundsTransferOtherAccountUseDebitCardCardAuthenticated = 610,

		FundsTransferOtherAccountUseEID = 611,
		FundsTransferOtherAccountManuallyEnteredAccount = 612,
		FundsTransferOtherAccountActivateOTPInput = 613,
		FundsTransferOtherAccountEnteredOTP = 614,
		FundsTransferOtherAccountOTPConfirmed = 615,
		FundsTransferOtherAccountAuthenticateEmiratesIdCard = 616,
		FundsTransferOtherAccountEmiratesIdAuthenticated = 617,
		FundsTransferOtherAccountEmiratesIdTaken = 618,

		FundsTransferOtherAccountFundsTransferAccounts = 619,

		#endregion

		#region "AccountInquiry"

		AccountInquiry = 700,

		AccountInquiryUseDebitCard = 701,
		AccountInquiryUseManualEntry = 702,

		AccountInquiryAuthenticateCard = 703,
		AccountInquiryCancelAuthentication = 704,

		AccountInquiryWaitingForPin = 705,
		AccountInquiryWaitingForAuthentication = 706,
		AccountInquiryCardAuthenticated = 707,

		AccountInquiryCardAccounts = 708,

		AccountInquiryUseEmiratesIdActivateOTPInput = 710,
		AccountInquiryUseEmiratesIdEnteredOTP = 711,
		AccountInquiryUseEmiratesIdOTPConfirmed = 712,

		AccountInquiryAuthenticateEmiratesIdCard = 713,
		AccountInquiryUseEmiratesIdAuthenticated = 714,
		AccountInquiryUseEmiratesIdSelectedAccount = 715,
		AccountInquiryEmiratesIdTaken = 716,

		AccountInquiryCompleted = 717,

		#endregion

		#region "Documents"

		Documents = 750,

		#endregion

		#region "FundsTransferOwnAccount"

		FundsTransferOwnAccount = 800,
		FundsTransferOwnAccountUseDebitCard = 801,

		FundsTransferOwnAccountUseDebitCardAuthenticateCard = 802,
		FundsTransferOwnAccountUseDebitCardWaitingForPIN = 803,
		FundsTransferOwnAccountUseDebitCardWaitingForAuthentication = 804,

		FundsTransferOwnAccountLoadTransferAccounts = 805,
		FundsTransferOwnAccountTransactionSuccess = 806,
		FundsTransferOwnAccountPrintReceipt = 807,
		FundsTransferOwnAccountReceiptPrinted = 808,
		FundsTransferOwnAccountUseDebitCardLinkedAccounts = 809,
		FundsTransferOwnAccountUseDebitCardCardAuthenticated = 810,

		FundsTransferOwnAccountUseEID = 811,
		FundsTransferOwnAccountManuallyEnteredAccount = 812,
		FundsTransferOwnAccountActivateOTPInput = 813,
		FundsTransferOwnAccountEnteredOTP = 814,
		FundsTransferOwnAccountOTPConfirmed = 815,
		FundsTransferOwnAccountAuthenticateEmiratesIdCard = 816,
		FundsTransferOwnAccountEmiratesIdAuthenticated = 817,
		FundsTransferOwnAccountEmiratesIdTaken = 818,

		FundsTransferOwnAccountFundsTransferAccounts = 819,

		#endregion

		#region "CreditCardPaymentWithAccount"

		CreditCardPaymentWithAccount = 850,
		CreditCardPaymentWithAccountCardNumber = 851,
		CreditCardPaymentWithAccountSelectedAccountNumber = 852,
		CreditCardPaymentWithAccountPaymentSuccess = 853,
		CreditCardPaymentWithAccountPrintReceipt = 854,
		CreditCardPaymentWithAccountPrinted = 855,

		#endregion

		#region "CreateBeneficiary"

		CreateBeneficiary = 900,

		CreateBeneficiaryUseDebitCardAuthenticateCard = 902,
		CreateBeneficiaryUseDebitCardWaitingForPIN = 903,
		CreateBeneficiaryUseDebitCardWaitingForAuthentication = 904,
		CreateBeneficiaryAddNewTransaction = 905,

		CreateBeneficiaryPrintReceipt = 907,
		CreateBeneficiaryReceiptPrinted = 908,
		CreateBeneficiaryUseDebitCardLinkedAccounts = 909,
		CreateBeneficiaryUseDebitCardCardAuthenticated = 910,

		CreateBeneficiaryUseEmiratesID = 911,
		CreateBeneficiaryManuallyEnteredAccount = 912,
		CreateBeneficiaryActivateOTPInput = 913,
		CreateBeneficiaryEnteredOTP = 914,
		CreateBeneficiaryOTPConfirmed = 915,
		CreateBeneficiaryAuthenticateEmiratesIdCard = 916,
		CreateBeneficiaryEmiratesIdAuthenticated = 917,
		CreateBeneficiaryEmiratesIdTaken = 918,

		#endregion

		#region "NewLead"

		AccountLeadStarting = 951,
		AccountLeadStarted = 952,
		AccountLeadCompleted = 953,

		LoanLeadStarting = 961,
		LoanLeadStarted = 962,
		LoanLeadCompleted = 963,

		CreditCardLeadStarting = 971,
		CreditCardLeadStarted = 972,
		CreditCardLeadCompleted = 973,

		WealthLeadStarting = 981,
		WealthLeadStarted = 982,
		WealthLeadCompleted = 983,

		#endregion

		#region "UpdateCustomerDetails"

		UpdateCustomerDetailsStarting = 1000,
		UpdateCustomerDetailsStarted = 1001,
		UpdateCustomerDetailsCompleted = 1002,

		UpdateCustomerDetailsOTPValidated = 1010, // Message from VTM: informs RT that OTP is valid
		UpdateCustomerDetailsScanEmiratesIdActivate = 1011, // Message from RT: commands VTM to activate scanner device
		UpdateCustomerDetailsScanEmiratesIdToVerify = 1012, // Message from VTM: informs RT that scanned Emirates Id is available for review
		UpdateCustomerDetailsScanEmiratesIdVerified = 1013, // Message from RT: informs VTM that Emirates Id has been verified by RT

		#endregion

		#region "Credit Card Payment With Cash"

		CreditCardPaymentWithCash = 1050,
		CreditCardPaymentWithCashCardNumber = 1051,
		CreditCardPaymentWithCashCardNumberConfirmed = 1052,
		CreditCardPaymentWithCashActivateCashDeposit = 1053,
		CreditCardPaymentWithCashAmountEntered = 1054,
		CreditCardPaymentWithCashConfirmed = 1055,
		CreditCardPaymentWithCashConfirmCashDeposit = 1056,
		CreditCardPaymentWithCashCancelCashDeposit = 1057,
		CreditCardPaymentWithCashPaymentSuccess = 1058,
		CreditCardPaymentWithCashPrintReceipt = 1059,
		CreditCardPaymentWithCashReceiptPrinted = 1060,
		CreditCardPaymentWithCashAddMore = 1061,
		CreditCardPaymentWithCashSourceOfFund = 1062,
		CreditCardPaymentWithCashLimitExceeded = 1063,
		CreditCardPaymentWithCashConfirmEID = 1064,
		CreditCardPaymentWithCashEIDConfirmed = 1065,
		CreditCardPaymentWithCashEmiratesIdTaken = 1066,
		CreditCardPaymentWithCashActivateEIDScanner = 1067,
		CreditCardPaymentWithCashEIDScannerActivated = 1068,
		CreditCardPaymentWithCashSourceOfFundConfirmed = 1069,
		CreditCardPaymentWithCashEIDUploaded = 1070,
		CreditCardPaymentWithCashRollbackStarted = 1071,

		#endregion

		#region "AdditionalAccountOpening"

		AdditionalAccountOpeningRequestByRT = 1100,
		AdditionalAccountOpeningRequestByRTCardNumber = 1101,               // Message from VTM: informs RT about card number used for authentication
		AdditionalAccountOpeningRequestByRTSetAccountType = 1102,           // Message from RT: sends account type to VTM
		AdditionalAccountOpeningRequestByRTSetCurrency = 1103,              // Message from RT: sends currency to VTM
		AdditionalAccountOpeningRequestByRTAccountDetailSpecified = 1104,   // Message from VTM: transits RT to Capture Signatures screen
		AdditionalAccountOpeningRequestByRTCaptureSignature1 = 1105,        // Message from RT: commands VTM to capture customer's first signature from Signpad
		AdditionalAccountOpeningRequestByRTSignature1Captured = 1106,       // Message from VTM: informs RT that first signature has been captured
		AdditionalAccountOpeningRequestByRTCaptureSignature2 = 1107,        // Message from RT: commands VTM to capture customer's second signature from Signpad
		AdditionalAccountOpeningRequestByRTSignature2Captured = 1108,       // Message from VTM: informs RT that second signature has been captured
		AdditionalAccountOpeningRequestByRTSignaturesVerified = 1109,       // Message from RT: informs VTM that signatures has been verified by RT. VTM proceeds to Form Review step
		AdditionalAccountOpeningRequestByRTVerifyFormByRT = 1110,           // Message from VTM: informs RT that form is available for verification
		AdditionalAccountOpeningRequestByRTFormVerifiedByRT = 1111,         // Message from RT: informs VTM that form has been verified by RT
		AdditionalAccountOpeningRequestByRTFormConfirmedByCustomer = 1112,  // Message from VTM: informs RT that form has been confirmed by customer. RT proceeds to scan Emirates Id step
		AdditionalAccountOpeningRequestByRTFormUploaded = 1113,             // Message from VTM: informs RT that form has been uploaded to Documentum. No action on RT is required
		AdditionalAccountOpeningRequestByRTEmiratesIdUploaded = 1114,       // Message from VTM: informs RT that scanned Emirates Id has been uploaded to Documentum. No action on RT is required
		AdditionalAccountOpeningRequestByRTEmailSent = 1115,                // Message from VTM: informs RT that email about account opening request has been sent. RT proceeds to End Session screen
		AdditionalAccountOpeningRequestByRTFgbError = 1116,                 // Message from VTM: informs RT that interaction with FGB services wasn't successful and client should go to branch office
		AdditionalAccountOpeningRequestByRTAccountTypeSelected = 1117,      // Message from VTM: informs RT about account type selected by the customer
		AdditionalAccountOpeningRequestByRTCurrencySelected = 1118,         // Message from VTM: informs RT about currency selected by the customer
		AdditionalAccountOpeningRequestByRTAccountDetailsEditing = 1120,    // Message from VTM: informs RT about editing account information by the customer

		#endregion

		#region ScanEmiratesIdStep

		ScanEmiratesIdStepActivateScanner = 1200,      // Message from RT: commands VTM to activate scanner device
		ScanEmiratesIdStepVerifyEmiratesId = 1201,     // Message from VTM: informs RT that scanned Emirates Id is available for review
		ScanEmiratesIdStepEmiratesIdVerified = 1202,   // Message from RT: informs VTM that Emirates Id has been verified by RT

		#endregion

		#region OTPSent

		OTPInitializing = 1300, // Message from VTM: informs RT that first try was made (required only for Add Beneficiary)
		OTPSent = 1301,         // Message from VTM: informs RT that new try to send (resend) OTP was made

		#endregion

		#region "BillPaymentWithCreditCard"

		BillPaymentWithCreditCard = 1400,
		BillPaymentWithCreditCardCardNumber = 1401,
		BillPaymentWithCreditCardConsumerNumber = 1402,
		BillPaymentWithCreditCardPaymentSuccess = 1403,
		BillPaymentWithCreditCardPrintReceipt = 1404,
		BillPaymentWithCreditCardPrinted = 1405,

		#endregion

		#region "BillPaymentWithAccount"

		BillPaymentWithAccount = 1500,
		BillPaymentWithAccountCardNumber = 1501,
		BillPaymentWithAccountConsumerNumber = 1502,
		BillPaymentWithAccountPaymentSuccess = 1503,
		BillPaymentWithAccountPrintReceipt = 1504,
		BillPaymentWithAccountPrinted = 1505,

		#endregion

		#region "ChequePrinting"

		ChequePrintingEligibility = 1549,
		ChequePrintingStart = 1550,
		ChequePrintingLinkedAccounts = 1551,
		ChequePrintingSelectedAccount = 1552,
		ChequePrintingCharges = 1553,
		ChequePrintingPrintingLeaf = 1554,
		ChequePrintingPrintingPrint = 1555,

		#endregion

		BackButtonCode = 999,

		/// <summary>
		/// Ends current workflow.
		/// </summary>
		EndCurrentSession = 999000,

		AuthenticationFailedEndCurrentSession = 999001,
		PINAuthenticationEndCurrentSession = 999002,
		CardInsertSuccess = 999003,

		/// <summary>
		/// Ends the whole call (can happen at any moment.
		/// </summary>
		EndCall = 999004,

		/// <summary>
		/// Ends current workflow and returns session to non-authenticated state.
		/// </summary>
		ResetCall = 999005,

		RollbackStarted = 999006,                 // Message from VTM: informs that rollback of transaction was started
		TransactionFailed = 999007,               // Message from VTM: informs that transaction was declined
		PrintTransactionFailureReceipt = 999008,  // Message from RT: commands to print declined transaction receipt
		TransactionDeclinedPrinted = 999009,      // Message from VTM: informs that printing completed
		StartSession = 999010,
		GetGrant = 999011,
		SendGrant = 999012,
		SendSessionLanguage = 999013,             // Message from VTM: informs about customer selected language

		HoldCall = 999014,
		UnholdCall = 999015,

		Back = 999200,
		ParseException = 9999999,
		SelfInstructionMode = 999300,
		ChequeDepositMode = 999400,
		ChequePrintingMode = 999500,
	}

	public enum Language
	{
		English,
		Arabic
	}

	public enum MessageType
	{
		CardAccounts = 1,
		SelectedAccount = 2,
		EnteredAmount = 3,
		Device = 4,
		Command = 5,
	}

	public enum DeviceType
	{
		CashDispenser = 1,
		CardReader = 2,
		Scanner = 3,
		ReceiptPrinter = 4,
		StatementPrinter = 5,
		PinPad = 6,
		Doors = 7,
		Sensors = 8,
		Camera = 9,
		RFIDReader = 10,
		Auxiliaries = 11,
		SignPad = 12,
		Indicators = 13,
		DVCSignal = 14,
		VDM = 15,
		VFD = 16,
		TMD = 17,

		ChequeScanner = 18,
		JournalPrinter = 19,
		CashAcceptor = 20,
		IDScanner = 21,
		A4Printer = 22,
		A4Scanner = 23,
		CassetteStatus = 24,
		CoinDispenser = 25,
		ChequeAcceptor = 26,
        FingerPrintScanner = 27,
		Cassettes = 28,
	}

	public enum RTDeviceStatus
	{
		Idle = 1,
		Offline = 2,
		Active = 3,
		PaperJam = 4,
	}

	public enum CurrenceyNoteType
	{
		TenNote = 1,
		TwentyNote = 2,
		FiftyNote = 3,
		OneHundredNote = 4,
		TwoHundredNote = 5,
		FiveHundredNote = 6,
		OneThousandNote = 7,
	}

	public enum TransactionType
	{
		None = 0,
		RemoteTeller = 1,
		SelfService = 2,
	}

}