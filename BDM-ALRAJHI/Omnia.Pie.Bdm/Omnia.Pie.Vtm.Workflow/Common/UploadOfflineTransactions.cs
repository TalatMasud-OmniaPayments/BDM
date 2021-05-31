using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services;
using Omnia.Pie.Vtm.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Service;

namespace Omnia.Pie.Vtm.Workflow.Common
{
    public class UploadOfflineTransactions

    {
        private readonly ITransactionStore _transactionStore = ServiceLocator.Instance.Resolve<ITransactionStore>();

        public async void UploadAllOfflineTransactions(ITransactionService _transactionService)
        {
            //var _transactionService = _container.Resolve<ITransactionService>();
            var offlineTrasnactions = _transactionStore.GetAllOfflineTransaction();
            

            foreach (var transaction in offlineTrasnactions)
            {
                var cashDeposited = _transactionStore.GetOfflineCashDepositedOf(transaction.id);
                var depositedDenominations = _transactionStore.GetOfflineDepositedDenomsOf(transaction.id);

                var _cashDepositToAccount =  await _transactionService.CashDepositAsync(
                        transaction.Username,
                        depositedDenominations,
                        cashDeposited);

                if (_cashDepositToAccount.ResponseCode == "000")
                {
                    bool isUpdated = await _transactionStore.UpdateOfflineTransaction(transaction.id, _cashDepositToAccount.ResponseCode);
                    Console.WriteLine("Upload offline transactions on the server");
                }
                
            }
        }
    }
}
