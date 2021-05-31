namespace Omnia.Pie.Vtm.Services.ISO.Authentication
{
	using System.Collections.Generic;

	public class Account
	{
		public string Nickname { get; set; }
		public string AccountType { get; set; }
		public string Branch { get; set; }
        public string Logo { get; set; }
        public string AccountNo { get; set; }
	
	}

	public class AccountsResponse : ResponseBase<List<Account>>
	{
		public List<Account> Accounts { get; set; }
	}
}