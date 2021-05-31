using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.CashDeposit
{
	public class CashDepositAccountSelectionViewModel : ExpirableBaseViewModel, ICashDepositAccountSelectionViewModel
	{
		//public List<Account> Accounts { get; set; }
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
		public Action ManualAccountEntryAction { get; set; }

		private ICommand _manualAccountEntryCommand;
		public ICommand ManualAccountEntryCommand
		{
			get
			{
				if (_manualAccountEntryCommand == null)
					_manualAccountEntryCommand = new DelegateCommand(EnterManualAccount);

				return _manualAccountEntryCommand;
			}
		}

		private void EnterManualAccount()
		{
			ManualAccountEntryAction();
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
