namespace Omnia.Pie.Vtm.Devices.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

	public interface IDevice : IDisposable
	{
		void Initialize();
		void AddObserver(IDeviceObserver deviceObserver);
		void RemoveObserver(IDeviceObserver deviceObserver);
		Task ResetAsync();
		Task TestAsync();
		Task PlayBeepAsync();
		Task StopBeepAsync();
		DeviceStatus Status { get; set; }
		bool IsAvailable { get; }
		DeviceError GetDeviceError(DeviceShortName name);
        
    }
}