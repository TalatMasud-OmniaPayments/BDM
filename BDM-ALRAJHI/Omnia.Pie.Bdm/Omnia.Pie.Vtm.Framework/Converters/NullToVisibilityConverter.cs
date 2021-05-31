using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Omnia.Pie.Vtm.Framework.Converters
{
	public class NullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isNull = true;
			var val = (List<Denomination>)value;
			if (val == null)
			{

			}
			else
			{
				foreach (var item in val)
				{
					if (!string.IsNullOrEmpty(item.Count.ToString()))
					{
						isNull = false;
					}
				}
			}
			return isNull ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is Visibility && (Visibility)value == Visibility.Visible;
		}
	}
}
