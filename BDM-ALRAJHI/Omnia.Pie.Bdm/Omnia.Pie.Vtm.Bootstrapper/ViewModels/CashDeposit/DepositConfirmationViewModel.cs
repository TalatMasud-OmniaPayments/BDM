using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.CashDeposit
{
	public class DepositConfirmationViewModel : ExpirableBaseViewModel, IDepositConfirmationViewModel
	{
		public Account SelectedAccount { get; set; }
		public List<DepositDenomination> DepositedCash { get; set; }
		public int TotalNotes { get; set; }
		public int TotalAmount { get; set; }
		public string Currency { get; set; }

		private ICommand _addMoreCommand;
		public ICommand AddMoreCommand
		{
			get
			{
				if (_addMoreCommand == null)
				{
					_addMoreCommand = new DelegateCommand(AddMoreAction);
				}
				return _addMoreCommand;
			}
		}

		public Action AddMoreAction { get; set; }

		public void Dispose()
		{
		}
	}
}
