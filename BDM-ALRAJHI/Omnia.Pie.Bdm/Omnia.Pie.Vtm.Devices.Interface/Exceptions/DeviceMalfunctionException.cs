using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	[Serializable]
	public class DeviceMalfunctionException : Exception, IDeviceException
	{
        public int ErrorResultNumber { get; private set; }

        public string ErrorResult { get; private set; }

		public DeviceMalfunctionException(string message)
		    : base(message)
		{
		}

		public DeviceMalfunctionException(string operation, int result)
		    : this($"Operation {operation} has failed with result={result}.")
		{
            ErrorResultNumber = result;
        }

		public DeviceMalfunctionException(string operation, string result) : 
			this($"Operation {operation} has failed with result={result}.")
		{
            ErrorResult = result;
        }

		public DeviceMalfunctionException(string message, Exception innerException): 
			base(message, innerException)
		{
		}
	}
}
