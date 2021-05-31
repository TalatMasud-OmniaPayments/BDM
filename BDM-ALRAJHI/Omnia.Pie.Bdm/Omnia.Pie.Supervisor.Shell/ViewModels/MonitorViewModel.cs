using Omnia.Pie.Supervisor.Shell.Views;

namespace Omnia.Pie.Supervisor.Shell.ViewModels {
	public abstract class MonitorViewModel : ViewModel {
		public abstract string Id { get; }
		public abstract MonitorStatus Status { get; }
		public abstract string StatusText { get; }
	}
}
