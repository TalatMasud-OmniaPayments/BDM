namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ChequePrinting
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Collections.Generic;

	public interface IAccountSelectionViewModel : IBaseViewModel
	{
		List<Account> Accounts { get; set; }
		Account SelectedAccount { get; set; }
		int NumberOfCheques { get; set; }
	}

	public class ChequeSelection
	{
		public int Number { get; set; }
		public string NumberOfCheque { get; set; }
		public string Amount { get; set; }
	}
}