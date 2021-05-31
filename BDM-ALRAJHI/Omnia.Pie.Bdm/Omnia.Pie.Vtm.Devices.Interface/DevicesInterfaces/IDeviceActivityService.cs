namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System;

	public interface IDeviceActivityService
	{
		DateTime LastUserActionTime { get; }
	}
}