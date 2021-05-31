namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Context
{
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System.Collections.Generic;

	public interface IRequestIBANContext
	{
		Account SelectedAccount { get; set; }
		List<Account> Accounts { get; set; }
		AccountDetailResult AcountDetail { get; set; }
		CustomerDetail CustomerDetail { get; set; }
		Attachment Attachment { get; set; }
		string TSNno { get; set; }
		bool SelfServiceMode { get; set; }
		bool SendSms { get; set; }
		bool SendEmail { get; set; }
	}
}