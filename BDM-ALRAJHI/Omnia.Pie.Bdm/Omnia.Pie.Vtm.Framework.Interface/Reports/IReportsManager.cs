namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	using System.Globalization;

	public interface IReportsManager
	{
		CultureInfo Culture { get; set; }
		IReport CreateReport<TReportData>(TReportData reportData);
	}
}