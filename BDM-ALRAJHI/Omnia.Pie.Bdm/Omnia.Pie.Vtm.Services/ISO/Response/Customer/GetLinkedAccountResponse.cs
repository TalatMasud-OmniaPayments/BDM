using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class Accounts
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string TypeCode { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
		public string Number { get; set; }
		public string AvailableBalance { get; set; }
		public string AccountingUnitId { get; set; }
		public string ConditionId { get; set; }
	}

	public class GetLinkedAccountResponse : ResponseBase<List<Accounts>>
	{
		public List<Accounts> Accounts { get; set; }
	}
}