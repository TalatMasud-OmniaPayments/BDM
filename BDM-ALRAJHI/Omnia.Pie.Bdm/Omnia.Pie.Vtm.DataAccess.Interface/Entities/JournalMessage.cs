namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
	using Omnia.Pie.Client.Journal.Interface.Enum;
	using System;

	public class JournalMessage
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public DateTime Timestamp { get; set; }
		public JournalTimestampStyle TimestampStyle { get; set; }
	}
}