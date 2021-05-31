using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Core.Model
{
    public class AccountHolder : BaseViewModel
    {
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

        private string _AccountHolderImage;
        public string AccountHolderImage
        {
            get { return _AccountHolderImage; }
            set
            {
                if (value != _AccountHolderImage)
                {
                    _AccountHolderImage = value;
                    OnPropertyChanged(() => AccountHolderImage);
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

        private string _IBANNumber;
        public string IBANNumber
        {
            get { return _IBANNumber; }
            set
            {
                if (value != _IBANNumber)
                {
                    _IBANNumber = value;
                    OnPropertyChanged(() => IBANNumber);
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

        private DateTime _PassportExpiryDate;
        public DateTime PassportExpiryDate
        {
            get { return _PassportExpiryDate; }
            set
            {
                if (value != _PassportExpiryDate)
                {
                    _PassportExpiryDate = value;
                    OnPropertyChanged(() => PassportExpiryDate);
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

        private DateTime _VisaExpiryDate;
        public DateTime VisaExpiryDate
        {
            get { return _VisaExpiryDate; }
            set
            {
                if (value != _VisaExpiryDate)
                {
                    _VisaExpiryDate = value;
                    OnPropertyChanged(() => VisaExpiryDate);
                }
            }
        }

        private string _EmiratesIdNumber;
        public string EmiratesIdNumber
        {
            get { return _EmiratesIdNumber; }
            set
            {
                if (value != _EmiratesIdNumber)
                {
                    _EmiratesIdNumber = value;
                    OnPropertyChanged(() => EmiratesIdNumber);
                }
            }
        }

        private DateTime _EmiratesIdExpiryDate;
        public DateTime EmiratesIdExpiryDate
        {
            get { return _EmiratesIdExpiryDate; }
            set
            {
                if (value != _EmiratesIdExpiryDate)
                {
                    _EmiratesIdExpiryDate = value;
                    OnPropertyChanged(() => EmiratesIdExpiryDate);
                }
            }
        }
    }
}
