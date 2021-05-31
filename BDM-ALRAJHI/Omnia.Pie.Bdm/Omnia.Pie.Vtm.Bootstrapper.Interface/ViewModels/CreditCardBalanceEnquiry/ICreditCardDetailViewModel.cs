using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	public interface ICreditCardDetailViewModel : IBaseViewModel
	{
		CreditCardDetailResult CreditCardDetail { get; set; }
	}
}