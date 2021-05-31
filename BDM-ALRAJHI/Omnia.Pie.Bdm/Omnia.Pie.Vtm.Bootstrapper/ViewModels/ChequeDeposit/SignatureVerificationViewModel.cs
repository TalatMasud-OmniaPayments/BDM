using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	class SignatureVerificationViewModel : BaseViewModel, ISignatureVerificationViewModel
	{
		public Cheque[] DepositedCheques { get; set; }

		public void Dispose()
		{

		}
	}
}
