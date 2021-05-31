using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
    public class UserTransactionRequest : RequestBase
    {
        public string Username { get; set; }
        public string StatementDateFrom { get; set; }
        public string AccountNumber { get; set; }
        public string StatementDateTo { get; set; }
        public string NumOfTransactions { get; set; }
        public string SortAs { get; set; }
        
    }
}
