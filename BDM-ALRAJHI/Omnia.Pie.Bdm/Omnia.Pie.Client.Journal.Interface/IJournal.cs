using Omnia.Pie.Client.Journal.Interface.Enum;

namespace Omnia.Pie.Client.Journal.Interface
{
	public interface IJournal
	{
		void Write(string message, JournalTimestampStyle timestampStyle = JournalTimestampStyle.Time);
        //void DeviceStatus(string message, JournalTimestampStyle timestampStyle = JournalTimestampStyle.Time);

    }
}