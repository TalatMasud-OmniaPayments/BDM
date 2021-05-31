namespace Omnia.Pie.Vtm.Devices.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System.Threading.Tasks;

	public interface IChequeAcceptor : IDevice, IMediaInDevice
	{
		Task<Cheque[]> AcceptChequesAsync();
		Task CancelAcceptChequesAsync();
		Task StoreChequesAsync();
		Task RollbackChequesAsync();
		Task<bool> RetractChequesAsync();
		int GetMediaCount(string binID, OperationType opType);
		int GetMediaCount(OperationType opType);
		Cheque[] Cheques { get; }
	}
}