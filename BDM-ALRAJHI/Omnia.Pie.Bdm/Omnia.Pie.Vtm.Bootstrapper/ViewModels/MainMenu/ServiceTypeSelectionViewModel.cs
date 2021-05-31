namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class ServiceTypeSelectionViewModel : BaseViewModel, IServiceTypeSelectionViewModel
	{
		public bool TellerAssistanceVisibility { get; set; } = false;
		public bool IsSelfModeCalling { get; set; } = false;
		public Action<string> TellerAssistedAction { get; set; }

		private ICommand _tellerAssistanceCommand;
		public ICommand TellerAssistanceCommand
		{
			get
			{
				if (_tellerAssistanceCommand == null)
				{
					_tellerAssistanceCommand = new DelegateCommand<string>
					(
						x => TellerAssistedAction?.Invoke(x),
						x => TellerAssistanceVisibility == true
					);
				}

				return _tellerAssistanceCommand;
			}
		}

		public Action BankingServicesAction { get; set; }

		private ICommand _bankingServicesCommand { get; set; }
		public ICommand BankingServicesCommand
		{
			get
			{
				if (_bankingServicesCommand == null)
				{
					_bankingServicesCommand = new DelegateCommand(
						  () =>
						  {
							  BankingServicesAction?.Invoke();
						  });
				}

				return _bankingServicesCommand;
			}
		}

		public Action ProductInfoAction { get; set; }

		private ICommand _productInfoCommand { get; set; }
		public ICommand ProductInfoCommand
		{
			get
			{
				if (_productInfoCommand == null)
				{
					_productInfoCommand = new DelegateCommand(
						  () =>
						  {
							  ProductInfoAction?.Invoke();
						  });
				}

				return _productInfoCommand;
			}
		}

		public Action CardlessDepositAction { get; set; }

		private ICommand _cardlessDepositCommand { get; set; }
		public ICommand CardlessDepositCommand
		{
			get
			{
				if (_cardlessDepositCommand == null)
				{
					_cardlessDepositCommand = new DelegateCommand(
						  () =>
						  {
							  CardlessDepositAction?.Invoke();
						  });
				}

				return _cardlessDepositCommand;
			}
		}
		

		public void Dispose()
		{

		}
	}
}