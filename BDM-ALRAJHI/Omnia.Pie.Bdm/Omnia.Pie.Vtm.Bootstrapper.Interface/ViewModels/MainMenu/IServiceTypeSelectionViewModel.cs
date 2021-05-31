using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IServiceTypeSelectionViewModel : IBaseViewModel
	{
		Action<string> TellerAssistedAction { get; set; }

		bool IsSelfModeCalling { get; set; }
		bool TellerAssistanceVisibility { get; set; }

		Action BankingServicesAction { get; set; }
		ICommand BankingServicesCommand { get; }

		Action ProductInfoAction { get; set; }
		ICommand ProductInfoCommand { get; }

		Action CardlessDepositAction { get; set; }
		ICommand CardlessDepositCommand { get; }
	}
}