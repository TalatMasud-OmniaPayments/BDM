namespace Omnia.Pie.Vtm.Framework.Base
{
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using System;
	using System.Globalization;
	using System.Linq;

	public class ReportsManager : IReportsManager
	{
		private IResolver _container { get; }
		public CultureInfo Culture { get; set; }

		public ReportsManager(IResolver container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public IReport CreateReport<TReportData>(TReportData reportData)
		{
			if(Culture == null)
				Culture = new CultureInfo("en-US");
			var reportType = GetReportType(reportData);
			var report = (IReportInternal)_container.Resolve(reportType);
			report.SetData(reportData, Culture);
			return (IReport)report;
		}

		private Type GetReportType(object reportData)
		{
			var reportDataTypeName = reportData.GetType().AssemblyQualifiedName;
			reportDataTypeName = string.Join(",", reportDataTypeName.Split(',').Take(2));
			var reportTypeName = ReportsConfiguration.GetReportTypeName(reportDataTypeName);
			return Type.GetType(reportTypeName, throwOnError: true);
		}
	}
}