using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common
{
	public interface IReceiptPrintOptionsViewModel : IBaseViewModel
	{
		Action Yes { get; set; }
		Action No { get; set; }
		ICommand ReceiptPrintCommand { get; }
	}
}
