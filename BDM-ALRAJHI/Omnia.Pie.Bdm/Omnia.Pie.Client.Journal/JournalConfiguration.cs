using Omnia.Pie.Client.Journal.Interface;
using System.Configuration;

namespace Omnia.Pie.Client.Journal
{
	// TODO: move to Vtm.Shell after integration of supervisor and Vtm applications
	internal class JournalConfiguration : IJournalConfiguration
	{
		public JournalConfiguration()
		{
			bool writeToJournalPrinter;
			if (bool.TryParse(ConfigurationManager.AppSettings["Journal.WriteToJournalPrinter"], out writeToJournalPrinter))
			{
				WriteToJournalPrinter = writeToJournalPrinter;
			}
			else
			{
				WriteToJournalPrinter = true;
			}

			int journalPrinterMaxLineLength;
			if (int.TryParse(ConfigurationManager.AppSettings["Journal.JournalPrinterMaxLineLength"], out journalPrinterMaxLineLength))
			{
				JournalPrinterMaxLineLength = journalPrinterMaxLineLength;
			}
			else
			{
				JournalPrinterMaxLineLength = 36;
			}
		}

		public bool WriteToJournalPrinter { get; private set; }

		public int JournalPrinterMaxLineLength { get; private set; }
	}
}