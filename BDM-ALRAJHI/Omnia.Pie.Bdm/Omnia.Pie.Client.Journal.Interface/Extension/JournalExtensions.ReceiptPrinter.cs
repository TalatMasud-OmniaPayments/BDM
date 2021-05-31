namespace Omnia.Pie.Client.Journal.Interface.Extension
{ 
	public static partial class JournalExtensions
    {
		public static void PrintingReceipt(this IJournal journal, string receipt)
        {
            journal.Write(receipt);
        }
    }
}
