using Omnia.PIE.VTA.ViewModels;
using System;

namespace Omnia.PIE.VTA.Core.Model
{
    public class CallDuration : BaseViewModel
    {
        private TimeSpan _Duration;
        public TimeSpan Duration
        {
            get
            {
                if (_Duration == null)
                {
                    _Duration = new TimeSpan(0);
                }

                return _Duration;
            }
            set
            {
                if (value != null)
                {
                    _Duration = value;
                    OnPropertyChanged(() => Duration);
                }
            }
        }
    }
}
