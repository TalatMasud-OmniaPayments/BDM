using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface IVerifyChequeSecurityViewModel : IBaseViewModel
	{
		Cheque[] DepositedCheques { get; set; }
	}
}
