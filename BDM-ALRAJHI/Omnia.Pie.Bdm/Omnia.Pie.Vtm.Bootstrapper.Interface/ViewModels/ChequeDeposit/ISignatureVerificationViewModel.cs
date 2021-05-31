using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	public interface ISignatureVerificationViewModel : IBaseViewModel
	{
		Cheque[] DepositedCheques { get; set; }
	}
}
