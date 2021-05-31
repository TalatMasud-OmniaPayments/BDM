namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	using System.Collections.Generic;

	public class LinkedAccount
	{
		public string AccountNumber { get; set; }
		public string AccountType { get; set; }
	}

	public class DebitCard
	{
		public string CardNumber { get; set; }
		public string CardStatus { get; set; }
		public string CardCode { get; set; }
		public List<LinkedAccount> LinkedAccounts { get; set; }
	}
}