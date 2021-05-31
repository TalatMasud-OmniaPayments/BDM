using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Console.Devices {
	public class Printer : Device {
		public override IDevice Model => printer;
		readonly IPrinter printer = new Vtm.Devices.Printer.Printer(new GuideLights.GuideLights());
	}
}
