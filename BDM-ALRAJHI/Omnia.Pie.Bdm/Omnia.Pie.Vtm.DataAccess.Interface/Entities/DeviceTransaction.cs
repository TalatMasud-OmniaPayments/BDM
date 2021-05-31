using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
    public class DeviceTransaction
    {
        public int id { get; set; }
        public string Username { get; set; }
        /*public string CreditAccountCurrency { get; set; }
        public string CreditAmount { get; set; }
        public string CreditNarrative { get; set; }
        public string DebitAccount { get; set; }
        public string DebitAccountCurrency { get; set; }
        public string Terminal { get; set; }
        public string TransactionData { get; set; }*/
        public string MessageTimestamp { get; set; }
        public int isOffline { get; set; }
        public int isUploaded { get; set; }
        public string OnlineAuthCode { get; set; }
    }

}
