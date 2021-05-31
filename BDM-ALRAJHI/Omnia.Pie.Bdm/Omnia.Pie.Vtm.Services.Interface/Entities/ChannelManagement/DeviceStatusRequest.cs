
namespace Omnia.Pie.Vtm.Services.Interface.Entities.ChannelManagement
{
	using System.Collections.Generic;

	public class Cassette
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Count { get; set; }
	}

	public class OperationalStatus
	{
		public string InkStatus { get; set; }
		public string PaperStatus { get; set; }
		public List<Cassette> Cassettes { get; set; }
	}

	public class DeviceStatus
	{
		public string DeviceName { get; set; }
		public OperationalStatus OperationalStatus { get; set; }
		public string Status { get; set; }
		public string ErrorCode { get; set; }
	}

    public class DeviceJournalStatus
    {
        public string DeviceName { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }

        public DeviceJournalStatus(string deviceName, string status, string errorCode)
        {
            DeviceName = deviceName;
            Status = status;
            ErrorCode = errorCode;
        }
    }
}