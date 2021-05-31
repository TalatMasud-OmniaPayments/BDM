using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices {
	public interface IOperationViewModel<out TDeviceViewModel, out TResult>
		where TDeviceViewModel : DeviceViewModel {

		TDeviceViewModel Device { get; }
		TResult Result { get; }
	}

	public class OperationViewModel<TDeviceViewModel, TResult> : BindableBase, IOperationViewModel<TDeviceViewModel, TResult>
		where TDeviceViewModel : DeviceViewModel {

		public OperationViewModel(TDeviceViewModel device) {
			Device = device;
			Command = new DelegateCommand(
				async () => {
					Result = default(TResult);
					try {
						Result = await Execute(device);
					}
					catch(Exception ex){
						Exception = ex;
					}
				},
				() => CanExecute(device));
		}

		public string Id { get; set; }
		public TDeviceViewModel Device { get; private set; }
		public Func<TDeviceViewModel, Task<TResult>> Execute { get; set; }
		public Func<TDeviceViewModel, bool> CanExecute { get; set; }
		public ICommand Command { get; private set; }

		TResult result;
		public TResult Result { get { return result; } set { SetProperty(ref result, value); } }

		Exception exception;
		public Exception Exception { get { return exception; } set { SetProperty(ref exception, value); } }
	}
}
