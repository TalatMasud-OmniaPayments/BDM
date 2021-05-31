using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashWithdrawal.DebitCard;
using System;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Windows.Input;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.CashWithdrawal.DebitCard
{
	class DenominationSelectionViewModel : ExpirableBaseViewModel, IDenominationSelectionViewModel
	{
		public bool HundredAvailable { get; set; }
		public bool TwoHundredAvailable { get; set; }
		public bool FiveHundredAvailable { get; set; }
		public bool ThousandAvailable { get; set; }

		public int Actual100Count { get; set; }
		public int Actual200Count { get; set; }
		public int Actual500Count { get; set; }
		public int Actual1000Count { get; set; }

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

		public int Denom100Count { get; set; }

		private ICommand _denom100Plus;
		public ICommand Denom100Plus
		{
			get
			{
				if (_denom100Plus == null)
					_denom100Plus = new DelegateCommand(() =>
					{
						if (Denom100Count < 200)
							Denom100Count++;

						SetDenomAmount(Type._100);
					},
					() => Actual100Count > 0 && Denom100Count < Actual100Count);

				return _denom100Plus;
			}
		}

		private ICommand _denom100Minus;
		public ICommand Denom100Minus
		{
			get
			{
				if (_denom100Minus == null)
					_denom100Minus = new DelegateCommand(() =>
					{
						if (Denom100Count - 1 < 0)
							return;

						Denom100Count--;
						SetDenomAmount(Type._100);
					});

				return _denom100Minus;
			}
		}

		public int Denom200Count { get; set; }

		private ICommand _denom200Plus;
		public ICommand Denom200Plus
		{
			get
			{
				if (_denom200Plus == null)
					_denom200Plus = new DelegateCommand(() =>
					{
						if (Denom200Count < 200)
							Denom200Count++;

						SetDenomAmount(Type._200);
					},
					() => Actual200Count > 0 && Denom200Count < Actual200Count);

				return _denom200Plus;
			}
		}

		private ICommand _denom200Minus;
		public ICommand Denom200Minus
		{
			get
			{
				if (_denom200Minus == null)
					_denom200Minus = new DelegateCommand(() =>
					{
						if (Denom200Count - 1 < 0)
							return;

						Denom200Count--;
						SetDenomAmount(Type._200);
					});

				return _denom200Minus;
			}
		}

		public int Denom500Count { get; set; }

		private ICommand _denom500Plus;
		public ICommand Denom500Plus
		{
			get
			{
				if (_denom500Plus == null)
					_denom500Plus = new DelegateCommand(() =>
					{
						if (Denom500Count < 200)
							Denom500Count++;

						SetDenomAmount(Type._500);
					},
					() => Actual500Count > 0 && Denom500Count < Actual500Count);

				return _denom500Plus;
			}
		}

		private ICommand _denom500Minus;
		public ICommand Denom500Minus
		{
			get
			{
				if (_denom500Minus == null)
					_denom500Minus = new DelegateCommand(() =>
					{
						if (Denom500Count - 1 < 0)
							return;

						Denom500Count--;
						SetDenomAmount(Type._500);
					});

				return _denom500Minus;
			}
		}

		public int Denom1000Count { get; set; }

		private ICommand _denom1000Plus;
		public ICommand Denom1000Plus
		{
			get
			{
				if (_denom1000Plus == null)
					_denom1000Plus = new DelegateCommand(() =>
					{
						if (Denom1000Count < 200)
							Denom1000Count++;

						SetDenomAmount(Type._1000);
					},
					() => Actual1000Count > 0 && Denom1000Count < Actual1000Count);

				return _denom1000Plus;
			}
		}

		private ICommand _denom1000Minus;
		public ICommand Denom1000Minus
		{
			get
			{
				if (_denom1000Minus == null)
					_denom1000Minus = new DelegateCommand(() =>
					{
						if (Denom1000Count - 1 < 0)
							return;

						Denom1000Count--;
						SetDenomAmount(Type._1000);
					},
					() => Actual1000Count > 0 /*&& Denom1000Count < Actual1000Count*/);

				return _denom1000Minus;
			}
		}
		
		public int DenomAmount { get; set; }

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

			RaisePropertyChanged(nameof(Denom100Count));
			RaisePropertyChanged(nameof(Denom200Count));
			RaisePropertyChanged(nameof(Denom500Count));
			RaisePropertyChanged(nameof(Denom1000Count));

			RaisePropertyChanged(nameof(Denom100Amount));
			RaisePropertyChanged(nameof(Denom200Amount));
			RaisePropertyChanged(nameof(Denom500Amount));
			RaisePropertyChanged(nameof(Denom1000Amount));

			RaisePropertyChanged(nameof(DenomAmount));
		}

		#endregion

		enum Type
		{
			_100,
			_200,
			_500,
			_1000,
		}

		public void Dispose()
		{

		}
	}
}