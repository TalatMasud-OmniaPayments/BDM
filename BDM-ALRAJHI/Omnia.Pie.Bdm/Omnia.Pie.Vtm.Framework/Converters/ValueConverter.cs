namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Globalization;

	public class ValueConverter : System.Windows.Data.IMultiValueConverter
	{

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return values.Clone();
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}