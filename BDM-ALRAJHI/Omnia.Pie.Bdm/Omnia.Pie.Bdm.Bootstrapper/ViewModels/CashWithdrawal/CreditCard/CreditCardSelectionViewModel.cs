using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.CreditCard;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.CashWithdrawal.CreditCard
{
	class CreditCardSelectionViewModel : ExpirableBaseViewModel, ICreditCardSelectionViewModel
	{
		[Required(ErrorMessage = "Please enter amount")]
		[RegularExpression("^[1-9]+[0-9]*00$", ErrorMessage = "Amount should be multiples on 100")]
		public double? Amount { get; set; }

		//public List<Account> Accounts { get; set; }

		public Account SelectedAccount { get; set; }

		private bool _accountSelection;
		public bool AccountSelection
		{
			get { return _accountSelection; }
			set { SetProperty(ref _accountSelection, value); }
		}

		public List<Account> _accounts;
		public List<Account> Accounts
		{
			get { return _accounts; }
			set
			{
				SetProperty(ref _accounts, value);
				if (_accounts != null && _accounts.Count > 1)
				{
					AccountSelection = true;
					RaisePropertyChanged(nameof(AccountSelection));
				}
				else
				{
					AccountSelection = false;
					RaisePropertyChanged(nameof(AccountSelection));
				}
			}
		}

		private ICommand _quickCashCommand;
		public ICommand QuickCashCommand
		{
			get
			{
				if (_quickCashCommand == null)
					_quickCashCommand = new DelegateCommand<double>(AmountSelected);
				return _quickCashCommand;
			}
		}

		private void AmountSelected(double amount)
		{
			Amount = amount;
			OnPropertyChanged(new PropertyChangedEventArgs("Amount"));
		}

		private ICommand _accountSelectedCommand;
		public ICommand AccountSelectedCommand
		{
			get
			{
				if (_accountSelectedCommand == null)
					_accountSelectedCommand = new DelegateCommand(AccountSelected);
				return _accountSelectedCommand;
			}
		}

		private void AccountSelected()
		{
			OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccount"));
		}

		public void Dispose()
		{
		}
	}
}
