using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.DebitCard;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.ComponentModel.DataAnnotations;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.CashWithdrawal.DebitCard
{
	public class AccountSelectionViewModel : ExpirableBaseViewModel, IAccountSelectionViewModel
	{
		private string _amountString;
		[Required(ErrorMessage = "Please enter amount")]
		[RegularExpression("^[1-9]+[0-9]*00$", ErrorMessage = "Amount should be multiples of 100")]
		public string AmountString
		{
			get { return _amountString; }
			set
			{
				SetProperty(ref _amountString, value);
				double.TryParse(_amountString, out var amount);
				Amount = amount;
			}
		}

		[Required(ErrorMessage = "Please enter amount")]
		[RegularExpression("^[1-9]+[0-9]*00$", ErrorMessage = "Amount should be multiples of 100")]
		public double? Amount { get; set; }

		private Account _selectedAccount;

		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public Account SelectedAccount
		{
			get { return _selectedAccount; }
			set { SetProperty(ref _selectedAccount, value); }
		}

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
			AmountString = amount.ToString();
			RaisePropertyChanged(nameof(Amount));
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
			RaisePropertyChanged(nameof(SelectedAccount));
		}

		public void Dispose()
		{
		}
	}
}
