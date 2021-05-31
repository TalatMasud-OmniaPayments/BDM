using System;

namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface IReceiptPrinter : IPrinter
	{
        event EventHandler<PrinterStatus> ReceiptPrinterMediaStatusChanged;
        event EventHandler<string> ReceiptPrinterDeviceStatusChanged;
        string GetReceiptPaperStatus();
    }
}