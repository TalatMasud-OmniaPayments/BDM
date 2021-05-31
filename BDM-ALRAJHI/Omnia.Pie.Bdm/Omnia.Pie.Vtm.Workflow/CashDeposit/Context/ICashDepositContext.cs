using System.Collections.Generic;
namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Context
{
	using Common.Context;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Services.Interface.Entities;
	public interface ICashDepositContext : ICommonContext
	{
		Account SelectedAccount { get; set; }
		List<Account> Accounts { get; set; }
		bool PrintReceipt { get; set; }
		string ReceiptLanguage { get; set; }
		bool ManualAccount { get; set; }
		List<DepositDenomination> DepositedCash { get; set; }
		int TotalNotes { get; set; }
		int TotalAmount { get; set; }
		string Currency { get; set; }
		string AuthCode { get; set; }
		CreditCard CardUsed { get; set; }
		bool SelfService { get; set; }
	}
}
