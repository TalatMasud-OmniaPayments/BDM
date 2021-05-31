using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.Common
{
	public class ReceiptPrintOptionsViewModel : BaseViewModel, IReceiptPrintOptionsViewModel
	{
		public Action Yes { get; set; }
		public Action No { get; set; }
		private ICommand _receiptPrintCommand;
		public ICommand ReceiptPrintCommand {
			get
			{
				if (_receiptPrintCommand == null)
					_receiptPrintCommand = new DelegateCommand<string>(ReceiptPrintOptionSelected);

				return _receiptPrintCommand;
			}
		}

		private void ReceiptPrintOptionSelected(string type)
		{
			if (type.ToLower() == "yes")
			{
				Yes();
			}
			else
			{
				No();
			}
		}

		public void Dispose()
		{
		}
	}
}
