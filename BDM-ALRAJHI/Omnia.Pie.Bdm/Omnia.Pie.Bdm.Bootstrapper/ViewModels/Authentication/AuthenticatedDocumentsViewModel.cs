namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.Authentication
{
	using System;
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;

	public class AuthenticatedDocumentsViewModel : ExpirableBaseViewModel, IAuthenticatedDocumentsViewModel
	{
		public Action LCRequestAction { get; set; }

		private ICommand _lcRequestCommand;
		public ICommand LCRequestCommand
		{
			get
			{
				if (_lcRequestCommand == null)
					_lcRequestCommand = new DelegateCommand(LCRequestAction);

				return _lcRequestCommand;
			}
		}

		public Action NLCRequestAction { get; set; }

		private ICommand _nlcRequestCommand;
		public ICommand NLCRequestCommand
		{
			get
			{
				if (_nlcRequestCommand == null)
					_nlcRequestCommand = new DelegateCommand(NLCRequestAction);

				return _nlcRequestCommand;
			}
		}

		public Action IBANLetterAction { get; set; }

		private ICommand _ibanLetterCommand;
		public ICommand IBANLetterCommand
		{
			get
			{
				if (_ibanLetterCommand == null)
					_ibanLetterCommand = new DelegateCommand(IBANLetterAction);

				return _ibanLetterCommand;
			}
		}

		public void Dispose()
		{
			
		}
	}
}