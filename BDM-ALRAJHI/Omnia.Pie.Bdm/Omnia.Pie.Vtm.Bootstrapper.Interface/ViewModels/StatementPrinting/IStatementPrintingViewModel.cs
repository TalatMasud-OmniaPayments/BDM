namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System;
	using System.Collections.Generic;

	public interface IStatementPrintingViewModel : IExpirableBaseViewModel
	{
		double Amount { get; set; }
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		DateTime StartDate { get; set; }
		DateTime EndDate { get; set; }
		string MonthsNumber { get; set; }
		int NumberofMonths { get; set; }

	}
	public class StatementPeriod
	{
		public StatementPeriod()
		{

		}

		public string Period { get; set; }
		//public string Charges { get; set; }
		public int Number { get; set; }
	}
}