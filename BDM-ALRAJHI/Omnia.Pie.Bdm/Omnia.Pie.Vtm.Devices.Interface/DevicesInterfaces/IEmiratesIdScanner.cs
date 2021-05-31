namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public interface IEmiratesIdScanner : IMediaInDevice
	{
		Task<ScannedEmiratesId> ScanEmiratesIdAsync();
		Task CancelScan();
		Task EjectEmiratesIdAsync();
		bool IsEmiratesIdInside { get; }
	}
}
