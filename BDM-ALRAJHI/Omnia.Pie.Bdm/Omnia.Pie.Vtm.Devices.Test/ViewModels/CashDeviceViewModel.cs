using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class CashDeviceViewModel : DeviceViewModel
	{
		public CashDeviceViewModel() 
		{
			GetCashCassettesInfo = new OperationViewModel<CashCassette>(() => Model.GetCashCassettesInfo())
			{
				Id = nameof(Model.GetCashCassettesInfo)
			};
		}

		new ICashDevice Model => (ICashDevice)base.Model;

		public OperationViewModel GetCashCassettesInfo { get; private set; }

		public override void Load()
		{
			base.Load();
		}
	}
}