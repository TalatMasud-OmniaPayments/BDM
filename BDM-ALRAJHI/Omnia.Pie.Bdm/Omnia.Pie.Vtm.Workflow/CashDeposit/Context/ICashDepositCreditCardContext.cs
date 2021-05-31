namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Context
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Collections.Generic;

	interface ICashDepositCreditCardContext : ICommonContext
	{
		string CardNumber { get; set; }
		bool PrintReceipt { get; set; }
		string ReceiptLanguage { get; set; }
		bool ManualAccount { get; set; }
		List<DepositDenomination> DepositedCash { get; set; }
		int TotalNotes { get; set; }
		int TotalAmount { get; set; }
		string Currency { get; set; }
		string AuthCode { get; set; }
	}
}
