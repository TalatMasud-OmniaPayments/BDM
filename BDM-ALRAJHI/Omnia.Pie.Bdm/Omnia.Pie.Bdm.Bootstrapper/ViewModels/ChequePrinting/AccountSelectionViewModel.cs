namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.ChequePrinting
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ChequePrinting;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	public class AccountSelectionViewModel : BaseViewModel, IAccountSelectionViewModel
	{
		public AccountSelectionViewModel()
		{
			decimal ChargesPerLeaf = 25;
			decimal.TryParse(SystemParametersConfiguration.GetElementValue("ChargesPerLeaf"), out ChargesPerLeaf);

			decimal VATPercentage = 5;
			decimal.TryParse(SystemParametersConfiguration.GetElementValue("VATPercentage"), out VATPercentage);

			ChequeSelections = new List<ChequeSelection>()
			{   new ChequeSelection()
				{
					Number = 1,
					NumberOfCheque = $"1 {Properties.Resources.LabelCheque}",
					Amount = $"{(ChargesPerLeaf * 1) + ((1 * ChargesPerLeaf) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new ChequeSelection()
				{
					Number = 5,
					NumberOfCheque = $"5 {Properties.Resources.LabelCheques}",
					Amount = $"{(ChargesPerLeaf * 5) + ((5 * ChargesPerLeaf) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new ChequeSelection()
				{
					Number = 10,
					NumberOfCheque = $"10 {Properties.Resources.LabelCheques}",
					Amount = $"{(ChargesPerLeaf * 10) + ((10 * ChargesPerLeaf) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new ChequeSelection()
				{
					Number = 20,
					NumberOfCheque = $"20 {Properties.Resources.LabelCheques}",
					Amount = $"{(ChargesPerLeaf * 20) + ((20 * ChargesPerLeaf) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				}
			};

			ChequeSelection = ChequeSelections?.FirstOrDefault();
			NumberOfCheques = ChequeSelection.Number;
		}

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

		public List<ChequeSelection> ChequeSelections { get; set; }

		private ChequeSelection _chequeSelection;
		public ChequeSelection ChequeSelection
		{
			get { return _chequeSelection; }
			set
			{
				SetProperty(ref _chequeSelection, value);
				NumberOfCheques = _chequeSelection.Number;
				RaisePropertyChanged(nameof(NumberOfCheques));
			}
		}

		public int NumberOfCheques { get; set; }

		public void Dispose()
		{

		}
	}
}