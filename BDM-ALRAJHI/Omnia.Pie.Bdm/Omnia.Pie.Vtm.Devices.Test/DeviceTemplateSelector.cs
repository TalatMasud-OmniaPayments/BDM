using System.Windows;
using System.Windows.Controls;

namespace Omnia.Pie.Vtm.Devices.Console {
	public class DeviceTemplateSelector : DataTemplateSelector {
		public DataTemplate CardReader { get; set; }
		public DataTemplate CashAcceptor { get; set; }
		public DataTemplate Printer { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			if(item is Devices.CardReader)
				return CardReader;
			if(item is Devices.CashAcceptor)
				return CashAcceptor;
			if(item is Devices.Printer)
				return Printer;
			return null;
		}
	}
}
