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
	public class ValueConverterGroup : List<IValueConverter>, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = value;

			// Split parameters.
			// E.g., "1stConverterParameter;2ndConverterParameter;3rdConverterParameter", 
			// ";2ndConverterParameter", "1stConverterParameter;", "lastConverterParameter".
			var parameters = Enumerable.Repeat<string>(null, Count).ToList();
			if (parameter != null)
			{
				var parts = parameter.ToString().Split(';');
				if (parts.Length > 1)
				{
					var length = Math.Min(Count, parts.Length);
					for (var i = 0; i < length; ++i)
					{
						parameters[i] = parts[i];
					}
				}
				else if (parts.Length == 1)
				{
					parameters[Count - 1] = parts[0]; // Use this parameter for the last converter by default.
				}
			}

			for (var i = 0; i < Count; ++i)
			{
				var converter = this[i];
				if (result != DependencyProperty.UnsetValue)
				{
					result = converter.Convert(result, targetType, parameters[i], culture);
				}
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
	}
}
