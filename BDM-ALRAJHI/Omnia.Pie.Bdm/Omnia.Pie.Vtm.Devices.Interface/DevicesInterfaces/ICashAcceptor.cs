namespace Omnia.Pie.Vtm.Devices.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public interface ICashAcceptor : IMediaInDevice
	{
        event EventHandler<string> CashAcceptorStatusChanged;
        event EventHandler<string> CashAcceptorUnitChanged;

        bool IsCashInRunning { get; }
		bool HasPendingCashInside { get; }
       bool isCassettteChanged { get; set; }

        Task<Cash> AcceptCashAsync();
		Task<Cash> AcceptMoreCashAsync();
		void CancelAcceptCash();
		Task StoreCashAsync();
		Task RollbackCashAndWaitTakenAsync();
		Task<bool> RetractCashAsync();

        List<CassetteInfo> GetCessettes();
        CashAcceptorCassetteStatus GetCashAcceptorStatus(List<CassetteStatus> cassetteStatuses);
        int AvailableToDepositCashIn();
        int GetMaxStackerLimit();
        void PerformExchange();
        CashAcceptorStackerStatus GetStackerStatus();
        void CheckCassetteStatus();
        string GetOldCassettes();
        string GetUnchangedCassettes(string oldCassettes);
    }
}