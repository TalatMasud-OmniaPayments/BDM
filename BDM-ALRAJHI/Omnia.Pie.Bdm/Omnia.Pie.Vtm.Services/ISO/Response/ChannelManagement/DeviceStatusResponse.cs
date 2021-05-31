
namespace Omnia.Pie.Vtm.Services.ISO.Response.ChannelManagement
{
	using Omnia.Pie.Vtm.Services.Interface.Entities.ChannelManagement;

	public class DeviceStatusResponse : ResponseBase<DeviceStatus>
	{
		public bool Result { get; set; }
	}
}