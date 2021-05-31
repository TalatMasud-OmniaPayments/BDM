using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
    public class DepositedDenominations
    {
        public int tranId { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
    }
    public class CashDeposited
    {
        public int tranId { get; set; }
        public string DebitAccount { get; set; }
        public string DebitAccountCurrency { get; set; }
        public string CreditAccount { get; set; }
        public string CreditAccountCurrency { get; set; }
        public string CreditAmount { get; set; }
        public string BagSerialNo { get; set; }
        public string SplitLength { get; set; }
        public string UpdateBalanceFlag { get; set; }
        public string UpdateBalanceRequester { get; set; }

        public string MachineType { get; set; }
        public string SystemFlag { get; set; }
        public string TransactionDateTime { get; set; }
    }
}
