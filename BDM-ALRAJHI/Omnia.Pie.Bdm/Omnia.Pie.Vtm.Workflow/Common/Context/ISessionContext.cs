namespace Omnia.Pie.Vtm.Workflow.Common.Context
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System.Collections.Generic;

	public interface ISessionContext
	{
		Card CardUsed { get; set; }
		string EIdNumber { get; set; }
		string CardFdk { get; set; }
		string CustomerIdentifier { get; set; }
		string Pin { get; set; }
		bool CifAuth { get; set; }

		string AccountNumber { get; set; }
		double? BalanceAmount { get; set; }
		List<string> TransactionHistory { get; set; }
		string Name { get; set; }

		bool SelfCallMod { get; set; }
		bool InCall { get; set; }
	}
}