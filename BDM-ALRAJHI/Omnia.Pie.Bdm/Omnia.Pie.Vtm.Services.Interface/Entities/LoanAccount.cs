namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class LoanAccount
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string AccountNumber { get; set; }
		public double InterestRate { get; set; }
		public double LoanAmount { get; set; }
		public double AvailableBalance { get; set; }
		public string DueDate { get; set; }
	}
}