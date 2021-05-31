using Microsoft.Reporting.WinForms;
using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.Reports.Template;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Framework.Interface.Reports;

namespace Omnia.Pie.Vtm.Framework.Reports
{
	internal class LCAndNLCReport : BaseReport<LCAndNLCReportData>
	{
		protected override void InitializeReport(LocalReport report, LCAndNLCReportData reportData)
		{
			base.InitializeReport(report, reportData);

			var dataSource = new ReportDataSource
			{
				Name = nameof(LCAndNLCData) + "Set",
				Value = new List<LCAndNLCData>
				{
					new LCAndNLCData
					{
						CIF = reportData.CIF,
						CustomerName = reportData.CustomerName,
						TransactionNo = reportData.TransactionNo,
						TypeofRequest = reportData.TypeofRequest,
						SignatureBase64Content = reportData.SignatureBase64Content
					}
				}
			};

			report.DataSources.Add(dataSource);
		}
	}
}