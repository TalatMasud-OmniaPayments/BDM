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
	public class DeviceStatusToImageConverter : IValueConverter
	{
		/// <summary>
		/// Converts status value to an image path.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string statusValue = value.ToString().ToUpper();

			if (!string.IsNullOrEmpty(statusValue))
			{
				string result = string.Empty;

				switch (statusValue)
				{
					case "IDLE":
						result = "idle.png";
						break;
					case "OFFLINE":
						result = "offline.png";
						break;
					case "PAPERJAM":
						result = "offline.png";
						break;
					default:
						result = "active.png";
						break;
				}

				return new Uri("pack://application:,,,/Omnia.PIE.VTA;component/Images/" + result);
			}

			return string.Empty;
		}

		/// <summary>
		/// No need to implement converting back on a one-way binding
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
