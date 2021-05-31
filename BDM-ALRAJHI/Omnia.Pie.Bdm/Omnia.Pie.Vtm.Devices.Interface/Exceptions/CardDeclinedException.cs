using System;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
	public class CardDeclinedException : Exception
	{
		public CardDeclinedException(string message) : base(message)
		{

		}
	}
}
