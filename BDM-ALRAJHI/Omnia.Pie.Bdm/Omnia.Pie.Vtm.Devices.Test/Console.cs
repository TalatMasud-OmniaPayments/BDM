namespace Omnia.Pie.Vtm.Devices.Console {
	public class Console {
		//class TL : TraceListener {
		//	public override void Write(string message) {
		//	}

		//	public override void WriteLine(string message) {
		//	}
		//}
		public Console() {
			//var config = new LoggingConfiguration();

			//config.AddLogSource("TL", SourceLevels.All, true)
			//  .AddTraceListener(new TL());
		}
		public Device[] Devices { get; } = new Device[] {
			new Devices.CardReader(),
			new Devices.CashAcceptor(),
			new Devices.Printer()
		};
	}
}
