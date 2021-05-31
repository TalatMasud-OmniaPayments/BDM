
namespace Omnia.Pie.Vtm.Services.ISO.Transaction
{
	public class Cashwithdrawal
	{
		public string CustomerIdentifier { get; set; }
		public string IccResponse { get; set; }
		public string AuthCode { get; set; }
	}

	public class CashWithdrawalResponse : ResponseBase<Cashwithdrawal>
	{
		public Cashwithdrawal CashWithdrawal { get; set; }
	}
}