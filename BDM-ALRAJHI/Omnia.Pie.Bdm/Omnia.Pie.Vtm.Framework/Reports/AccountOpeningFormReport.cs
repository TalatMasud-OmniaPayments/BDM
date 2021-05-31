namespace Omnia.Pie.Vtm.Framework.Reports
{
	using Microsoft.Reporting.WinForms;
	using Omnia.Pie.Vtm.Framework.Base;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using Omnia.Pie.Vtm.Framework.Reports.Template;
	using System.Collections.Generic;

	internal class AccountOpeningFormReport : BaseReport<AccountOpeningFormReportData>
	{
		protected override void InitializeReport(LocalReport report, AccountOpeningFormReportData reportData)
		{
			base.InitializeReport(report, reportData);

			var dataSource = new ReportDataSource
			{
				Name = nameof(AccountOpeningFormData) + "Set",
				Value = new List<AccountOpeningFormData>
				{
					new AccountOpeningFormData
					{
						CustomerId = reportData.CustomerId,
						CustomerName = reportData.CustomerName,
						AccountType = reportData.AccountType,
						AccountCurrency = reportData.AccountCurrency,
						IsChequeBook = reportData.IsChequeBook,
						Signature1Base64Content = reportData.Signature1?.ToBase64String(),
						Signature2Base64Content = reportData.Signature2?.ToBase64String(),
						CheckedById = reportData.CheckedById,
						CheckedByName = reportData.CheckedByName
					}
				}
			};

			report.DataSources.Add(dataSource);
		}
	}
}