namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	using Omnia.Pie.Vtm.Devices.Interface;

	public class DeviceConfiguration
	{
		public enum DeviceType
		{
			CardReader,
			CashAcceptor,
            CashDispenser,
            CoinDispenser,
			ChequeAcceptor,
			PinPad,
			ReceiptPrinter,
            FingerPrintScanner,
			Cassettes,
		}

		public string Text { get; set; }
		public DeviceType Type { get; set; }
		public DeviceStatus Status { get; set; }
	}
}