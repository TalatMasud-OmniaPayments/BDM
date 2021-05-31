namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;

	public class EqualityToVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var trueMeansVisible = parameter == null || parameter.ToString().ToLower() == "true";

			var result = false;

			if (values?.Length > 1)
			{
				var first = values[0];
				var second = values[1];
				result = (first == null && second == null) || ((first != null && second != null) && (first.Equals(second)));
			}

			if (!trueMeansVisible)
			{
				result = !result;
			}

			return result ? Visibility.Visible : Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}