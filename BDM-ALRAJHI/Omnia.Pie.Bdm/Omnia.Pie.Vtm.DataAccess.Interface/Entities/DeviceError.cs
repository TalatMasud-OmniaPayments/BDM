namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
	using System;

	public class DeviceError
	{
		public string Source { get; set; }
		public DateTime Created { get; set; }
		public string Message { get; set; }
		public short StatusSent { get; set; }
	}
}