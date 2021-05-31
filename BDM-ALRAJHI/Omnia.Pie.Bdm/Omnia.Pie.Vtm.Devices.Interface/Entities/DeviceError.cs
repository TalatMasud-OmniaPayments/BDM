namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	using System;

	public class DeviceError
	{
		public string Message { get; set; }
		public string Code { get; set; }
		public DateTime Time { get; set; }
	}
}