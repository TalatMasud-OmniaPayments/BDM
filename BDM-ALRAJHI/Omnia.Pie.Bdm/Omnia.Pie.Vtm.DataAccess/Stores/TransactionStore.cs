using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Stores
{
    internal class TransactionStore : StoreBase, ITransactionStore
    {
        private TaskCompletionSource<bool> _taskSource;
        public TransactionStore(IResolver container) : base(container)
        {


            /*Task isCreated = ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [DeviceTransaction] (
					[id]                            INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Username]                      TEXT,
                    [DebitAccount]                  TEXT,
                    [DebitAccountCurrency]          TEXT,
                    [CreditAccount]                 TEXT,
					[CreditAccountCurrency]         TEXT,
                    [CreditAmount]                  TEXT,
                    [BagSerialNo]                   TEXT,
                    [BagSerialNo]                   TEXT,
                    [SplitLength]                   TEXT,
                    [UpdateBalanceFlag]             TEXT,
                    [UpdateBalanceRequester]        TEXT,
                    [MachineType]                   TEXT,
                    [SystemFlag]                    TEXT,
                    [BagSerialNo]                   TEXT,
                    


                    [TransactionType]               TEXT,
                    [TransactionNumber]             TEXT,
                    [SessionId]                     TEXT,
                    [SessionLanguage]               TEXT,
                    [MessageTimestamp]              TEXT,
                   





                    [IsOffline]                     NOT NULL CHECK ([IsOffline] IN (0,1)),
                    [IsUploaded]                    NOT NULL CHECK ([IsUploaded] IN (0,1)),
                    [OnlineAuthCode]                TEXT
                    )");*/


            Task isDevTranCreated = ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [tbl_DeviceTransaction] (
					[id]                            INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Username]                      TEXT,
                    [MessageTimestamp]              TEXT,
                    [IsOffline]                     NOT NULL CHECK ([IsOffline] IN (0,1)),
                    [IsUploaded]                    NOT NULL CHECK ([IsUploaded] IN (0,1)),
                    [OnlineAuthCode]                TEXT
                    )");

            Console.WriteLine("isDevTranCreated: " + isDevTranCreated);

            Task isDepositDataCreated = ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [tbl_DepositData] (
					[id]                            INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [tranId]                        INTEGER      NOT NULL,
                    [DebitAccount]                  TEXT,
                    [DebitAccountCurrency]          TEXT,
                    [CreditAccount]                 TEXT,
					[CreditAccountCurrency]         TEXT,
                    [CreditAmount]                  TEXT,
                    [BagSerialNo]                   TEXT,
                    [SplitLength]                   TEXT,
                    [UpdateBalanceFlag]             TEXT,
                    [UpdateBalanceRequester]        TEXT,
                    [MachineType]                   TEXT,
                    [SystemFlag]                    TEXT,
                    [TransactionDateTime]           TEXT
                    )");

            Console.WriteLine("isDepositDataCreated: " + isDepositDataCreated);

            Task isDenomListCreated = ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [tbl_DenominationList] (
					[id]                            INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [tranId]                        INTEGER      NOT NULL,
                    [Type]                          INTEGER,
                    [Count]                         INTEGER
                    )");
             
            Console.WriteLine("isDenomListCreated: " + isDenomListCreated);


        }

        
        public async Task<bool> SaveTranasactionLocally(DeviceTransaction transaction, CashDeposited cashDeposited, List<DepositedDenominations> depositedDenominations) {


            Task isTranCreated = CreateTranasaction(transaction);
            _taskSource = new TaskCompletionSource<bool>();

            if (isTranCreated.Exception == null) {
                var currentTran = getLastId();
                transaction.id = currentTran.Result.id;
                cashDeposited.tranId = transaction.id;

                isTranCreated = SaveCashDeposited(cashDeposited);
                SaveDepositedDenominations(depositedDenominations, transaction.id);


                _taskSource.SetResult(true);
            }
            else
            {

                RollbackTransaction(transaction.id);
                Exception ex = new Exception("Unable to save transaction offline.", isTranCreated.Exception);
                _taskSource.SetException(ex);
                throw ex;
            }

            return await _taskSource.Task;
        }
        private Task CreateTranasaction(DeviceTransaction transaction)
        {

            var isSaved = ExecuteCommand(@"

                INSERT INTO [tbl_DeviceTransaction] (
					[Username],
                    [MessageTimestamp],
                    [IsOffline],
                    [IsUploaded],
                    [OnlineAuthCode]

				) VALUES (
					@Username,
                    @MessageTimestamp,
                    @IsOffline,
                    @IsUploaded,
                    @OnlineAuthCode
                    
				);", transaction);

            return isSaved;



        }

        private Task SaveCashDeposited(CashDeposited cashDeposited)
        {

            var isSaved = ExecuteCommand(@"

                INSERT INTO [tbl_DepositData] (
					[tranId],
                    [DebitAccount],
                    [DebitAccountCurrency],
                    [CreditAccount],
                    [CreditAccountCurrency],
                    [CreditAmount],
                    [BagSerialNo],
                    [SplitLength],
                    [UpdateBalanceFlag],
                    [UpdateBalanceRequester],
                    [MachineType],
                    [SystemFlag],
                    [TransactionDateTime]

				) VALUES (
					@tranId,
                    @DebitAccount,
                    @DebitAccountCurrency,
                    @CreditAccount,
                    @CreditAccountCurrency,
                    @CreditAmount,
                    @BagSerialNo,
                    @SplitLength,
                    @UpdateBalanceFlag,
                    @UpdateBalanceRequester,
                    @MachineType,
                    @SystemFlag,
                    @TransactionDateTime
                    
				);", cashDeposited);

            return isSaved;

    }

        private void SaveDepositedDenominations(List<DepositedDenominations> depositedDenominations, int tranid)
        {
            
            foreach (var depositedDenomination in depositedDenominations)
            {
                depositedDenomination.tranId = tranid;
                SaveDepositedDenomination(depositedDenomination);
            }
            

        }
        private Task SaveDepositedDenomination(DepositedDenominations depositedDenominations)
        {

            var isSaved = ExecuteCommand(@"

                INSERT INTO [tbl_DenominationList] (
					[tranId],
                    [Type],
                    [Count]

				) VALUES (
					@tranId,
                    @Type,
                    @Count
                    
				);", depositedDenominations);

            return isSaved;
    }

        private async Task<DeviceTransaction> getLastId()
        {
            var list = await ExecuteQuery<DeviceTransaction>($@"
				SELECT
					 *
				FROM [tbl_DeviceTransaction]
				ORDER BY id DESC LIMIT 1");



            return list?.FirstOrDefault();
        }

        public List<DeviceTransaction> GetAllOfflineTransaction()
        {

            _taskSource = new TaskCompletionSource<bool>();
            var transactions = OfflineTransactions();

            var offlineTrasns = new List<DeviceTransaction>();

            for (int i = 0; i < transactions.Result.Count; i++)
            {
                offlineTrasns.Add(transactions.Result[i]);
            }

              

                return offlineTrasns;
        }
        

        private async Task<List<DeviceTransaction>> OfflineTransactions()
        {
            var list = await ExecuteQuery<DeviceTransaction>($@"
				SELECT
					 *
				FROM [tbl_DeviceTransaction]
                WHERE
                    [IsUploaded] == 0
                 AND 
                    [IsOffline] == 1");




            return list;
        }


        public List<DepositedDenominations> GetOfflineDepositedDenomsOf(int tranId)
        {

            _taskSource = new TaskCompletionSource<bool>();
            var denoms = OfflineDepositedDenomos(tranId);

            var depositedDenominitions = new List<DepositedDenominations>();

            for (int i = 0; i < denoms.Result.Count; i++)
            {
                depositedDenominitions.Add(denoms.Result[i]);
            }

            return depositedDenominitions;
        }


        private async Task<List<DepositedDenominations>> OfflineDepositedDenomos(int tranId)
        {
            var list = await ExecuteQuery<DepositedDenominations>($@"
				SELECT
					 *
				FROM [tbl_DenominationList]
                 WHERE
                    [tranId] == '{tranId}'");
           return list;
        }

        public CashDeposited GetOfflineCashDepositedOf(int tranId)
        {

            _taskSource = new TaskCompletionSource<bool>();
            var transactions = OfflineCashDeposited(tranId);


            return transactions.Result;
        }


        private async Task<CashDeposited> OfflineCashDeposited(int tranId)
        {
            var list = await ExecuteQuery<CashDeposited>($@"
				SELECT
					 *
				FROM [tbl_DepositData]
                 WHERE
                    [tranId] == '{tranId}'");
            return list?.FirstOrDefault();
        }

        public async Task<bool> UpdateOfflineTransaction(int id, string authCode)
        {

            Task isSaved = UpdateTranasaction(id, authCode);
            _taskSource = new TaskCompletionSource<bool>();

            if (isSaved.Exception == null)
            {
                
                _taskSource.SetResult(true);
            }
            else
            {
                Exception ex = new Exception("Unable to update transaction, please try again.", isSaved.Exception);
                _taskSource.SetException(ex);
                throw ex;
            }

            return await _taskSource.Task;
        }


        private Task UpdateTranasaction(int id, string authCode)
        {

            var isSaved = ExecuteCommand($@"
				UPDATE [tbl_DeviceTransaction]
	 
				SET
                [IsUploaded] = 1,
                [OnlineAuthCode] = '{authCode}'
                WHERE
                    [id] == '{id}'");

            //Console.WriteLine("Update response");
            return isSaved;
        }


        private void RollbackTransaction(int tranid)
        {

            DeleteTransaction(tranid);
            DeleteDepositedData(tranid);
            DeleteDepositedDenomination(tranid);


        }

        public Task DeleteTransaction(int tranid)
        {
            return ExecuteCommand($@"
				DELETE
                FROM [tbl_DeviceTransaction]
                WHERE
                    [id] == '{tranid}'");
        }
        public Task DeleteDepositedData(int tranid)
        {
            return ExecuteCommand($@"
				DELETE
                FROM [tbl_DeviceTransaction]
                WHERE
                    [tranId] == '{tranid}'");
        }
        public Task DeleteDepositedDenomination(int tranid)
        {
            return ExecuteCommand($@"
				DELETE
                FROM [tbl_DeviceTransaction]
                WHERE
                    [tranId] == '{tranid}'");
        }

    }
}
