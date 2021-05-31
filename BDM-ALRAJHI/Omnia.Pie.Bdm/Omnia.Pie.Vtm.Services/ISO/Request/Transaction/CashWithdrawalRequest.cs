using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Services.ISO.Transaction
{
	public class CashWithdrawalRequest : RequestBase
	{
		public TransactionData Transactiondata { get; set; }
		public Terminal Tarminal { get; set; }
		public string Track2 { get; set; }
		public string Pin { get; set; }
		public string IccRequest { get; set; }
		public string TransactionAmount { get; set; }
		public string AccountNumber { get; set; }

		public string AccountType { get; set; }
        public string CurrencyCode { get; set; }
        public string ConvertedAmount { get; set; }
        public string ExchangeRate { get; set; }
	}
}