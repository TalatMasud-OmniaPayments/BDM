using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface
{
    public interface ITransactionStore
    {
        Task<bool> SaveTranasactionLocally(DeviceTransaction transaction, CashDeposited cashDeposited, List<DepositedDenominations> depositedDenominations);
        List<DeviceTransaction> GetAllOfflineTransaction();
        Task<bool> UpdateOfflineTransaction(int id, string authCode);
        CashDeposited GetOfflineCashDepositedOf(int tranId);
        List<DepositedDenominations> GetOfflineDepositedDenomsOf(int tranId);

    }
}
