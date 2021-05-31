using System;

namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface IAuxiliaries : IDevice
	{
		event EventHandler PowerFailure;
		event EventHandler PoweredUp;
	}
}