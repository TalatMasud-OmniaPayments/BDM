namespace Omnia.Pie.Vtm.Devices
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using System;

	public class DeviceActivityService : IDeviceObserver, IDeviceActivityService
	{
		public DeviceActivityService()
		{
			LastUserActionTime = DateTime.Now;
		}

		public DateTime LastUserActionTime { get; private set; }

		public void OnUserAction(IDevice device)
		{
			LastUserActionTime = DateTime.Now;
		}
	}
}