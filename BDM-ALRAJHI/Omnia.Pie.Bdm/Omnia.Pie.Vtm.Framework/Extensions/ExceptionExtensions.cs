namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using System;

	public static class ExceptionExtensions
	{
		public static Exception ToDeviceException(this Exception ex)
		{
			return ex is IDeviceException ? ex : new DeviceMalfunctionException(ex.Message, ex);
		}
	}
}