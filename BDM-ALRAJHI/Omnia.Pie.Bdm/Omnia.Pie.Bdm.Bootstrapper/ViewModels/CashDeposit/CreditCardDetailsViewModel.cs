using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.CashDeposit
{
	class CreditCardDetailsViewModel : ExpirableBaseViewModel, ICreditCardDetailsViewModel
	{
		public CreditCard CardUsed { get; set; }

		public void Dispose()
		{

		}
	}
}
