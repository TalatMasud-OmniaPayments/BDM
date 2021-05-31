using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	[Serializable]
	public class DeviceTimeoutException : TimeoutException, IDeviceException
	{
		public DeviceTimeoutException(string operation)
		    : base($"Operation {operation} has timed-out") { }

		public DeviceTimeoutException(string message, Exception innerException) : base(message, innerException) { }
	}
}
