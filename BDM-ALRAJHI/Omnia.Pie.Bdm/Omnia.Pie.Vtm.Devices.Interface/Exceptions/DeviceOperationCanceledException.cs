using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	[Serializable]
	public class DeviceOperationCanceledException : OperationCanceledException, IDeviceException
	{
		public DeviceOperationCanceledException(string message) : base(message) { }

		public DeviceOperationCanceledException(string operation, int result)
		    : this($"Operation {operation} is canceled") { }

		public DeviceOperationCanceledException(string message, Exception innerException) : base(message, innerException) { }
	}
}
