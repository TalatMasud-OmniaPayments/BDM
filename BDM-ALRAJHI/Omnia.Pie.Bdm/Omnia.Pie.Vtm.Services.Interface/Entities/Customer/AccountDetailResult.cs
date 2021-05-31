namespace Omnia.Pie.Vtm.Services.Interface.Entities.Customer
{
	public class AccountDetailResult
	{
		public string IBAN { get; set; }
		public string AccountType { get; set; }
		public string BranchId { get; set; }
		public string AccountCurrency { get; set; }
		public string AccountStatus { get; set; }
        public string AccountOpenDate { get; set; }
        public string AccountTitle { get; set; }
		public string AccountNumber { get; set; }
		public double? AvailableBalance { get; set; }
		public string ResponseCode { get; set; }
	}
}