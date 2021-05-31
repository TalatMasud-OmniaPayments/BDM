using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
    public class UserTransaction
    {
        public string SessionId { get; set; }
        public string MessageId { get; set; }
        public string TransactionId { get; set; }
        public DateTime? RequestDateTime { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string BagSerialNo { get; set; }
        public string SplitLength { get; set; }
        public string UpdateBalanceFlag { get; set; }
        public string UpdateBalanceRequester { get; set; }

        public string MachineType { get; set; }
        public string SystemFlag { get; set; }
        public DateTime? TransactionDateTime { get; set; }

        public string IPAddress { get; set; }
        public string SessionLang { get; set; }
        public string UserId { get; set; }

        public string TerminalId { get; set; }
        public string BranchId { get; set; }
        public string AccountNo { get; set; }
        public string Amount { get; set; }

        public List<DepositedDenominations> DenominationList { get; set; }

    }
}
