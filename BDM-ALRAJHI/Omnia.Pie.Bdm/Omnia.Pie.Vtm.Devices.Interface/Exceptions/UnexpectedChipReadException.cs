using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	public class UnexpectedChipReadException : Exception
	{
		public UnexpectedChipReadException(string message) : base(message)
		{

		}
	}

	public class EmvValidationExcepton : Exception
	{
		public EmvValidationExcepton(string message) : base(message)
		{

		}
	}

    public class CryptographicErrorException : Exception
    {
        public CryptographicErrorException(string message) : base(message)
        {

        }
    }
}