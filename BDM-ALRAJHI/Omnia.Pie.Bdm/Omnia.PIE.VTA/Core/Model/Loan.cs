using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Core.Model
{
    public class Loan : BaseViewModel
    {
        private string _LoanType;
        public string LoanType
        {
            get { return _LoanType; }
            set
            {
                if (value != _LoanType)
                {
                    _LoanType = value;
                    OnPropertyChanged(() => LoanType);
                }
            }
        }

        private string _LoanAccountNumber;
        public string LoanAccountNumber
        {
            get { return _LoanAccountNumber; }
            set
            {
                if (value != _LoanAccountNumber)
                {
                    _LoanAccountNumber = value;
                    OnPropertyChanged(() => LoanAccountNumber);
                }
            }
        }

        private string _OutstandingBalance;
        public string OutstandingBalance
        {
            get { return _OutstandingBalance; }
            set
            {
                if (value != _OutstandingBalance)
                {
                    _OutstandingBalance = value;
                    OnPropertyChanged(() => OutstandingBalance);
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
