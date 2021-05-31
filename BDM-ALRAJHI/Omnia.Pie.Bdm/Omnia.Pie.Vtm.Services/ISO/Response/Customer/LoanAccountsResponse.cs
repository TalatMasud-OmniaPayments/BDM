using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class LoanAccount
	{
		public string Type { get; set; }
		public string Number { get; set; }
		public string Balance { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
	}

	public class LoanAccountsResponse : ResponseBase<List<LoanAccount>>
	{
		public List<LoanAccount> LoanAccounts { get; set; }
		
	}
}
