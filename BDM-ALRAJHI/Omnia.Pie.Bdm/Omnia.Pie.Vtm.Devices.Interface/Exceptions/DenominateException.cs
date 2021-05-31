using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	[Serializable]
	public class DenominateException : Exception, IDeviceException
	{
		public DenominateException(int amount) : this($"Cant denominate amount: {amount}")
		{

		}

		public DenominateException(string message) : base(message)
		{

		}

		public DenominateException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}