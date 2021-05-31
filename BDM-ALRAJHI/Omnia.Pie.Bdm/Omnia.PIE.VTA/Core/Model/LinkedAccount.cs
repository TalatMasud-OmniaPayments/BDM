using Omnia.PIE.VTA.ViewModels;

namespace Omnia.PIE.VTA.Core.Model
{
	public class LinkedAccount : BaseViewModel
	{
		private string _AccountType;
		public string AccountType
		{
			get { return _AccountType; }
			set
			{
				if (value != _AccountType)
				{
					_AccountType = value;
					OnPropertyChanged(() => AccountType);
				}
			}
		}

		private string _AccountNumber;
		public string AccountNumber
		{
			get { return _AccountNumber; }
			set
			{
				if (value != _AccountNumber)
				{
					_AccountNumber = value;
					OnPropertyChanged(() => AccountNumber);
				}
			}
		}
	}
}
