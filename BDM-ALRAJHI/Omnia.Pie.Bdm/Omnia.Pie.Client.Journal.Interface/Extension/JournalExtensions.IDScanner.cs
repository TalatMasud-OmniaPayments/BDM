namespace Omnia.Pie.Client.Journal.Interface.Extension
{ 
	public static partial class JournalExtensions
    {
        public static void EmiratesIdScannerCardInserted(this IJournal journal)
        {
            journal.Write("EID INSERTED");
        }

        public static void EmiratesIdScannerCardScanned(this IJournal journal)
        {
            journal.Write("EID SCANNED");
        }

        public static void EmiratesIdScannerCardEjected(this IJournal journal)
        {
            journal.Write("EID EJECTED");
        }

        public static void EmiratesIdScannerCardTaken(this IJournal journal)
        {
            journal.Write("EID TAKEN");
        }
    }
}
