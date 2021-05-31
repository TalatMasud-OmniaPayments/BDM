using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.ViewModels.Devices;
using Omnia.Pie.Supervisor.Shell.ViewModels.Pages;

namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class StatusViewModel : ViewModel
	{
		public DoorsViewModel Doors { get; } = new DoorsViewModel();
		public object Connection { get; } = new object();
	}
}
