namespace Omnia.Pie.Client.Journal.Interface
{
	public interface IJournalConfiguration
	{
		/// <summary>
		/// Indicates whether journal records should be written to the journal printer.
		/// </summary>
		bool WriteToJournalPrinter { get; }

		/// <summary>
		/// Maximal number of characters that might be put on single line in journal printer.
		/// </summary>
		int JournalPrinterMaxLineLength { get; }
	}
}
