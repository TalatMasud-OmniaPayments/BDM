namespace Omnia.PIE.VTA
{
	public enum WorkflowType
	{
		CashWithdrawal = 0,
		CashDeposit = 1,
		ChequeDeposit = 2,
		FundsTransferBetweenOwnAccounts = 3,
		FundsTransfertoOtherAccounts = 4,
	}

	public enum TellerState
	{
		Unknown = 0,
		Busy = 3,
		Idle = 4,
		Working = 5,
		Talking = 7,
		Rest = 8,
	}

	public enum SupportVideoParam
	{
		FrameRate,
		x,
		y,
	}

	public enum CassetteStatus
	{
		OneHundredNote = 1,
		TwoHundredNote = 2,
		FiveHundredNote = 3,
		OneThousandNote = 4,
		Rut = 5,
		Overflow = 6,
	}
}