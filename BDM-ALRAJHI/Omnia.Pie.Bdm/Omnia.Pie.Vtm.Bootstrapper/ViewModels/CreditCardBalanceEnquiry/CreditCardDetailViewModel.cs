using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.CreditCardBalanceEnquiry
{
	public class CreditCardDetailViewModel : BaseViewModel, ICreditCardDetailViewModel
	{
		public CreditCardDetailResult CreditCardDetail { get ; set ; }

		public void Dispose()
		{
			
		}
	}
}