namespace Omnia.Pie.Vtm.Services.Interface.Entities.Transaction
{
	public class CashWithdrawal
	{
		public string AuthCode { get; set; }
		public double? AvailableBalance { get; set; }
		/// <remarks>
		/// The value makes sense for cash withdrawal using debit / credit card only.
		/// It's null for cash withdrawal using Emirates ID.
		/// </remarks>
		public string IccData { get; set; }
	}
}