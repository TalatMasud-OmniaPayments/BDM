namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.RequestIBAN
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.Interface.Entities.Customer;
	using System;
	using System.Windows.Input;

	public class ConfirmationRequestIBANViewModel : ExpirableBaseViewModel, IConfirmationRequestIBANViewModel
	{
		public Account SelectedAccount { get; set ; }
		public AccountDetailResult AcountDetail { get; set; }

		public Action SendSmsAction { get; set; }

		private ICommand _sendSmsCommand;
		public ICommand SendSmsCommand
		{
			get
			{
				if (_sendSmsCommand == null)
					_sendSmsCommand = new DelegateCommand(SendSmsAction);

				return _sendSmsCommand;
			}
		}

		public Action SendEmailAction { get; set; }

		private ICommand _sendEmailCommand;
		public ICommand SendEmailCommand
		{
			get
			{
				if (_sendEmailCommand == null)
					_sendEmailCommand = new DelegateCommand(SendEmailAction);

				return _sendEmailCommand;
			}
		}
		public void Dispose()
		{
			
		}
	}
}