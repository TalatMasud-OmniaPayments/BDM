namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class ToUpperConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((string)value)?.ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}