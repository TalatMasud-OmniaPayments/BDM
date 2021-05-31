using Omnia.Pie.Client.Journal.Interface.Enum;
using Omnia.Pie.Vtm.Devices.Interface;
using System;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void RestartMachine(this IJournal journal)
		{
			journal.Write($"Restarting the Machine.");
		}
        public static void CloseApplication(this IJournal journal)
        {
            journal.Write($"BDM Application closed.");
        }
        public static void ExitedVdmMode(this IJournal journal)
		{
			journal.Write($"Exited Vdm Mode.");
		}
		public static void EnteringVdmMode(this IJournal journal)
		{
			journal.Write($"Entering Vdm Mode."); 
		}
		public static void Reset(this IJournal journal, string deviceName)
		{
			journal.Write($"{deviceName} has been Reset.");
		}
		public static void Initialized(this IJournal journal, string deviceName)
		{
			journal.Write($"{deviceName} has been Initialized.");
		}
		public static void DoorClosed(this IJournal journal)
		{
			journal.Write($"All Door Closed.");
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
		}
        public static void DoorStatus(this IJournal journal, string doorName, DoorStatus doorStatus)
        {
            journal.Write($" {doorName} Door {doorStatus}.");
        }
	}
}