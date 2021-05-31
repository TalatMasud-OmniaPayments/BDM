using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IActiveConfirmationViewModel : IExpirableBaseViewModel
	{
		void Type(QuestionType questionType);

		Action YesAction { get; set; }
		Action NoAction { get; set; }
		ICommand YesCommand { get; }
		ICommand NoCommand { get; }
	}

	public enum QuestionType
	{
		Retard,
		PerformTransaction,
		ProceedWithoutReceipt
	}
}