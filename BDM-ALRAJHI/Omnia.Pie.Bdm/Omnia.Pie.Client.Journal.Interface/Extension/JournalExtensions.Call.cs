using Omnia.Pie.Client.Journal.Interface.Enum;
using System;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void CallConnected(this IJournal journal, string tellerId)
		{
			journal.Write("Call Connected");
			journal.Write($"Teller Id: [{tellerId}]");
		}

		public static void CallDeclined(this IJournal journal)
		{
			journal.Write("Call Declined");
		}

		public static void NoTellerAvailable(this IJournal journal)
		{
			journal.Write("No Teller Available");
		}

		public static void NoTellerLoggedIn(this IJournal journal)
		{
			journal.Write("No Teller Logged In");
		}

		public static void CallEnded(this IJournal journal)
		{
			journal.Write("Call Ended");
		}

		public static void CallingForAssistance(this IJournal journal)
		{
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
			journal.Write("Calling for Assistance");
		}
	}
}