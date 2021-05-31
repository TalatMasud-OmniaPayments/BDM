namespace Omnia.Pie.Vtm.Workflow.Common.Context
{
	using System.Collections.Generic;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public class SessionContext : ISessionContext
	{
		public Card CardUsed { get; set; }
		public string EIdNumber { get; set; }
		public string CardFdk { get; set; }

		public string CustomerIdentifier { get; set; }
		public string Pin { get; set; }
		public bool CifAuth { get; set; }

		public string AccountNumber { get; set; }
		public double? BalanceAmount { get; set; }
		public List<string> TransactionHistory { get; set; }
		public string Name { get; set; }
		public bool SelfCallMod { get; set; }

		public bool InCall { get; set; }
	}
}