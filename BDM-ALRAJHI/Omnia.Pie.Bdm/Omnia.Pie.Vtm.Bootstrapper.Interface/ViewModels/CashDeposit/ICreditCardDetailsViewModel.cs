using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit
{
	public interface ICreditCardDetailsViewModel : IExpirableBaseViewModel
	{
		CreditCard CardUsed { get; set; }
	}
}
