namespace Omnia.Pie.Vtm.DataAccess.Interface
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;

	public interface IDeviceErrorStore
	{
		Task ClearAll();
		Task Save(DeviceError deviceError);
		Task<string> GetList(string devName);
		Task<List<DeviceError>> GetList(DateTime start, DateTime end);
	}
}