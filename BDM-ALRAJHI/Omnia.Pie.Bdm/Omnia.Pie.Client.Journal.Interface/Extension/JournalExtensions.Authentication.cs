namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void AuthenticationSucceeded(this IJournal journal)
		{
			journal.Write("Auth Success");
		}

		public static void AuthenticationFailed(this IJournal journal)
		{
			journal.Write("Auth Failed");
		}

        public static void OffUsCard(this IJournal journal)
        {
            journal.Write("Card Type: Off Us Card");
        }
    }
}