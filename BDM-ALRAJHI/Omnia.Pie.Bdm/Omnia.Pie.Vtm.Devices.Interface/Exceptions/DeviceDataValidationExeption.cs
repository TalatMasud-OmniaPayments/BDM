using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	[Serializable]
	public class DeviceDataValidationExeption : Exception, IDeviceException
	{
		public DeviceDataValidationExeption(string message)
		    : base(message)
		{
		}

		public DeviceDataValidationExeption(string message, Exception innerException) :
			base(message, innerException)
		{
		}
	}
}
