namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;

	public interface IConfirmationStatementPrintingViewModel : IExpirableBaseViewModel
	{
		bool MonthType { get; set; }
		string Amount { get; set; }
		Account SelectedAccount { get; set; }
		string NumberofMonths { get; set; }
	}
}