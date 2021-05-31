using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Omnia.PIE.VTA.Converters
{
	public class CassetteAmountToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int amount = 0;
			int.TryParse(value.ToString(), out amount);
			if (amount > 0)
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Visibility.Visible;
		}
	}
}
