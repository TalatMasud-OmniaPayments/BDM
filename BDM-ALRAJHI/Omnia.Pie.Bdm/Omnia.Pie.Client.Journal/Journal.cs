using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Client.Journal.Interface.Enum;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StorageEntities = Omnia.Pie.Vtm.DataAccess.Interface.Entities;

namespace Omnia.Pie.Client.Journal
{
	public class Journal : IJournal
	{
		private readonly ILogger _logger;
		private bool _writingInProgress;
		private readonly IJournalMessageStore _journalStore;
		private readonly IJournalConfiguration _journalConfiguration;
		private readonly IJournalPrinter _journalPrinter;
		private readonly ILogger _journalLogger;
		private readonly JournalFormatter _journalFormatter;

		public Journal(
			ILogger logger,
			IJournalMessageStore journalStore,
			IJournalConfiguration journalConfiguration,
			IJournalPrinter journalPrinter,
			ILogger journalLogger)
		{
			_logger = logger;
			_journalStore = journalStore;
			_journalConfiguration = journalConfiguration;
			_journalPrinter = journalPrinter;
			_journalLogger = journalLogger;
			_journalFormatter = new JournalFormatter(journalConfiguration);
		}

		public void Write(string message, JournalTimestampStyle timestampStyle)
		{
			if (message == null)
			{
				return;
			}

			var journalMessage = new JournalMessage(message, timestampStyle);
			WriteToStorage(journalMessage);
			WriteToFile(journalMessage);
			if (!_writingInProgress)
			{
				WriteStoredMessages();
			}
		}

		private async void WriteStoredMessages()
		{
			_writingInProgress = true;
			try
			{
				if (_journalPrinter.IsAvailable)
				{
					var message = await _journalStore.Get();
					while (message != null)
					{
						try
						{
							await WriteToPrinterAsync(new JournalMessage(message.Text, message.TimestampStyle, message.Timestamp));
							await _journalStore.Delete(message);
							message = await _journalStore.Get();
						}
						catch (Exception ex)
						{
							_logger.Exception(ex);
							break;
						}
					}
				}
			}
			finally
			{
				_writingInProgress = false;
			}
		}

		private void WriteToFile(JournalMessage message)
		{
			string journalText = _journalFormatter.FormatJournalFileText(message);
			_journalLogger.Info(journalText);
		}

		private void WriteToStorage(JournalMessage message)
		{
			_journalStore.Save(new StorageEntities.JournalMessage
			{
				Text = message.Text,
				Timestamp = message.Timestamp,
				TimestampStyle = message.TimestampStyle
			});
		}

		private async Task WriteToPrinterAsync(JournalMessage message)
		{
			IEnumerable<string> journalLines = _journalFormatter.FormatJournalPrinterLines(message);

			if (_journalConfiguration.WriteToJournalPrinter)
			{
				foreach (string journalLine in journalLines)
				{
					await _journalPrinter.PrintAsync(journalLine);
				}
			}
			else
			{
				//_logger?.Info($"JournalPrinter:{Environment.NewLine}{string.Join(Environment.NewLine, journalLines)}");
			}
		}
	}
}
