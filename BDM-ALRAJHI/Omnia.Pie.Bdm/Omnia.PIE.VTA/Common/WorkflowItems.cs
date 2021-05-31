namespace Omnia.PIE.VTA
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Collections.Generic;

	public class WorkflowItems
	{
		public List<WorkflowItem> ItemsList = new List<WorkflowItem>();

		public WorkflowItems()
		{
			#region "Grouping"

			ItemsList.Add(new WorkflowItem() { Id = 1, ItemName = "Withdrawals", ItemLabel = "Withdrawals" });
			ItemsList.Add(new WorkflowItem() { Id = 2, ItemName = "Deposits", ItemLabel = "Deposits"});
			ItemsList.Add(new WorkflowItem() { Id = 3, ItemName = "Inquiries", ItemLabel = "Inquiries" });
			//ItemsList.Add(new WorkflowItem() { Id = 4, ItemName = "Payments", ItemLabel = "Payments" });
			//ItemsList.Add(new WorkflowItem() { Id = 5, ItemName = "Transfers", ItemLabel = "Transfers" });
			ItemsList.Add(new WorkflowItem() { Id = 6, ItemName = "Requests", ItemLabel = "Requests" });
			//ItemsList.Add(new WorkflowItem() { Id = 7, ItemName = "Leads", ItemLabel = "Leads", ParentId = -20 });

			#endregion

			#region "Withdrawals"

			ItemsList.Add(new WorkflowItem()
			{
				Id = 11,
				ParentId = 1,
				ItemName = "CashWithdrawal",
				ItemLabel = "Cash Withdrawal",
				CommandType = (int)StatusEnum.CashWithdrawal,
			});

			#endregion

			#region "Deposits"

			ItemsList.Add(new WorkflowItem()
			{
				Id = 11,
				ParentId = 2,
				ItemName = "CashDepositAccount",
				ItemLabel = "Cash Deposit to Account",
				CommandType = (int)StatusEnum.CashDepositAccount,
			});

			ItemsList.Add(new WorkflowItem()
			{
				Id = 12,
				ParentId = 2,
				ItemName = "CreditCardPaymentWithCash",
				ItemLabel = "Credit Card Payment With Cash",
				CommandType = (int)StatusEnum.CreditCardPaymentWithCash,
			});

			ItemsList.Add(new WorkflowItem()
			{
				Id = 13,
				ParentId = 2,
				ItemName = "ChequeDepositAccount",
				ItemLabel = "Cheque Encashment",
				CommandType = (int)StatusEnum.ChequeDepositAccount,
			});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 14,
			//	ParentId = 2,
			//	ItemName = "ChequeDepositCreditCard",
			//	ItemLabel = "Cheque Deposit to Credit Card",
			//	CommandType = (int)StatusEnum.ChequeDepositCreditCard,
			//});

			#endregion

			#region "Inquiries"

			ItemsList.Add(new WorkflowItem()
			{
				Id = 30,
				ParentId = 3,
				ItemName = "AccountInquiry",
				ItemLabel = "Account Inquiry",
				CommandType = (int)StatusEnum.AccountInquiryAuthenticateCard,
			});

			ItemsList.Add(new WorkflowItem()
			{
				Id = 31,
				ParentId = 3,
				ItemName = "Documents",
				ItemLabel = "Documents",
				CommandType = (int)StatusEnum.Documents,
			});

			#endregion

			#region "Payments"

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 40,
			//	ParentId = 4,
			//	ItemName = "CreditCardPaymentWithAccount",
			//	ItemLabel = "Credit Card Payment With Account",
			//	CommandType = (int)StatusEnum.CreditCardPaymentWithAccount,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 41,
			//	ParentId = 4,
			//	ItemName = "BillPaymentWithCreditCard",
			//	ItemLabel = "Bill Payment With Credit Card",
			//	CommandType = (int)StatusEnum.BillPaymentWithCreditCard,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 42,
			//	ParentId = 4,
			//	ItemName = "BillPaymentWithAccount",
			//	ItemLabel = "Bill Payment With Account",
			//	CommandType = (int)StatusEnum.BillPaymentWithAccount,
			//});

			#endregion

			#region "Transfers"

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 20,
			//	ParentId = 5,
			//	ItemName = "FundsTransferBetweenOwnAccounts",
			//	ItemLabel = "Funds transfer between own accounts",
			//	CommandType = (int)StatusEnum.FundsTransferOwnAccountUseDebitCardAuthenticateCard,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 21,
			//	ParentId = 5,
			//	ItemName = "FundsTransfertoOtherAccounts",
			//	ItemLabel = "Funds transfer to other accounts",
			//	CommandType = (int)StatusEnum.FundsTransferOtherAccountUseDebitCardAuthenticateCard,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//    Id = 22,
			//    ParentId = 5,
			//    ItemName = "CreateBeneficiary",
			//    ItemLabel = "Create beneficiary for other accounts transfer",
			//    CommandType = (int)StatusEnum.CreateBeneficiaryUseDebitCardAuthenticateCard,
			//});

			#endregion

			#region "Requests"

			ItemsList.Add(new WorkflowItem()
			{
				Id = 50,
				ParentId = 6,
				ItemName = "ChequePrinting",
				ItemLabel = "Cheque Printing",
				CommandType = (int)StatusEnum.ChequePrintingEligibility,
			});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 52,
			//	ParentId = 6,
			//	ItemName = "UpdateCustomerDetails",
			//	ItemLabel = "Update Customer Details",
			//	CommandType = (int)StatusEnum.UpdateCustomerDetailsStarting,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 51,
			//	ParentId = 6,
			//	ItemName = "AccountOpeningRequest",
			//	ItemLabel = "Account Opening Request",
			//	CommandType = (int)StatusEnum.AdditionalAccountOpeningRequestByRT,
			//});

			#endregion

			#region "Leads"

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 31,
			//	ParentId = 7,
			//	ItemName = "AccountLeads",
			//	ItemLabel = "Account",
			//	CommandType = (int)StatusEnum.AccountLeadStarting,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 32,
			//	ParentId = 7,
			//	ItemName = "LoanLeads",
			//	ItemLabel = "Loan",
			//	CommandType = (int)StatusEnum.LoanLeadStarting,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 33,
			//	ParentId = 7,
			//	ItemName = "CreditCardLeads",
			//	ItemLabel = "Credit Card",
			//	CommandType = (int)StatusEnum.CreditCardLeadStarting,
			//});

			//ItemsList.Add(new WorkflowItem()
			//{
			//	Id = 33,
			//	ParentId = 7,
			//	ItemName = "WealthLeads",
			//	ItemLabel = "Wealth",
			//	CommandType = (int)StatusEnum.WealthLeadStarting,
			//});

			#endregion
		}
	}

	public class WorkflowItem
	{
		public int Id { get; set; }
		public string ItemName { get; set; }
		public string ItemLabel { get; set; }
		public int ParentId { get; set; } = 0;
		public int CommandType { get; set; }
		public int UseDebitCard { get; set; }
		public int UseEmiratesId { get; set; }
	}
}
