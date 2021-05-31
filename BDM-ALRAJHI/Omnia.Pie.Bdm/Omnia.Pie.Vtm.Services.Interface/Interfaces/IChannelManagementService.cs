namespace Omnia.Pie.Vtm.Services.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.ChannelManagement;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IChannelManagementService
	{
		Task<bool> SendDeviceStatus(List<DeviceStatus> devStatus, bool cashReplenishment = false, bool coinReplenishment = false);
		Task<InsertEventResult> InsertEventAsync(string Event, string value);
	}
}