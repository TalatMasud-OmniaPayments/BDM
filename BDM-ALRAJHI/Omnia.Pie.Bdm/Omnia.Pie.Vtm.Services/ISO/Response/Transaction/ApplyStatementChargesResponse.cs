using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class ApplyStatementCharge
	{
		public string ReferenceNum { get; set; }
		public string Status { get; set; }
	}

	public class ApplyStatementChargesResponse : ResponseBase<List<ApplyStatementCharge>>
	{
		public ApplyStatementCharge ApplyStatementCharge { get; set; }
	}
}