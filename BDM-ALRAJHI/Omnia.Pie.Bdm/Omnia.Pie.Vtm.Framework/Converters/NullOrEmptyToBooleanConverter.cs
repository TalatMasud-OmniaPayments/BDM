namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class NullOrEmptyToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nullOrEmptyMeansTrue = parameter == null || parameter.ToString().ToLower() == "true";
			var valueAsString = value as string;
			var result = nullOrEmptyMeansTrue ? string.IsNullOrEmpty(valueAsString) : !string.IsNullOrEmpty(valueAsString);
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}