namespace Omnia.Pie.Vtm.Services.ISO.Response.ChannelManagement
{
	public class InsertEvent
	{
		public string Result { get; set; }
	}

	public class InsertEventResponse : ResponseBase<InsertEvent>
	{
		public InsertEvent InsertEvent { get; set; }
	}
}