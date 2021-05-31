using Omnia.PIE.VTA.Core.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Omnia.PIE.VTA.ViewModels
{
	public class WorkFlowViewModel : BaseViewModel, IDataErrorInfo
	{
		private ObservableCollection<Core.Model.LinkedAccount> _LinkedAccounts;
		public ObservableCollection<Core.Model.LinkedAccount> LinkedAccounts
		{
			get
			{
				if (_LinkedAccounts == null)
					_LinkedAccounts = new ObservableCollection<LinkedAccount>();
				return _LinkedAccounts;
			}
			set
			{
				_LinkedAccounts = value;
				OnPropertyChanged(() => LinkedAccounts);
			}
		}

		private Core.Model.LinkedAccount _SelectedAccountNumber;
		public Core.Model.LinkedAccount SelectedAccountNumber
		{
			get
			{
				if (_SelectedAccountNumber == null)
					_SelectedAccountNumber = new Core.Model.LinkedAccount();

				return _SelectedAccountNumber;
			}
			set
			{
				_SelectedAccountNumber = value;
				OnPropertyChanged(() => SelectedAccountNumber);
			}
		}

		private string _EnteredOTPNumber;
		public string EnteredOTPNumber
		{
			get
			{
				return _EnteredOTPNumber;
			}
			set
			{
				if (value != _EnteredOTPNumber)
				{
					_EnteredOTPNumber = value;
					OnPropertyChanged(() => EnteredOTPNumber);
				}
			}
		}

		private int _otpSentCount;
		public int OTPSentCount
		{
			get
			{
				return _otpSentCount;
			}
			set
			{
				if (value != _otpSentCount)
				{
					_otpSentCount = value;
					OnPropertyChanged(() => OTPSentCount);
				}
			}
		}

		private int _otpResendCount;
		public int OTPResendCount
		{
			get
			{
				return _otpResendCount;
			}
			set
			{
				if (value != _otpResendCount)
				{
					_otpResendCount = value;
					OnPropertyChanged(() => OTPResendCount);
				}
			}
		}

		private string _CardNumber;
		public string CardNumber
		{
			get
			{
				return _CardNumber;
			}
			set
			{
				if (value != _CardNumber)
				{
					_CardNumber = value;
					OnPropertyChanged(() => CardNumber);
				}
			}
		}

		private string _Amount;
		public string Amount
		{
			get
			{
				return _Amount;
			}
			set
			{
				if (value != _Amount)
				{
					_Amount = value;
					OnPropertyChanged(() => Amount);
				}
			}
		}

		private string _Charges;
		public string Charges
		{
			get
			{
				return _Charges;
			}
			set
			{
				if (value != _Charges)
				{
					_Charges = value;
					OnPropertyChanged(() => Charges);
				}
			}
		}

		private string _AccountBalance;
		public string AccountBalance
		{
			get
			{
				return _AccountBalance;
			}
			set
			{
				if (value != _AccountBalance)
				{
					_AccountBalance = value;
					OnPropertyChanged(() => AccountBalance);
				}
			}
		}

		private string _AccountNumber;
		public string AccountNumber
		{
			get
			{
				return _AccountNumber;
			}
			set
			{
				if (value != _AccountNumber)
				{
					_AccountNumber = value;
					OnPropertyChanged(() => AccountNumber);
				}
			}
		}

		private string _AccountType;
		public string AccountType
		{
			get
			{
				return _AccountType;
			}
			set
			{
				if (value != _AccountType)
				{
					_AccountType = value;
					OnPropertyChanged(() => AccountType);
				}
			}
		}

		private string _AccountCurrency;
		public string AccountCurrency
		{
			get
			{
				return _AccountCurrency;
			}
			set
			{
				if (value != _AccountCurrency)
				{
					_AccountCurrency = value;
					OnPropertyChanged(() => AccountCurrency);
				}
			}
		}

		private string _TenNote;
		public string TenNote
		{
			get
			{
				return _TenNote;
			}
			set
			{
				if (value != _TenNote)
				{
					_TenNote = value;
					OnPropertyChanged(() => TenNote);
				}
			}
		}

		private string _TwentyNote;
		public string TwentyNote
		{
			get
			{
				return _TwentyNote;
			}
			set
			{
				if (value != _TwentyNote)
				{
					_TwentyNote = value;
					OnPropertyChanged(() => TwentyNote);
				}
			}
		}

		private string _FiftyNote;
		public string FiftyNote
		{
			get
			{
				return _FiftyNote;
			}
			set
			{
				if (value != _FiftyNote)
				{
					_FiftyNote = value;
					OnPropertyChanged(() => FiftyNote);
				}
			}
		}

		private string _OneHundredNote;
		public string OneHundredNote
		{
			get
			{
				return _OneHundredNote;
			}
			set
			{
				if (value != _OneHundredNote)
				{
					_OneHundredNote = value;
					OnPropertyChanged(() => OneHundredNote);
				}
			}
		}

		private string _TwoHundredNote;
		public string TwoHundredNote
		{
			get
			{
				return _TwoHundredNote;
			}
			set
			{
				if (value != _TwoHundredNote)
				{
					_TwoHundredNote = value;
					OnPropertyChanged(() => TwoHundredNote);
				}
			}
		}

		private string _FiveHundredNote;
		public string FiveHundredNote
		{
			get
			{
				return _FiveHundredNote;
			}
			set
			{
				if (value != _FiveHundredNote)
				{
					_FiveHundredNote = value;
					OnPropertyChanged(() => FiveHundredNote);
				}
			}
		}

		private string _OneThousandNote;
		public string OneThousandNote
		{
			get
			{
				return _OneThousandNote;
			}
			set
			{
				if (value != _OneThousandNote)
				{
					_OneThousandNote = value;
					OnPropertyChanged(() => OneThousandNote);
				}
			}
		}

		private string _EIDDetail;
		public string EIDDetail
		{
			get
			{
				return _EIDDetail;
			}
			set
			{
				if (value != _EIDDetail)
				{
					_EIDDetail = value;
					OnPropertyChanged(() => EIDDetail);
				}
			}
		}

		private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				if (value != _Title)
				{
					_Title = value;
					OnPropertyChanged(() => Title);
				}
			}
		}

		private string _Message;
		public string Message
		{
			get { return _Message; }
			set
			{
				if (value != _Message)
				{
					_Message = value;
					OnPropertyChanged(() => Message);
				}
			}
		}

		private string _Date;
		public string Date
		{
			get
			{
				return _Date;
			}
			set
			{
				if (value != _Date)
				{
					_Date = value;
					OnPropertyChanged(() => Date);
				}
			}
		}

		private string _TransactionNumber;
		public string TransactionNumber
		{
			get { return _TransactionNumber; }
			set
			{
				if (value != _TransactionNumber)
				{
					_TransactionNumber = value;
					OnPropertyChanged(() => TransactionNumber);
				}
			}
		}

		private string _AccountHolderName;
		public string AccountHolderName
		{
			get { return _AccountHolderName; }
			set
			{
				if (value != _AccountHolderName)
				{
					_AccountHolderName = value;
					OnPropertyChanged(() => AccountHolderName);
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

		private string _CustomerMobile;
		public string CustomerMobile
		{
			get { return _CustomerMobile; }
			set
			{
				if (value != _CustomerMobile)
				{
					_CustomerMobile = value;
					OnPropertyChanged(() => CustomerMobile);
				}
			}
		}

		private string _FlowType;
		public string FlowType
		{
			get
			{
				return _FlowType;
			}
			set
			{
				if (value != _FlowType)
				{
					_FlowType = value;
					OnPropertyChanged(() => FlowType);
				}
			}
		}

		private string error = string.Empty;
		public string Error
		{
			get { return error; }
		}

		public string this[string columnName]
		{
			get
			{
				error = string.Empty;

				if (columnName == "TransactionNumber" && string.IsNullOrWhiteSpace(TransactionNumber))
				{
					error = "Transaction Number is required!";
				}
				else if (columnName == "AccountHolderName" && string.IsNullOrWhiteSpace(AccountHolderName))
				{
					error = "Account Holder Name is required!";
				}
				else if (columnName == "SelectedAccountNumber" && string.IsNullOrWhiteSpace(SelectedAccountNumber.AccountNumber))
				{
					error = "Selected Account Number is required!";
				}

				return error;
			}
		}
	}
}
