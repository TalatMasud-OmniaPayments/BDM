namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface IAuthenticatedEnquiryViewModel : IExpirableBaseViewModel
	{
		Action StatementPrintAction { get; set; }
		ICommand StatementPrintCommand { get; }

		Action BalanceEnquiryAction { get; set; }
		ICommand BalanceEnquiryCommand { get; }
	}
}