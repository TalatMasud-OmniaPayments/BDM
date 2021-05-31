using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Collections.Generic;
using System;
using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
using Omnia.Pie.Vtm.Services.Interface.Entities.Transaction;

namespace Omnia.Pie.Vtm.Workflow.StatementPrinting.Context
{
	public class StatementPrintingContext : BaseContext, IStatementPrintingContext
	{
		public Account SelectedAccount { get; set; }
		public List<Account> Accounts { get; set; }
		public double Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		//public List<StatementItem> StatementItems { get ; set; }
        public List<UserTransaction> UserTransactions { get; set; }
        public AccountDetailResult AcountDetail { get; set; }
		public GetStatementChargesResult StatementCharges { get; set ; }
		public ApplyStatementChargesResult ApplyCharges { get ; set ; }
		public string NumberofMonths { get; set; }
		public int Period { get; set; }
		public bool SelfServiceMode { get; set; }
		public CustomerDetail CustomerDetail { get; set; }
	}
}