namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System;
	using System.Collections.Generic;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;

	public interface ICashDispenser : IMediaOutDevice
	{
		Task PresentCashAndWaitTakenAsync(int amount);
		Task PresentCashAndWaitTakenAsync(int amount, int[] notesCount);
		Task<bool> RetractCashAsync();
		int GetAvailableCashAmount();
        CashDispenserCassetteStatus GetCashDispenserStatus();
        List<CassetteStatus> GetDispensableCassettesStatus();

        List<List<Denomination>> GetDenominations(int highest, int sum, int goal, List<int> notes = null);
		int GetCassettesCount();
		int[] GetDenominationBreakDown(List<Denomination> denom);
		List<CassetteInfo> GetCessettes();
        void CheckCassetteStatus();

    }
}
