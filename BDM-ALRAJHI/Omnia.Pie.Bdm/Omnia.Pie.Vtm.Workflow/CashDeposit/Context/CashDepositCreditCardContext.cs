using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Context
{
	class CashDepositCreditCardContext : BaseContext, ICashDepositCreditCardContext
	{
		public string CardNumber { get; set; }
		public bool PrintReceipt { get; set; }
		public string ReceiptLanguage { get; set; }
		public bool ManualAccount { get; set; }
		public List<DepositDenomination> DepositedCash { get; set; }
		public int TotalNotes { get; set; }
		public int TotalAmount { get; set; }
		public string Currency { get; set; }
		public string AuthCode { get; set; }
		public bool IsCanceled { get; set; }
	}
}
