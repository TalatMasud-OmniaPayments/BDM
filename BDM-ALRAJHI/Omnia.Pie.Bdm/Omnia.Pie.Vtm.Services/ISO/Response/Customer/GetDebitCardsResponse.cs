using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	internal class LinkedAccount

	{
		public string AccountNumber { get; set; }
		public string AccountType { get; set; }
	}

	internal class DebitCard
	{
		public string CardNumber { get; set; }
		public string CardStatus { get; set; }
		public string CardCode { get; set; }
		public List<LinkedAccount> LinkedAccounts { get; set; }
	}

	internal class GetDebitCardsResponse : ResponseBase<List<DebitCard>>
	{
		public List<DebitCard> DebitCards { get; set; }
	}
}