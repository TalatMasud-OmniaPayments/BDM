namespace Omnia.Pie.Vtm.Devices.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System.Threading.Tasks;

	public interface ISignpadScanner : IDevice
	{
		Task<SignPadImage> CaptureSignAsync();
	}
}