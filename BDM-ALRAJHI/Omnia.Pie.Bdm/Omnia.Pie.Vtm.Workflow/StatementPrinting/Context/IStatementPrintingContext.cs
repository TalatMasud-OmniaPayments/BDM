namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Context
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;
	using System;
	using System.Collections.Generic;

	public interface IStatementPrintingContext
	{
		Account SelectedAccount { get; set; }
		List<Account> Accounts { get; set; }
		double Amount { get; set; }
		DateTime StartDate { get; set; }
		DateTime EndDate { get; set; }
        //List<StatementItem> StatementItems { get; set; }
        List<UserTransaction> UserTransactions { get; set; }
        AccountDetailResult AcountDetail { get; set; }
		GetStatementChargesResult StatementCharges { get; set; }
		ApplyStatementChargesResult ApplyCharges { get; set; }
		string NumberofMonths { get; set; }
		int Period { get; set; }
		bool SelfServiceMode { get; set; }
		CustomerDetail CustomerDetail { get; set; }
	}
}