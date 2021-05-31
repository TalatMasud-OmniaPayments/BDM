namespace Omnia.Pie.Vtm.Framework.Reports
{
	using Microsoft.Reporting.WinForms;
	using Omnia.Pie.Vtm.Framework.Base;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Framework.Reports.Template;
	using System.Collections.Generic;

	internal class AccountDetailReport : BaseReport<AccountDetailReportData>
	{
		protected override void InitializeReport(LocalReport report, AccountDetailReportData reportData)
		{
			base.InitializeReport(report, reportData);

			var dataSource = new ReportDataSource
			{
				Name = nameof(AccountDetailData) + "Set",
				Value = new List<AccountDetailData>
				{
					new AccountDetailData
					{
						AccountNumber = reportData.AccountNumber,
						AccountStatus = reportData.AccountStatus,
						AccountType = reportData.AccountType,
						AccountCurrency = reportData.AccountCurrency,
						AccountTitle = reportData.AccountTitle,
						AvailableBalance = reportData.AvailableBalance,
						BranchName = reportData.BranchName,
						AccountOpenedSince = reportData.AccountOpenedSince,
						BranchNameArabic = reportData.BranchNameArabic,
						IBAN = reportData.IBAN,
						ReferenceNo = reportData.ReferenceNo
					}
				}
			};

			report.DataSources.Add(dataSource);
		}
	}
}