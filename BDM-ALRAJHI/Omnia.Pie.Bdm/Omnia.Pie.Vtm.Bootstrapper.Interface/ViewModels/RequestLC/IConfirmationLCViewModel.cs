namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface IConfirmationLCViewModel : IExpirableBaseViewModel
	{
		Action SendSmsAction { get; set; }
		ICommand SendSmsCommand { get; }

		Action SendEmailAction { get; set; }
		ICommand SendEmailCommand { get; }

		string ReferenceNo { get; set; }
	}
}