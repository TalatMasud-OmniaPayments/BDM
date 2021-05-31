namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System;
	using System.Windows.Input;

	public interface IConfirmationRequestIBANViewModel : IExpirableBaseViewModel
	{
		Action SendSmsAction { get; set; }
		ICommand SendSmsCommand { get; }

		Action SendEmailAction { get; set; }
		ICommand SendEmailCommand { get; }

		Account SelectedAccount { get; set; }
		AccountDetailResult AcountDetail { get; set; }
	}
}