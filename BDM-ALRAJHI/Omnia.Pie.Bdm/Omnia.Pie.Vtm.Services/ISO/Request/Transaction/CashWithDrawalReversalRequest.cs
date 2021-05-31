using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class CashWithDrawalReversalRequest : RequestBase
	{
		public string AuthCode { get; set; }
		public string ReversalReason { get; set; }
		public string Track2 { get; set; }
	}
}
