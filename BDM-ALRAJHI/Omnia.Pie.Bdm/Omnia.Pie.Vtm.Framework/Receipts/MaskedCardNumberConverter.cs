namespace Omnia.Pie.Vtm.Framework.Interface.Converters
{
	using Omnia.Pie.Vtm.Framework.Extensions;

	public class MaskedCardNumberConverter : IValueConverter
	{	
		public object Convert(object value, object parameter) => value?.ToString().ToMaskedCardNumber();
	}


	public interface IValueConverter
	{
		object Convert(object value, object parameter);
	}
}