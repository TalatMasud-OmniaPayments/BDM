using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.CreditCardBalanceEnquiry
{
	public class CreditCardDetailViewModel : BaseViewModel, ICreditCardDetailViewModel
	{
		public CreditCardDetailResult CreditCardDetail { get ; set ; }

		public void Dispose()
		{
			
		}
	}
}