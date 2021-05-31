using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	public class ManualAccountEntryViewModel : ExpirableBaseViewModel, IManualAccountEntryViewModel
	{
		[Required(ErrorMessage ="Required")]
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
