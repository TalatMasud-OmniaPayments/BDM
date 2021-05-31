namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ICoinDispenser : IMediaOutDevice
	{
		int GetAvailableCashAmount();
		List<Entities.CassetteInfo> GetCessettes();
		Task PresentCoinsAsync(int amount);
		Task PresentCoinAndWaitTakenAsync(int amount);
	}
}