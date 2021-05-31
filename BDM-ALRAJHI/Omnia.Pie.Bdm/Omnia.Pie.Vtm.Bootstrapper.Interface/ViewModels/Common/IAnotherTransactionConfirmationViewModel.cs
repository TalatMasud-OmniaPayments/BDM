namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common
{
	using System;
	using System.Windows.Input;

	public interface IAnotherTransactionConfirmationViewModel : IExpirableBaseViewModel
	{
		Action YesAction { get; set; }
		ICommand YesCommand { get; }

		ICommand NoCommand { get; }
		Action NoAction { get; set; }
	}
}