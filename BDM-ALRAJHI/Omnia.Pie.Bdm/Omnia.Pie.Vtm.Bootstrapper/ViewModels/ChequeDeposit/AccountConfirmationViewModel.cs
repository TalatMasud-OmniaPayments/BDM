using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	public class AccountConfirmationViewModel : ExpirableBaseViewModel, IAccountConfirmationViewModel
	{
		public Account SelectedAccount { get; set; }

		public void Dispose()
		{

		}
	}
}
