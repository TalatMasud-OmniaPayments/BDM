using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class DeviceViewModel : ViewModel
	{
		public DeviceViewModel()
		{
			Open = new OperationViewModel(
				() => Model.Initialize(),
				() => !IsOpen)
			{
				Id = nameof(Model.Initialize)
			};
			Close = new OperationViewModel(
				() => Model.Dispose(),
				() => IsOpen)
			{
				Id = nameof(Model.Dispose)
			};
			Reset = new OperationViewModel(
				() => Model.ResetAsync())
			{
				Id = nameof(Model.ResetAsync)
			};
			Test = new OperationViewModel(
				() => Model.TestAsync())
			{
				Id = nameof(Model.TestAsync)
			};
			Load();
		}

		public override string Id => Model.GetType().Name;
		public IDevice Model { get; set; }
		public bool IsOpen => Model.Status == DeviceStatus.Online;
		public DeviceStatus Status => Model.Status;

		public OperationViewModel Open { get; private set; }
		public OperationViewModel Close { get; private set; }
		public OperationViewModel Reset { get; private set; }
		public OperationViewModel Test { get; private set; }

		public override void Load()
		{
			RaisePropertyChanged(nameof(Status));
			RaisePropertyChanged(nameof(IsOpen));
		}
	}
}