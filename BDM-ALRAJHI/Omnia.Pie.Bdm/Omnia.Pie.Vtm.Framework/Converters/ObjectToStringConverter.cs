namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class ObjectToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = value?.ToString();
			return string.IsNullOrEmpty(result) ? null : result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}