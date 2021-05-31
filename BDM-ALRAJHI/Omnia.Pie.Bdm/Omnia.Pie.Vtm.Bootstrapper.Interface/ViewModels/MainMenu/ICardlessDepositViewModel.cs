namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface ICardlessDepositViewModel : IBaseViewModel
	{
		Action CashDepositAction { get; set; }
		ICommand CashDepositCommand { get; }

		Action ChequeDepositAction { get; set; }
		ICommand ChequeDepositCommand { get; }
	}
}