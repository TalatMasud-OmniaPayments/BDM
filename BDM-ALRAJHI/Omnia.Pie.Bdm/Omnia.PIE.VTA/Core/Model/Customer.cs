using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Core.Model
{
	public class Customer : BaseViewModel
	{
		private string _Name;
		public string Name
		{
			get { return _Name; }
			set
			{
				if (value != _Name)
				{
					_Name = value;
					OnPropertyChanged(() => Name);
				}
			}
		}

		private string _CustomerId;
		public string CustomerId
		{
			get { return _CustomerId; }
			set
			{
				if (value != _CustomerId)
				{
					_CustomerId = value;
					OnPropertyChanged(() => CustomerId);
				}
			}
		}

		private string _Address1;
		public string Address1
		{
			get { return _Address1; }
			set
			{
				if (value != _Address1)
				{
					_Address1 = value;
					OnPropertyChanged(() => Address1);
				}
			}
		}

		private string _Address2;
		public string Address2
		{
			get { return _Address2; }
			set
			{
				if (value != _Address2)
				{
					_Address2 = value;
					OnPropertyChanged(() => Address2);
				}
			}
		}

		private string _Country;
		public string Country
		{
			get { return _Country; }
			set
			{
				if (value != _Country)
				{
					_Country = value;
					OnPropertyChanged(() => Country);
				}
			}
		}

		private string _RegisteredMobile;
		public string RegisteredMobile
		{
			get { return _RegisteredMobile; }
			set
			{
				if (value != _RegisteredMobile)
				{
					_RegisteredMobile = value;
					OnPropertyChanged(() => RegisteredMobile);
				}
			}
		}

		private string _Nationality;
		public string Nationality
		{
			get { return _Nationality; }
			set
			{
				if (value != _Nationality)
				{
					_Nationality = value;
					OnPropertyChanged(() => Nationality);
				}
			}
		}

		private string _Language;
		public string Language
		{
			get { return _Language; }
			set
			{
				if (value != _Language)
				{
					_Language = value;
					OnPropertyChanged(() => Language);
				}
			}
		}

		private string _Salary;
		public string Salary
		{
			get { return _Salary; }
			set
			{
				if (value != _Salary)
				{
					_Salary = value;
					OnPropertyChanged(() => Salary);
				}
			}
		}

		private string _Staff;
		public string Staff
		{
			get { return _Staff; }
			set
			{
				if (value != _Staff)
				{
					_Staff = value;
					OnPropertyChanged(() => Staff);
				}
			}
		}

		private string _PassportNumber;
		public string PassportNumber
		{
			get { return _PassportNumber; }
			set
			{
				if (value != _PassportNumber)
				{
					_PassportNumber = value;
					OnPropertyChanged(() => PassportNumber);
				}
			}
		}

		private string _PassportExpiry;
		public string PassportExpiry
		{
			get { return _PassportExpiry; }
			set
			{
				if (value != _PassportExpiry)
				{
					_PassportExpiry = value;
					OnPropertyChanged(() => PassportExpiry);
				}
			}
		}

		private string _VisaNumber;
		public string VisaNumber
		{
			get { return _VisaNumber; }
			set
			{
				if (value != _VisaNumber)
				{
					_VisaNumber = value;
					OnPropertyChanged(() => VisaNumber);
				}
			}
		}

		private string _VisaExpiry;
		public string VisaExpiry
		{
			get { return _VisaExpiry; }
			set
			{
				if (value != _VisaExpiry)
				{
					_VisaExpiry = value;
					OnPropertyChanged(() => VisaExpiry);
				}
			}
		}

		private string _EmiratesId;
		public string EmiratesId
		{
			get { return _EmiratesId; }
			set
			{
				if (value != _EmiratesId)
				{
					_EmiratesId = value;
					OnPropertyChanged(() => EmiratesId);
				}
			}
		}

		private string _EmiratesIdExpiry;
		public string EmiratesIdExpiry
		{
			get { return _EmiratesIdExpiry; }
			set
			{
				if (value != _EmiratesIdExpiry)
				{
					_EmiratesIdExpiry = value;
					OnPropertyChanged(() => EmiratesIdExpiry);
				}
			}
		}

		private string _EducationLevel;
		public string EducationLevel
		{
			get { return _EducationLevel; }
			set
			{
				if (value != _EducationLevel)
				{
					_EducationLevel = value;
					OnPropertyChanged(() => EducationLevel);
				}
			}
		}

		private string _RegisteredEmailAccount;
		public string RegisteredEmailAccount
		{
			get { return _RegisteredEmailAccount; }
			set
			{
				if (value != _RegisteredEmailAccount)
				{
					_RegisteredEmailAccount = value;
					OnPropertyChanged(() => RegisteredEmailAccount);
				}
			}
		}

		private string _RegisteredEmailCreditCard;
		public string RegisteredEmailCreditCard
		{
			get { return _RegisteredEmailCreditCard; }
			set
			{
				if (value != _RegisteredEmailCreditCard)
				{
					_RegisteredEmailCreditCard = value;
					OnPropertyChanged(() => RegisteredEmailCreditCard);
				}
			}
		}
	}
}
