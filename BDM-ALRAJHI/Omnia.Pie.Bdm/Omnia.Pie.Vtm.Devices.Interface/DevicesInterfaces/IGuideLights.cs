namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface IGuideLights : IDevice
	{
		IGuideLight CashDispenser { get; }
		IGuideLight CoinDispenser { get; }
		IGuideLight CardReader { get; }
		IGuideLight EmiratesIdScanner { get; }
		IGuideLight PinPad { get; }
		IGuideLight ReceiptPrinter { get; }
		IGuideLight Scanner { get; }
		IGuideLight DocumentPrinter { get; }
		IGuideLight ChequeAcceptor { get; }
        //IGuideLight FingerPrintScanner { get; }
    }
}