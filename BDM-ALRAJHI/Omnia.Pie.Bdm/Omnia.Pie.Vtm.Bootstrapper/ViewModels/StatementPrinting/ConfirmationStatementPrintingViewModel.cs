using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.StatementPrinting
{
	public class ConfirmationStatementPrintingViewModel : ExpirableBaseViewModel, IConfirmationStatementPrintingViewModel
	{
		public string Amount { get; set; }
		public Account SelectedAccount { get; set; }
		public bool MonthType { get; set; }
		public string NumberofMonths { get ; set; }

		public void Dispose()
		{

		}
	}
}