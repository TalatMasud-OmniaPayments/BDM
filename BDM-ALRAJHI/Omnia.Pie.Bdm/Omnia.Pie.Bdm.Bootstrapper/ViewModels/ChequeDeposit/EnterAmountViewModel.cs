using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.ComponentModel.DataAnnotations;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.ChequeDeposit
{
	public class EnterAmountViewModel : ExpirableBaseViewModel, IEnterAmountViewModel
	{
		public Account SelectedAccount { get; set; }

		private string _enteredAmount;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public string EnteredAmount
		{
			get
			{
				DecAmount();
				return _enteredAmount;
			}
			set
			{
				DecAmount();
				SetProperty(ref _enteredAmount, value);
			}
		}

		private string _chequeDate;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public string ChequeDate
		{
			get { return _chequeDate; }
			set { SetProperty(ref _chequeDate, value); }
		}

		private void DecAmount()
		{
			if (!string.IsNullOrWhiteSpace(_enteredAmount))
			{
				if (_enteredAmount.Length <= 2)
				{
					_enteredAmount = _enteredAmount.Replace(".", "");
					_enteredAmount = "." + _enteredAmount;
				}
				else
				{
					_enteredAmount = _enteredAmount.Replace(".", "");
					_enteredAmount = _enteredAmount.Insert(_enteredAmount.Length - 2, ".");
				}
			}
		}

		public void Dispose()
		{

		}
	}
}