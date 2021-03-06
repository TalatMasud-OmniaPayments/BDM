using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Omnia.Pie.Supervisor.UI.Themes.Resources.Converters
{
	public class NullOrEmptyToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nullOrEmptyMeansTrue = parameter == null || parameter.ToString().ToLower() == "true";
			var valueAsString = value as string;
			var result = nullOrEmptyMeansTrue ? string.IsNullOrEmpty(valueAsString) : !string.IsNullOrEmpty(valueAsString);
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
	}
}
