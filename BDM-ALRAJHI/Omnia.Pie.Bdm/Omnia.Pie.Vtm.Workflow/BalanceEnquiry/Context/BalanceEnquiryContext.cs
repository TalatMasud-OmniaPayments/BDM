namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context
{
	using System.Collections.Generic;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService;

	public class BalanceEnquiryContext : BaseContext, IBalanceEnquiryContext
	{
		public Account SelectedAccount { get; set; }
		public List<Account> Accounts { get; set; }
		public AccountDetailResult AcountDetail { get; set; }
		public bool SelfServiceMode { get; set; }
		public GenerateTSNResult TSNno { get; set; }
	}
}