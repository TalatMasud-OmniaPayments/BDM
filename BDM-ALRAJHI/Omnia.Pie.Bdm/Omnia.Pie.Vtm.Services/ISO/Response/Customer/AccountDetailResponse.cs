namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class AccountDetail
	{
		public string IBAN { get; set; }
		public string AccountType { get; set; }
		public string BranchId { get; set; }
		public string AccountCurrency { get; set; }
		public string AccountStatus { get; set; }
        public string AccountOpenDate { get; set; }
        public string AccountTitle { get; set; }
		public string AccountNumber { get; set; }
		public string AvailableBalance { get; set; }
	}

	public class AccountDetailResponse : ResponseBase<AccountDetail>
	{
		public AccountDetail AccountDetail { get; set; }
	}
}