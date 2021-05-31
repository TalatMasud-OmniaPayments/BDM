namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class ChequeDeposit
	{
		public string ReferenceNumber { get; set; }
		public string Status { get; set; }
	}

	public class ChequeDepositResponse : ResponseBase<ChequeDeposit>
	{
		public ChequeDeposit ChequeDeposit { get; set; }
	}
}