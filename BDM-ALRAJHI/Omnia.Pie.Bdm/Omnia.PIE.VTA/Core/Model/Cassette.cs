using Omnia.PIE.VTA.ViewModels;

namespace Omnia.PIE.VTA.Core.Model
{
	public class Cassette : BaseViewModel
	{
		private string _CurrenceyNoteName;
		public string CurrenceyNoteName
		{
			get { return _CurrenceyNoteName; }
			set
			{
				if (value != _CurrenceyNoteName)
				{
					_CurrenceyNoteName = value;
					OnPropertyChanged(() => CurrenceyNoteName);
				}
			}
		}

		private string _CurrencyNoteQuantity;
		public string CurrencyNoteQuantity
		{
			get { return _CurrencyNoteQuantity; }
			set
			{
				if (value != _CurrencyNoteQuantity)
				{
					_CurrencyNoteQuantity = value;
					OnPropertyChanged(() => CurrencyNoteQuantity);
				}
			}
		}

		private string _CurrencyNotePercentage;
		public string CurrencyNotePercentage
		{
			get { return _CurrencyNotePercentage; }
			set
			{
				if (value != _CurrencyNotePercentage)
				{
					_CurrencyNotePercentage = value;
					OnPropertyChanged(() => CurrencyNotePercentage);
				}
			}
		}

		private string _CurrenceyAmount;
		public string CurrenceyAmount
		{
			get { return _CurrenceyAmount; }
			set
			{
				if (value != _CurrenceyAmount)
				{
					_CurrenceyAmount = value;
					OnPropertyChanged(() => CurrenceyAmount);
				}
			}
		}
	}
}