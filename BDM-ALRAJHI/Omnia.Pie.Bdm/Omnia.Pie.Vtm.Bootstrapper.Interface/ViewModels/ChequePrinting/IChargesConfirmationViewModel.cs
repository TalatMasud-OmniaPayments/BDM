using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ChequePrinting
{
	public interface IChargesConfirmationViewModel : IBaseViewModel
	{
		Account SelectedAccount { get; set; }
		int NumberOfCheques { get; set; }
		double? Charges { get; set; }
	}
}