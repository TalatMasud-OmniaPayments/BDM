

namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
    using System.Windows.Media.Imaging;
    public class NewAccountReportData
    {
        public string WelcomeMessage { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountIBAN { get; set; }
        public string AccountType { get; set; }
        public string AccountCurrency { get; set; }
        public bool IsChequeBook { get; set; }
        public BitmapSource Signature1 { get; set; }
        public BitmapSource Signature2 { get; set; }
        public string CustomerEidNo { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPassport { get; set; }


        public string CheckedById { get; set; }
        public string CheckedByName { get; set; }
    }
}
