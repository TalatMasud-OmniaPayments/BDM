using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
    public static partial class JournalExtensions
    {
        public static void DeviceError(this IJournal journal, string deviceName, string status, string errorCode)
        {

            journal.Write($"{deviceName} {status}");

            /*if (errorCode.Length > 0)
            {
                journal.Write($"{deviceName} {status} with ErrorCode {errorCode}.");

            }
            else
            {
                journal.Write($"{deviceName} {status}");

            }*/
        }
    }
}
