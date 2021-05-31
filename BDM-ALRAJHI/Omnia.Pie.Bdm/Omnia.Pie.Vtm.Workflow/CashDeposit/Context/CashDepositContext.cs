using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Context
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Services.Interface.Entities;
	public class CashDepositContext : BaseContext, ICashDepositContext
	{
		public Account SelectedAccount { get; set; }
		public List<Account> Accounts { get; set; }
		public bool PrintReceipt { get; set; }
		public string ReceiptLanguage { get; set; }
		public bool ManualAccount { get; set; }
		public bool IsCanceled { get; set; }
		public List<DepositDenomination> DepositedCash { get; set; }
		public int TotalNotes { get; set; }
		public int TotalAmount { get; set; }
		public string Currency { get; set; }
		public string AuthCode { get; set; }
		public CreditCard CardUsed { get; set; }
		public bool SelfService { get; set; }
        public string Username { get; set; }
    }
}
