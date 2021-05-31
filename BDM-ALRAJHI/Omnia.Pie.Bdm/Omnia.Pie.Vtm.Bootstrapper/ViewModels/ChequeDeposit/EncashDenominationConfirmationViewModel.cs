using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.DebitCard;
using System;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.ComponentModel;
using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.ChequeDeposit;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.ChequeDeposit
{
	class EncashDenominationConfirmationViewModel : BaseViewModel, IEncashDenominationConfirmationViewModel
	{
		public bool HundredAvailable { get; set; }
		public bool TwoHundredAvailable { get; set; }
		public bool FiveHundredAvailable { get; set; }
		public bool ThousandAvailable { get; set; }
		public string Denom100Amount { get; set; }
		public string Denom200Amount { get; set; }
		public string Denom500Amount { get; set; }
		public string Denom1000Amount { get; set; }

		public Action<List<Denomination>> DenominationAction { get; set; }
		private ICommand _DenominationSelected;
		public ICommand DenominationSelected
		{
			get
			{
				if (_DenominationSelected == null)
					_DenominationSelected = new DelegateCommand<Int16>(DenominationCommand);
				return _DenominationSelected;
			}
		}

		private void DenominationCommand(Int16 denomIndex)
		{
			if (denomIndex != 100)
			{
				DenominationAction(Denominations[denomIndex]);
			}
			else
			{
				if (Amount != DenomAmount)
				{
					DenominationAction(null);
					return;
				}

				List<Denomination> customDenom = new List<Denomination>
				{
					new Denomination() { Amount = Denominations[0][0].Amount, CassettePresent = true, Count = Denom100Count },
					new Denomination() { Amount = Denominations[0][1].Amount, CassettePresent = true, Count = Denom200Count },
					new Denomination() { Amount = Denominations[0][2].Amount, CassettePresent = true, Count = Denom500Count },
					new Denomination() { Amount = Denominations[0][3].Amount, CassettePresent = true, Count = Denom1000Count }
				};

				DenominationAction(customDenom);
			}
		}

		public Account SelectedAccount { get; set; }
		public double? Amount { get; set; }
		public List<List<Denomination>> Denominations { get; set; }

		#region Drag & Drop

		private ICommand _denom100;

		public ICommand Denom100
		{
			get
			{
				if (_denom100 == null)
					_denom100 = new DelegateCommand<string>(HundredOp);
				return _denom100;
			}
		}
		public int Denom100Count { get; set; }
		private void HundredOp(string type)
		{
			if (type == "-")
			{
				if (Denom100Count - 1 < 0)
					return;
				Denom100Count--;
			}
			else
			{
				Denom100Count++;
			}
			SetDenomAmount(Type._100);
		}

		private ICommand _denom200;

		public ICommand Denom200
		{
			get
			{
				if (_denom200 == null)
					_denom200 = new DelegateCommand<string>(TwoHundredOp);
				return _denom200;
			}
		}
		public int Denom200Count { get; set; }
		private void TwoHundredOp(string type)
		{
			if (type == "-")
			{
				if (Denom200Count - 1 < 0)
					return;
				Denom200Count--;
			}
			else
			{
				Denom200Count++;
			}

			SetDenomAmount(Type._200);
		}

		private ICommand _denom500;

		public ICommand Denom500
		{
			get
			{
				if (_denom500 == null)
					_denom500 = new DelegateCommand<string>(FiveHundredOp);
				return _denom500;
			}
		}
		public int Denom500Count { get; set; }
		private void FiveHundredOp(string type)
		{
			if (type == "-")
			{
				if (Denom500Count - 1 < 0)
					return;
				Denom500Count--;
			}
			else
			{
				Denom500Count++;
			}

			SetDenomAmount(Type._500);
		}

		private ICommand _denom1000;

		public ICommand Denom1000
		{
			get
			{
				if (_denom1000 == null)
					_denom1000 = new DelegateCommand<string>(ThousandOp);
				return _denom1000;
			}
		}
		public int Denom1000Count { get; set; }
		private void ThousandOp(string type)
		{
			if (type == "-")
			{
				if (Denom1000Count - 1 < 0)
					return;
				Denom1000Count--;
			}
			else
			{
				Denom1000Count++;
			}

			SetDenomAmount(Type._1000);
		}

		public int DenomAmount { get; set; }
		enum Type
		{
			_100,
			_200,
			_500,
			_1000,
		}
		private void SetDenomAmount(Type type)
		{

			DenomAmount = (Denom100Count * Denominations[0][0].Amount) + (Denom200Count * Denominations[0][1].Amount) + (Denom500Count * Denominations[0][2].Amount) + (Denom1000Count * Denominations[0][3].Amount);

			if (DenomAmount > Amount)
			{
				if (type == Type._100)
				{
					Denom100Count--;
				}
				else if (type == Type._200)
				{
					Denom200Count--;
				}
				else if (type == Type._500)
				{
					Denom500Count--;
				}
				else if (type == Type._1000)
				{
					Denom1000Count--;
				}
				DenomAmount = (Denom100Count * Denominations[0][0].Amount) + (Denom200Count * Denominations[0][1].Amount) + (Denom500Count * Denominations[0][2].Amount) + (Denom1000Count * Denominations[0][3].Amount);
			}
			Denom100Amount = (Denom100Count * 100).ToString();
			Denom200Amount = (Denom200Count * 200).ToString();
			Denom500Amount = (Denom500Count * 500).ToString();
			Denom1000Amount = (Denom1000Count * 1000).ToString();

			OnPropertyChanged(new PropertyChangedEventArgs("Denom100Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom200Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom500Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom1000Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom100Amount"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom200Amount"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom500Amount"));
			OnPropertyChanged(new PropertyChangedEventArgs("Denom1000Amount"));
			OnPropertyChanged(new PropertyChangedEventArgs("DenomAmount"));
		}
		#endregion
		public void Dispose()
		{
		}
	}
}
