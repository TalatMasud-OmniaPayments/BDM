namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void ChequeAcceptorChequesInserted(this IJournal journal)
		{
			journal.Write("CHK INSERTED");
		}

		public static void ChequeAcceptorInsertCompleted(this IJournal journal)
		{
			journal.Write("CHK INSERT COMPLETED");
		}

		public static void ChequeAcceptorChequesDeposited(this IJournal journal)
		{
			journal.Write("CHK DEPOSITED");
		}

		public static void ChequeAcceptorDepositCanceled(this IJournal journal)
		{
			journal.Write("CHK DEPOSIT CANCELED");
		}

		public static void ChequeAcceptorChequesTaken(this IJournal journal)
		{
			journal.Write("CHK TAKEN");
		}

		public static void ChequeAcceptorChequesRetracted(this IJournal journal)
		{
			journal.Write("CHK RETRACTED");
		}
	}
}
