using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Framework.Reports.Template
{
    public class NewAccountData
    {
        public string WelcomeMessage { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountIBAN { get; set; }
        public string AccountType { get; set; }
        public string AccountCurrency { get; set; }
        public bool IsChequeBook { get; set; }
        public string Signature1Base64Content { get; set; }
        public string Signature2Base64Content { get; set; }
        public string CustomerEidNo { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPassport { get; set; }

        public string CheckedById { get; set; }
        public string CheckedByName { get; set; }

    }
}
