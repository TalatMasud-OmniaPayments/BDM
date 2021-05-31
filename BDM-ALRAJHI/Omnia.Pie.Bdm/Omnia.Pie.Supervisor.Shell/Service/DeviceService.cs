using Omnia.Pie.Client.Infrastructure.Interface.Logging;
using Omnia.Pie.Vtm.Devices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Supervisor.Shell.Service
{
	public class DeviceService
	{
		private readonly ILogger _logger;
		private readonly IDevice[] _devices;
		private List<IDevice> deactivatedDevices;

		public DeviceService(ILogger logger)
		{
			_logger = logger;
			_devices = new IDevice[]
			{
				ServiceLocator.Instance.Resolve<IGuideLights>(),
				CardReader = ServiceLocator.Instance.Resolve<ICardReader>(),
				CashAcceptor = ServiceLocator.Instance.Resolve<ICashAcceptor>(),
				CashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>(),
				CheckAcceptor = ServiceLocator.Instance.Resolve<IChequeAcceptor>(),
				ServiceLocator.Instance.Resolve<IEmiratesIdScanner>(),
				ServiceLocator.Instance.Resolve<IPinPad>(),
				ServiceLocator.Instance.Resolve<ISignpadScanner>(),
				Printer =  ServiceLocator.Instance.Resolve<IReceiptPrinter>(),
				JournalPrinter =  ServiceLocator.Instance.Resolve<IJournalPrinter>(),
				Doors = ServiceLocator.Instance.Resolve<IDoors>()
			};
		}

		public readonly ICardReader CardReader;
		public readonly IChequeAcceptor CheckAcceptor;
		public readonly ICashDispenser CashDispenser;
		public readonly ICashAcceptor CashAcceptor;
		public readonly IReceiptPrinter Printer;
		public readonly IJournalPrinter JournalPrinter;
		public readonly IDoors Doors;

		public void DeactivateDevices()
		{
			if (deactivatedDevices != null)
			{
				return;
			}

			deactivatedDevices = new List<IDevice>();
			foreach (IDevice device in _devices)
			{
				if (device.Status == DeviceStatus.Online)
				{
					try
					{
						device.Dispose();
						deactivatedDevices.Add(device);
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
					}
				}
			}
		}

		public void ReactivateDevices()
		{
			if (deactivatedDevices != null)
			{
				foreach (IDevice device in deactivatedDevices)
				{
					device.Initialize();
				}
			}
		}
	}
}
