using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Omnia.Pie.Supervisor.UI.Themes.Resources.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public const char Separator = '|';

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var trueMeansVisible = true;
			var hiddenMode = false;

			if (parameter != null)
			{
				var parameters = parameter.ToString().ToLower().Split(Separator).ToArray();
				trueMeansVisible = parameters.Length > 0 && parameters[0] == "true";
				hiddenMode = parameters.Length > 1 && parameters[1] == "hidden";
			}

			var visible = false;
			if (value is bool)
			{
				visible = (bool)value;
			}

			if (!trueMeansVisible)
			{
				visible = !visible;
			}

			return visible ? Visibility.Visible : (hiddenMode ? Visibility.Hidden : Visibility.Collapsed);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
	}
}
