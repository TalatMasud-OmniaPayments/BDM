namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class CardlessDepositViewModel : BaseViewModel, ICardlessDepositViewModel
	{
		public Action CashDepositAction { get; set; }

		private ICommand _cashDepositCommand { get; set; }
		public ICommand CashDepositCommand
		{
			get
			{
				if (_cashDepositCommand == null)
				{
					_cashDepositCommand = new DelegateCommand(
						  () =>
						  {
							  CashDepositAction?.Invoke();
						  });
				}

				return _cashDepositCommand;
			}
		}

		public Action ChequeDepositAction { get; set; }

		private ICommand _chequeDepositCommand { get; set; }
		public ICommand ChequeDepositCommand
		{
			get
			{
				if (_chequeDepositCommand == null)
				{
					_chequeDepositCommand = new DelegateCommand(
						  () =>
						  {
							  ChequeDepositAction?.Invoke();
						  });
				}

				return _chequeDepositCommand;
			}
		}

		public void Dispose()
		{

		}
	}
}