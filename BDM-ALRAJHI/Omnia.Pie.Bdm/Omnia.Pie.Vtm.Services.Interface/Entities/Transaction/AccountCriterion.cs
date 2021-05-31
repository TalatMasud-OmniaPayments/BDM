namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public enum AccountCriterion
	{
		All = 0,
		/// <summary>
		/// Casa will be used for Linked accounts(Casa => Current Account + Saving Account)
		/// </summary>
		Casa = 1,
		Loan = 2,
		Deposit = 3,
		Current = 4,
	}
}