namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Windows.Controls;
	using System.Windows.Data;

	public class ProgressToAngleConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var progress = (double)values[0];
			var bar = values[1] as ProgressBar;

			return 359.999 * (progress / (bar.Maximum - bar.Minimum));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}