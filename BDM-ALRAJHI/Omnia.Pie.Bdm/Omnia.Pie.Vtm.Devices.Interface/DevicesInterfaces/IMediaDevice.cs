namespace Omnia.Pie.Vtm.Devices.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System;

	public interface IMediaDevice : IDevice
	{
		MediaUnit[] GetMediaInfo();
		void SetMediaInfo(int[] ids, int[] counts);
		event EventHandler MediaChanged;
	}
}