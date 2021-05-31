using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface ICashDevice : IDevice
	{
		CashCassette GetCashCassettesInfo();
	}
}