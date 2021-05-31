using Omnia.Pie.Client.Journal.Interface.Enum;
using System;

namespace Omnia.Pie.Client.Journal
{
	public class JournalMessage
	{
		public JournalMessage(string text, JournalTimestampStyle timestampStyle)
		{
			Text = text;
			TimestampStyle = timestampStyle;
			Timestamp = DateTime.Now;
		}

		public JournalMessage(string text, JournalTimestampStyle timestampStyle, DateTime timestamp)
		{
			Text = text;
			TimestampStyle = timestampStyle;
			Timestamp = timestamp;
		}

		public string Text { get; }
		public DateTime Timestamp { get; }
		public JournalTimestampStyle TimestampStyle { get; }
	}
}