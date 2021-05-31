namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.Threading.Tasks;

	public interface ICardDispenser : IDevice, IMediaInDevice
	{
		Task DispenseCardAsync();
		Task EjectCardAndWaitTakenAsync();
		Task RetainCardAsync();
		//Task ReadRawDataAsync();
		//Task WriteRawDataAsync(string track1, string track2, string track3);
		//short GetMaxRetainCount();
		short GetRetainCount();
	}
}