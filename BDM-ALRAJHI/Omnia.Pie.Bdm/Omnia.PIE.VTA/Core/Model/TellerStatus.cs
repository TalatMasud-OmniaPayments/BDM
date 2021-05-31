using Omnia.PIE.VTA.ViewModels;
using System.Windows.Media;

namespace Omnia.PIE.VTA.Core.Model
{
    public class TellerStatus : BaseViewModel
    {
        private Brush _BackGround = Brushes.GreenYellow;
        public Brush BackGround
        {
            get { return _BackGround; }
            set
            {
                if (value != _BackGround)
                {
                    _BackGround = value;
                    OnPropertyChanged(() => BackGround);
                }
            }
        }
    }
}
