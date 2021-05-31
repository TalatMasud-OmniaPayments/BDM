using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common
{
	public interface IDeviceNotAvailableViewModel : IBaseViewModel
	{
		Action YesAction { get; set; }
		Action NoAction { get; set; }
		ICommand YesCommand { get; }
		ICommand NoCommand { get; }
	}
}
