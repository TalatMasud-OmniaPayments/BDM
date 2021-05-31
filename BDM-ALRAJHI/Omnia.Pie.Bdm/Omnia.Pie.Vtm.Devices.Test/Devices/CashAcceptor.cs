using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Console.Devices {
	public class CashAcceptor : Device {
		public override IDevice Model => cashAcceptor;
		readonly ICashAcceptor cashAcceptor = new Vtm.Devices.CashAcceptor.CashAcceptor(new GuideLights.GuideLights());
	}
}
