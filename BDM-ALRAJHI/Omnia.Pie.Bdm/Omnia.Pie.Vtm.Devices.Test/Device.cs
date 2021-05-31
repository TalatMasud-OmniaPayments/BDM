using System.ComponentModel;
using System.Runtime.CompilerServices;
using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Console {
	public abstract class Device : INotifyPropertyChanged {
		public Device() {
			Initialize = new Command(
				() => {
					Model.Initialize();
					initialized = true;
				},
				() => !initialized);
			Dispose = new Command(
				() => {
					Model.Dispose();
					initialized = false;
				},
				() => initialized);
		}

		protected bool initialized;

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		public string Id {
			get {
				return GetType().Name;
			}
		}

		public abstract IDevice Model { get; }
		public Command Initialize { get; }
		public Command Dispose { get; }

	}
}
