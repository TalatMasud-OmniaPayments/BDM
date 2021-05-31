namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface IMediaInDevice : IMediaDevice
	{
		bool HasMediaInserted { get; }
	}
}