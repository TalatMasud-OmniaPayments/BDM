namespace Omnia.Pie.Vtm.Framework.Converters
{
	using Omnia.Pie.Vtm.Framework.Extensions;
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class MaskedCardNumberConverter : IValueConverter
	{
		public const char MaskSymbol = '*';
		public const string MaskPattern = "^(.{6})(.+)(.{4})$";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value?.ToString().ToMaskedCardNumber();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}