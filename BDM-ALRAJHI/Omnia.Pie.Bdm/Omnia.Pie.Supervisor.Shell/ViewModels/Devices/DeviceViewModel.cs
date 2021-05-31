using System.Windows.Input;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Supervisor.Shell.Views;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public abstract class DeviceViewModel : MonitorViewModel {
		public DeviceViewModel() {
			Connect = new DelegateCommand(
				() => {
					if(!Connected)
						Device.Initialize();
					else
						Device.Dispose();
					Load();
				});
		}

		public override string Id => Device?.GetType().Name.ToHumanString();
		public abstract IDevice Device { get; }
		public ICommand Connect { get; }
		public bool Connected => Device.Status == DeviceStatus.Online;
		public override string StatusText { get { return Device.Status.ToString(); } }
		public override MonitorStatus Status => Connected ? MonitorStatus.On : MonitorStatus.Off;

		public override void Load() {
			RaisePropertyChanged(nameof(StatusText));
			RaisePropertyChanged(nameof(Status));
			RaisePropertyChanged(nameof(Connected));
		}

		public IOperationViewModel<DeviceViewModel, object>[] Operations { get; protected set; }
	}
}
