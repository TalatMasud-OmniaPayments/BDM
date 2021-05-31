﻿using Microsoft.Reporting.WinForms;
using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.Extensions;
using Omnia.Pie.Vtm.Framework.Interface.Reports;
using Omnia.Pie.Vtm.Framework.Reports.Template;
using System.Collections.Generic;
using System.Linq;

namespace Omnia.Pie.Vtm.Framework.Reports
{
	internal class StatementPrintReport : BaseReport<StatementReportData>
	{
		protected override void InitializeReport(LocalReport report, StatementReportData reportData)
		{
			base.InitializeReport(report, reportData);

			var statementDataSource = new ReportDataSource
			{
				Name = nameof(StatementData) + "Set",
				Value = new List<StatementData>
				{
					new StatementData
					{
						AccountNumber = reportData.AccountNumber?.ToMaskedCardNumber(),
						AccountIban = reportData.AccountIban?.ToMaskedCardNumber(),
						AccountType = reportData.AccountType,
						AccountCurrency = reportData.AccountCurrency,
						StartDate = reportData.StartDate,
						EndDate = reportData.EndDate,
						NumberOfMonths = reportData.NumberOfMonths,
						BranchLocation = reportData.BranchLocation,
						CustomerName = reportData.CustomerName,
						City = reportData.City,
						POBox = reportData.POBox
					}
				}
			};

			report.DataSources.Add(statementDataSource);

			var statementItemsDataSource = new ReportDataSource
			{
				Name = nameof(StatementItem) + "DataSet",
				Value = reportData.Items ?? new List<StatementItem>()
			};

			report.DataSources.Add(statementItemsDataSource);
		}
	}
}
