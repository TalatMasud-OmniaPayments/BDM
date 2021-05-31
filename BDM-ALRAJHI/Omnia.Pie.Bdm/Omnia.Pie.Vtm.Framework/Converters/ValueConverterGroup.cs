namespace Omnia.Pie.Vtm.Framework.Converters
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows;
	using System.Windows.Data;

	public class ValueConverterGroup : List<IValueConverter>, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = value;
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
					parameters[Count - 1] = parts[0];
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

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}