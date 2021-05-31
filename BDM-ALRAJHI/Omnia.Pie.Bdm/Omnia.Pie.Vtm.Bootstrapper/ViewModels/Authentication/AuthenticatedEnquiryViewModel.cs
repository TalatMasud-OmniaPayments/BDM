namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class AuthenticatedEnquiryViewModel : ExpirableBaseViewModel, IAuthenticatedEnquiryViewModel
	{
		public Action StatementPrintAction { get; set; }

		private ICommand _statementPrintCommand;
		public ICommand StatementPrintCommand
		{
			get
			{
				if (_statementPrintCommand == null)
					_statementPrintCommand = new DelegateCommand(StatementPrintAction);

				return _statementPrintCommand;
			}
		}

		public Action BalanceEnquiryAction { get; set; }

		private ICommand _balanceEnquiryCommand;
		public ICommand BalanceEnquiryCommand
		{
			get
			{
				if (_balanceEnquiryCommand == null)
					_balanceEnquiryCommand = new DelegateCommand(BalanceEnquiryAction);

				return _balanceEnquiryCommand;
			}
		}

		public void Dispose()
		{

		}
	}
}