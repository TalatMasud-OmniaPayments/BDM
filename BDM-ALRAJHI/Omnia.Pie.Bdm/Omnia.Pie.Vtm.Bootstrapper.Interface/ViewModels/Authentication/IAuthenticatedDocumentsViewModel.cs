namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface IAuthenticatedDocumentsViewModel : IExpirableBaseViewModel
	{
		Action LCRequestAction { get; set; }
		ICommand LCRequestCommand { get; }

		Action NLCRequestAction { get; set; }
		ICommand NLCRequestCommand { get; }

		Action IBANLetterAction { get; set; }
		ICommand IBANLetterCommand { get; }
	}
}