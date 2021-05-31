namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.RequestNLC
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class ConfirmationNLCViewModel : ExpirableBaseViewModel, IConfirmationNLCViewModel
	{
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

		public string ReferenceNo { get; set; }

		public void Dispose()
		{

		}
	}
}