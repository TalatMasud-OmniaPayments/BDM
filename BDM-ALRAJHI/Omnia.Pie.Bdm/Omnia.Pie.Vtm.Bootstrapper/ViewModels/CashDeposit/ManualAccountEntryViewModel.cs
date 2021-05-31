using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.CashDeposit
{
	public class ManualAccountEntryViewModel : ExpirableBaseViewModel, IManualAccountEntryViewModel
	{
		public string AccountNumber { get; set; }

		private ICommand _validateAccountCommand;
		public ICommand ValidateAccountCommand
		{
			get
			{
				if (_validateAccountCommand == null)
					_validateAccountCommand = new DelegateCommand<object>(AccountNumberEntered);
				return _validateAccountCommand;
			}
		}

		private void AccountNumberEntered(object accountNumber)
		{
			AccountNumber = accountNumber.ToString();
			OnPropertyChanged(new PropertyChangedEventArgs("AccountNumber"));
			DefaultAction();
		}

		public void Dispose()
		{

		}
	}
}
