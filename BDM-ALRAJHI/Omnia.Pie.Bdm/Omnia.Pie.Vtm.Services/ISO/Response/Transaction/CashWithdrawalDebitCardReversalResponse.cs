namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class CashwithdrawalReversal
	{
		public string AuthCode { get; set; }
	}

	public class CashWithdrawalDebitCardReversalResponse : ResponseBase<CashwithdrawalReversal>
	{
		public CashwithdrawalReversal ResponsecCode { get; set; }
		
	}
}