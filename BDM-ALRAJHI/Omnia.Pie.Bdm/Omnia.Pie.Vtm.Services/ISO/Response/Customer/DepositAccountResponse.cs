namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	using System.Collections.Generic;

	public class DepositAccount
	{
		public string Type { get; set; }
		public string Number { get; set; }
		public string Balance { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
	}

	public class DepositAccountResponse : ResponseBase<List<DepositAccount>>
	{
		public List<DepositAccount> DepositAccounts { get; set; }
	}
}