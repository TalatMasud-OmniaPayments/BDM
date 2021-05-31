namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	using System.Globalization;

	public interface IReportInternal
	{
		void SetData(object reportData, CultureInfo culture);
	}
}