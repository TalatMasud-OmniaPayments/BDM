namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class DeviceData
	{
        public string UnitId { get; set; }
        public string Title { get; set; }
		public int MaxCount { get; set; }
		public int CurrentCount { get; set; }
        public string Status { get; set; }
    }
	public class DashboardConfiguration
	{
		public string DeviceTitle { get; set; }
		public string Status { get; set; }
		public DeviceData[] DeviceData { get; set; }
	}
}
