namespace Omnia.Pie.Vtm.Devices
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Module;
	using System;

	public class Module : IModule
	{
		private IResolver _container { get; }
		private ILogger _logger;

		public Module(IResolver container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
			_logger = _container.Resolve<ILogger>();
		}

		public void Initialize()
		{
			var devices = new IDevice[]
			{
				_container.Resolve<IGuideLights>(),
				_container.Resolve<IPinPad>(),
				_container.Resolve<ICardReader>(),
				_container.Resolve<ICashAcceptor>(),
				_container.Resolve<ICashDispenser>(),
				//_container.Resolve<ICoinDispenser>(),
				_container.Resolve<IReceiptPrinter>(),
				_container.Resolve<IJournalPrinter>(),
				//_container.Resolve<IStatementPrinter>(),
				//_container.Resolve<IChequeAcceptor>(),
				//_container.Resolve<ISignpadScanner>(),
				//_container.Resolve<IEmiratesIdScanner>(),
				_container.Resolve<IDoors>(),
                _container.Resolve<IDeviceSensors>(),
				_container.Resolve<IAuxiliaries>(),
                _container.Resolve<IFingerPrintScanner>(),
            };

			//var windowsServices = DependencyResolver.Resolve<IWindowsServices>();
			//windowsServices.RMMAgentServiceStop();

			var deviceActivityService = _container.Resolve<DeviceActivityService>();
			foreach (IDevice device in devices)
			{
				device.Initialize();
				device.AddObserver(deviceActivityService);
			}

			_container.Resolve<IPinPad>().StartReading();

			//windowsServices.RMMAgentServiceStart();

			_logger?.Info("Vtm.Devices Initialized");
		}
	}
}