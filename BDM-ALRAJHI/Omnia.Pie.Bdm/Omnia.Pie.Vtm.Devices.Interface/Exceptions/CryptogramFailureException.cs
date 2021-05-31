using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	public class CryptogramFailureException : Exception
	{
		public CryptogramFailureException(string message) : base(message)
		{

		}
	}
}
