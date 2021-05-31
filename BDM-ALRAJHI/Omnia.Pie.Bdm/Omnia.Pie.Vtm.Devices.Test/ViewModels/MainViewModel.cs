using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Devices.Interface;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class MainViewModel : ViewModel
	{
		public DeviceViewModel[] Devices { get; } = new[] {
			new CashAcceptorViewModel { Model = UnityContainer.Container.Resolve<ICashAcceptor>()},
			new DeviceViewModel { Model = UnityContainer.Container.Resolve<IGuideLights>()},
			new CardReaderViewModel { Model = UnityContainer.Container.Resolve<ICardReader>()},
			new CashDeviceViewModel { Model = UnityContainer.Container.Resolve<ICashDevice>()},
			new CashDispenserViewModel { Model = UnityContainer.Container.Resolve<ICashDispenser>()},
			//new CoinDispenserViewModel {Model = UnityContainer.Container.Resolve<ICoinDispenser>()},
			//new ChequeAcceptorViewModel { Model = UnityContainer.Container.Resolve<IChequeAcceptor>()},
			//new EmiratesIdScannerViewModel { Model = UnityContainer.Container.Resolve<IEmiratesIdScanner>()},
			new FingerPrintScannerViewModel { Model = UnityContainer.Container.Resolve<IFingerPrintScanner>()},
			//new SignpadScannerViewModel { Model = UnityContainer.Container.Resolve<ISignpadScanner>()},
			new DeviceViewModel { Model = UnityContainer.Container.Resolve<IPinPad>()},
			new PrinterViewModel<IReceiptPrinter> { Model = UnityContainer.Container.Resolve<IReceiptPrinter>()},
			new PrinterViewModel<IJournalPrinter> { Model = UnityContainer.Container.Resolve<IJournalPrinter>()},
			//new PrinterViewModel<IStatementPrinter> { Model = UnityContainer.Container.Resolve<IStatementPrinter>()},
			new DeviceViewModel { Model = UnityContainer.Container.Resolve<IDoors>()},
            new DeviceViewModel { Model = UnityContainer.Container.Resolve<IDeviceSensors>()},
        };

		public ObservableCollection<object> Results { get; } = new ObservableCollection<object>();

		public override void Load()
		{
			foreach (var i in Devices)
				i.Load();

			CheckPrinter(GetPrinterSettings("Lexmark MS510 Series XL"));
			_logger.Info("Called CheckPrinter");
		}

		private PrinterSettings GetPrinterSettings(string printerName)
		{
			PrinterSettings result;

			var printers = PrinterSettings.InstalledPrinters
				.Cast<string>()
				.Select(p => new PrinterSettings { PrinterName = p })
				.Where(s => s.IsValid)
				.ToList();

			if (string.Compare(printerName, "default", ignoreCase: true) == 0)
			{
				result = printers.FirstOrDefault(p => p.IsDefaultPrinter);
			}
			else
			{
				result = printers.FirstOrDefault(p => string.Compare(p.PrinterName, printerName, ignoreCase: true) == 0);
			}

			return result;
		}

		private void CheckPrinter(PrinterSettings printerSettings)
		{
			var printServer = new PrintServer();
			if (printerSettings != null)
			{
				var printQueue = printServer.GetPrintQueue(printerSettings.PrinterName);
				_logger.Info($"{printQueue == null} The printer is not found.");
				_logger.Info($"{printQueue.HasPaperProblem} The printer is having an unspecified paper problem.");
				_logger.Info($"{printQueue.IsInError} The printer or device is in an error condition.");
				_logger.Info($"{printQueue.IsNotAvailable} The printer is not available.");
				_logger.Info($"{printQueue.IsOffline} The printer is offline.");
				_logger.Info($"{printQueue.IsOutOfMemory} The printer is out of memory.");
				_logger.Info($"{printQueue.IsOutOfPaper} The printer needs to be reloaded with paper of the size required for the current job.");
				_logger.Info($"{printQueue.IsPaperJammed} The current sheet of paper is stuck in the printer.");
				_logger.Info($"{printQueue.IsServerUnknown} The printer is in an error state.");
			}
		}
	}
}