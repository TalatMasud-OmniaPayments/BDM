﻿using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class CreditCard 
	{
		public string Type { get; set; }
		public string Number { get; set; }
		public string Balance { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
		public string CardLimit { get; set; }
		public string StatementMinimumDue { get; set; }
		public string CardCode { get; set; }
	}
	public class CreditCardResponse : ResponseBase<List<CreditCard>>
	{
		public List<CreditCard> CreditCards { get; set; }
	}
}