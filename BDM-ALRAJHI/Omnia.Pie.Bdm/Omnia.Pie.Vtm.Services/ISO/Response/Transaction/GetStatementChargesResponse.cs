using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class Charge
	{
		public string ChargeAmount { get; set; }
		public string ChargeCurrency { get; set; }
	}

	public class GetStatementChargesResponse : ResponseBase<List<Charge>>
	{
		public Charge Charge { get; set; }
	}
}