namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.StatementPrinting
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	public class StatementPrintingViewModel : ExpirableBaseViewModel, IStatementPrintingViewModel
	{
		public StatementPrintingViewModel()
		{
			decimal MonthlyStatementCharge = 25;
			decimal.TryParse(SystemParametersConfiguration.GetElementValue("StatementChargePerMonth"), out MonthlyStatementCharge);

			decimal VATPercentage = 5;
			decimal.TryParse(SystemParametersConfiguration.GetElementValue("VATPercentage"), out VATPercentage);

			StatementPeriods = new List<StatementPeriod>()
			{
				new StatementPeriod()
				{
					Number=1,
					Period = Properties.Resources.LabelLast1Month,
					Charges = $"{(MonthlyStatementCharge * 1) + ((1 * MonthlyStatementCharge) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new StatementPeriod()
				{
					Number=2,
					Period = Properties.Resources.LabelLast2Month,
					Charges = $"{(MonthlyStatementCharge * 2) + ((2 * MonthlyStatementCharge) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new StatementPeriod()
				{
					Number=3,
					Period = Properties.Resources.LabelLast3Month,
					Charges = $"{(MonthlyStatementCharge * 3) + ((3 * MonthlyStatementCharge) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				},
				new StatementPeriod()
				{
					Number=6,
					Period = Properties.Resources.LabelLast6Month,
					Charges = $"{(MonthlyStatementCharge * 6) + ((6 * MonthlyStatementCharge) * VATPercentage / 100)} {Properties.Resources.LabelAed}"
				}
			};

			StatementPeriod = StatementPeriods?.FirstOrDefault();
			NumberofMonths = StatementPeriod.Number;
		}

		public string MonthsNumber { get; set; }
		public double Amount { get; set; }
		public int NumberofMonths { get; set; }
		public List<StatementPeriod> StatementPeriods { get; set; }

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

		private StatementPeriod _statementPeriod;
		public StatementPeriod StatementPeriod
		{
			get { return _statementPeriod; }
			set
			{
				SetProperty(ref _statementPeriod, value);
				NumberofMonths = _statementPeriod.Number;
				StartDate = DateTime.Today.AddMonths(-_statementPeriod.Number);

				RaisePropertyChanged(nameof(StartDate));
				RaisePropertyChanged(nameof(NumberofMonths));
			}
		}

		private Account account;
		public Account Account
		{
			get { return account; }
			set
			{
				SetProperty(ref account, value);
			}
		}

		private Account _selectedAccount;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public Account SelectedAccount
		{
			get { return _selectedAccount; }
			set { SetProperty(ref _selectedAccount, value); }
		}

		private DateTime _startDate;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public DateTime StartDate
		{
			get
			{
				if (_startDate == DateTime.MinValue)
					_startDate = DateTime.Today.AddMonths(-1);

				return _startDate;
			}
			set { _startDate = value; }
		}

		private DateTime _endDate;
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public DateTime EndDate
		{
			get
			{
				if (_endDate == DateTime.MinValue)
					_endDate = DateTime.Today;

				return _endDate;
			}
			set
			{
				_endDate = value;
			}
		}

		protected override bool ExecuteDefaultCommand()
		{
			var result = false;

			if (Validate())
			{
				result = true;
			}

			base.ExecuteDefaultCommand();
			return result;
		}

		public void Dispose()
		{

		}
	}
}