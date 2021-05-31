using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.DebitCard
{
	public interface IDenominationSelectionViewModel : IExpirableBaseViewModel
	{
		bool HundredAvailable { get; set; }
		bool TwoHundredAvailable { get; set; }
		bool FiveHundredAvailable { get; set; }
		bool ThousandAvailable { get; set; }
		string Denom100Amount { get; set; }
		string Denom200Amount { get; set; }
		string Denom500Amount { get; set; }
		string Denom1000Amount { get; set; }

		Account SelectedAccount { get; set; }
		double? Amount { get; set; }

		List<List<Denomination>> Denominations { get; set; }

		ICommand DenominationSelected { get; }

		Action<List<Denomination>> DenominationAction { get; set; }

		int Denom100Count { get; set; }
		int Denom200Count { get; set; }
		int Denom500Count { get; set; }
		int Denom1000Count { get; set; }
		int DenomAmount { get; set; }

		int Actual100Count { get; set; }
		int Actual200Count { get; set; }
		int Actual500Count { get; set; }
		int Actual1000Count { get; set; }
	}
}