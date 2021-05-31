namespace Omnia.Pie.Vtm.Services.ISO.Request.ChannelManagement
{
	public class InsertEventRequest : RequestBase
	{
		public string Event { get; set; }
		public string Value { get; set; }
	}
}