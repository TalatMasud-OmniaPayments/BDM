using System;
using System.Globalization;
using System.Windows.Data;

namespace Omnia.Pie.Supervisor.UI.Themes.Resources.Converters {
	public class IntToBooleanConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var nullMeansTrue = parameter == null || parameter.ToString().ToLower() == "true";
			var result = value is int && (int)value != 0;
			return nullMeansTrue ? result : !result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
	}
}
