using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Core.Model
{
    public class Card : BaseViewModel
    {
        private string _CardType;
        public string CardType
        {
            get { return _CardType; }
            set
            {
                if (value != _CardType)
                {
                    _CardType = value;
                    OnPropertyChanged(() => CardType);
                }
            }
        }

        private string _CardNumber;
        public string CardNumber
        {
            get { return _CardNumber; }
            set
            {
                if (value != _CardNumber)
                {
                    _CardNumber = value;
                    OnPropertyChanged(() => CardNumber);
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

        private string _AvailableCardLimit;
        public string AvailableCardLimit
        {
            get { return _AvailableCardLimit; }
            set
            {
                if (value != _AvailableCardLimit)
                {
                    _AvailableCardLimit = value;
                    OnPropertyChanged(() => AvailableCardLimit);
                }
            }
        }

        private string _StatementMinimumDue;
        public string StatementMinimumDue
        {
            get { return _StatementMinimumDue; }
            set
            {
                if (value != _StatementMinimumDue)
                {
                    _StatementMinimumDue = value;
                    OnPropertyChanged(() => StatementMinimumDue);
                }
            }
        }
    }
}
