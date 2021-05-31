namespace Omnia.Pie.Vtm.Workflow.BalanceEnquiry.Context
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.CommunicationService;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System.Collections.Generic;

	public interface IBalanceEnquiryContext
	{
		Account SelectedAccount { get; set; }
		List<Account> Accounts { get; set; }
		AccountDetailResult AcountDetail { get; set; }
		bool SelfServiceMode { get; set; }
		GenerateTSNResult TSNno { get; set; }
	}
}