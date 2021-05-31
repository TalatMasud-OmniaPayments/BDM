namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;

	public interface IEnterAmountViewModel : IExpirableBaseViewModel
	{
		Account SelectedAccount { get; set; }
		string EnteredAmount { get; set; }
		string ChequeDate { get; set; }
	}
}