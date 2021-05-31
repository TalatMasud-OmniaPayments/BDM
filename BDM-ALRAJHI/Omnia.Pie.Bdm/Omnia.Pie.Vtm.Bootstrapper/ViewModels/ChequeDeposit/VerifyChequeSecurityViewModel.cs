using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	class VerifyChequeSecurityViewModel : BaseViewModel, IVerifyChequeSecurityViewModel
	{
		public Cheque[] DepositedCheques { get; set; }

		public void Dispose()
		{

		}
	}
}
