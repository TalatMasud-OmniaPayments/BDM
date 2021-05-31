using System;
using System.Globalization;
using System.Windows.Data;

namespace Omnia.Pie.Supervisor.UI.Themes.Resources.Converters {
	public class InverseBooleanConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			value is bool && !(bool)value;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			Convert(value, targetType, parameter, culture);
	}
}
