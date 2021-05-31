using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Client.Journal.Interface.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Omnia.Pie.Client.Journal
{
	public class JournalFormatter
	{
		/// <summary>
		/// Line terminator on Journal Printer device.
		/// </summary>
		private static readonly string PrinterNewLine = "\n";

		/// <summary>
		/// Empty line for Journal Printer device.
		/// </summary>
		private static readonly string PrinterEmptyLine = " ";

		private static readonly string DateTimeTimestampFormat = "[dd.MM.yy-HH:mm:ss]";
		private static readonly string TimeTimestampFormat = "[HH:mm:ss]";

		private readonly IJournalConfiguration _journalConfiguration;

		public JournalFormatter(IJournalConfiguration journalConfiguration)
		{
			_journalConfiguration = journalConfiguration;
		}

		public string FormatJournalFileText(JournalMessage message)
		{
			string journalText = FormatJournalText(message);
			journalText = journalText.Trim(Environment.NewLine);
			return journalText;
		}

		public IEnumerable<string> FormatJournalPrinterLines(JournalMessage message)
		{
			string journalText = FormatJournalText(message);
			journalText = journalText.Replace(Environment.NewLine, PrinterNewLine);
			List<string> journalLines = SplitTextIntoSubLines(journalText, _journalConfiguration.JournalPrinterMaxLineLength);
			return journalLines;
		}

		private string FormatTimestamp(JournalMessage message)
		{
			switch (message.TimestampStyle)
			{
				case JournalTimestampStyle.DateTime:
					return message.Timestamp.ToString(DateTimeTimestampFormat, CultureInfo.InvariantCulture);

				case JournalTimestampStyle.Time:
					return message.Timestamp.ToString(TimeTimestampFormat, CultureInfo.InvariantCulture);

				case JournalTimestampStyle.None:
					return string.Empty;

				default:
					throw new InvalidOperationException($"JournalTimestampStyle is not supported: [{message.TimestampStyle}].");
			}
		}

		private string FormatJournalText(JournalMessage message)
		{
			string trimmedMessage = message.Text.Trim();

			if (message.TimestampStyle != JournalTimestampStyle.None)
			{
				string journalText = $"{FormatTimestamp(message)} {trimmedMessage}";
				return journalText;
			}
			else
			{
				if (string.IsNullOrEmpty(trimmedMessage))
				{
					return PrinterEmptyLine;
				}
				else
				{
					return trimmedMessage;
				}
			}
		}

		private List<string> SplitTextIntoSubLines(string journalText, int lineMaxLength)
		{
			var subLines = new List<string>();

			int subLineStart = 0;
			while (subLineStart < journalText.Length)
			{
				// Skip line breaks
				while (subLineStart < journalText.Length && journalText.ContainsAtIndex(PrinterNewLine, subLineStart))
				{
					subLineStart++;
				}
				if (subLineStart > journalText.Length)
				{
					break;
				}

				// Determine end of sub line in the message, so that it will not exceed PrinterLineMaxLength limit
				int subLineEnd;
				int newLineIndex = journalText.IndexOf(PrinterNewLine, subLineStart);

				if (newLineIndex != -1 && newLineIndex < subLineStart + lineMaxLength)
				{
					// There's new line character that terminates current sub line and keeps it length within PrinterLineMaxLength limit
					subLineEnd = newLineIndex;
				}
				else
				{
					// There's no new line character that would terminate current sub line (or it is too far and sub line exceeds the limit)
					// So, we should break the sub line at PrinterLineMaxLength length
					subLineEnd = Math.Min(subLineStart + lineMaxLength, journalText.Length);
				}

				// Put sub line to result
				string subLine = journalText.Substring(subLineStart, subLineEnd - subLineStart);
				subLines.Add(subLine);

				subLineStart = subLineEnd;
			}

			return subLines;
		}
	}
}