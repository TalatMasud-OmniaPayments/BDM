using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{

    public class DenominationList
    {
        public int Type { get; set; }
        public int Count { get; set; }
    }
    public class DepositData
    {
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
    public class CashDepositRequest : RequestBase
	{
        public string Username { get; set; }
        public DepositData DepositData { get; set; }
        public List<DenominationList> DenominationList { get; set; }
    }
}