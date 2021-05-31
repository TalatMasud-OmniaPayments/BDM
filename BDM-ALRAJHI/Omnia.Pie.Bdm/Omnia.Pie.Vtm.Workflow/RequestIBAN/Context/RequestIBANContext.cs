namespace Omnia.Pie.Vtm.Workflow.RequestIBAN.Context
{
	using System.Collections.Generic;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;

	public class RequestIBANContext : BaseContext, IRequestIBANContext
	{
		public Account SelectedAccount { get ; set; }
		public List<Account> Accounts { get ; set ; }
		public AccountDetailResult AcountDetail { get; set; }
		public CustomerDetail CustomerDetail { get; set; }
		public Attachment Attachment { get; set; }
		public string TSNno { get; set; }
		public bool SelfServiceMode { get; set; }
		public bool SendSms { get; set; }
		public bool SendEmail { get; set; }
	}
}