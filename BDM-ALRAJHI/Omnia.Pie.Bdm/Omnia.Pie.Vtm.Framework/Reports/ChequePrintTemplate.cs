using Microsoft.Reporting.WinForms;
using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.Interface.Reports;
using Omnia.Pie.Vtm.Framework.Reports.Template;
using System.Collections.Generic;

namespace Omnia.Pie.Vtm.Framework.Reports
{
	internal class ChequePrintTemplate : BaseReport<ChequePrintTemplateReportData>
	{
		protected override void InitializeReport(LocalReport report, ChequePrintTemplateReportData reportData)
		{
			base.InitializeReport(report, reportData);

			var dataSource = new ReportDataSource
			{
				Name = nameof(ChequePrintTemplateData) + "Set",
				Value = new List<ChequePrintTemplateData>
				{
					new ChequePrintTemplateData
					{
						AccountNo = reportData.AccountNo,
						ChequeNo = reportData.ChequeNo,
						Counter = reportData.Counter,
						MicrNo = reportData.MicrNo,
						Name = reportData.Name,
						BranchName = reportData.BranchName,
					}
				}
			};

			report.DataSources.Add(dataSource);
		}
	}
}
