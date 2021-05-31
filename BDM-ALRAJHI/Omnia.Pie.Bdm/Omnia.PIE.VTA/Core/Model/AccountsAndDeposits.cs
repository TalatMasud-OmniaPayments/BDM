using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Core.Model
{
    public class AccountsAndDeposits : BaseViewModel
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

        private string _AccountBalance;
        public string AccountBalance
        {
            get { return _AccountBalance; }
            set
            {
                if (value != _AccountBalance)
                {
                    _AccountBalance = value;
                    OnPropertyChanged(() => AccountBalance);
                }
            }
        }

        private string _Currency;
        public string Currency
        {
            get { return _Currency; }
            set
            {
                if (value != _Currency)
                {
                    _Currency = value;
                    OnPropertyChanged(() => Currency);
                }
            }
        }
    }
}
