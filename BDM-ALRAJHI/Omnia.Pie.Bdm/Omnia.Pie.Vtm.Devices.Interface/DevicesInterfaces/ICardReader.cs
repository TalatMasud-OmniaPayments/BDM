namespace Omnia.Pie.Vtm.Devices.Interface
{
    using System;
    using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public interface ICardReader : IDevice, IMediaInDevice
	{
		/// <summary>
		/// true for EID 
		/// </summary>
		/// <param name="readChip"></param>
		/// <returns></returns>
		Task<Card> ReadCardAsync(bool readChip);
		Task EjectCardAndWaitTakenAsync();
		Task RetainCardAsync();
		Task<IEmvData> GetEmvDataAsync(int amount, string transactionType);
		Card Card { get; }
		void CancelReadCard();
		bool IsCardInside { get; }
		short GetMaxRetainCount();
		short GetRetainCount();

        event EventHandler<string> CardReaderStatusChanged;

    }
}