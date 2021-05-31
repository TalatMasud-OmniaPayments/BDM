using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    public class FingerprintScanningViewModel : ExpirableBaseViewModel, IFingerprintScanningViewModel
    {

        private bool _fingerSelection;
        public bool FingerSelection
        {
            get { return _fingerSelection; }
            set { SetProperty(ref _fingerSelection, value); }
        }

        public List<Finger> _fingers;
        public List<Finger> Fingers
        {
            get { return _fingers; }
            set
            {
                SetProperty(ref _fingers, value);
                if (_fingers != null && _fingers.Count > 1)
                {
                    FingerSelection = true;
                    RaisePropertyChanged(nameof(FingerSelection));
                }
                else
                {
                    FingerSelection = false;
                    RaisePropertyChanged(nameof(FingerSelection));
                }
            }
        }

        private Finger _selectedFinger;
        //[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
        public Finger SelectedFinger
        {
            get { return _selectedFinger; }
            set { SetProperty(ref _selectedFinger, value); }
        }

        public AnimationType AnimationType { get; set; }
        public string DisplayMessage { get; set; }

        public void Type(AnimationType type)
        {
            AnimationType = type;
            DisplayMessage = Properties.Resources.ResourceManager.GetString($"Message{type}", Properties.Resources.Culture);
        }
        public FingerprintScanningViewModel()
        {
            LoadFingers();
        }

        public void LoadFingers()
        {
            Finger finger1 = new Finger();
            finger1.Name = "Right Thumb";
            finger1.Index = "RIGHT_THUMB";

            Finger finger2 = new Finger();
            finger2.Name = "Right Index Finger";
            finger2.Index = "RIGHT_INDEX";

            Finger finger3 = new Finger();
            finger3.Name = "Right Middle Finger";
            finger3.Index = "RIGHT_MIDDLE";

            Finger finger4 = new Finger();
            finger4.Name = "Right Ring Finger";
            finger4.Index = "RIGHT_RING";

            Finger finger5 = new Finger();
            finger5.Name = "Right Littel Finger";
            finger5.Index = "RIGHT_LITTLE";

            Finger finger6 = new Finger();
            finger6.Name = "Left Thumb";
            finger6.Index = "LEFT_THUMB";

            Finger finger7 = new Finger();
            finger7.Name = "Left Index Finger";
            finger7.Index = "LEFT_INDEX";

            Finger finger8 = new Finger();
            finger8.Name = "Left Middle Finger";
            finger8.Index = "LEFT_MIDDLE";

            Finger finger9 = new Finger();
            finger9.Name = "Left Ring Finger";
            finger9.Index = "LEFT_RING";

            Finger finger10 = new Finger();
            finger10.Name = "Left Little Finger";
            finger10.Index = "LEFT_LITTLE";

            

            Fingers = new List<Finger>(new Finger[] { finger1, finger2, finger3, finger4, finger5, finger6, finger7, finger8, finger9, finger10 });


            SelectedFinger = Fingers[1];

        }
            public void Dispose()
        {
            
        }
    }
}
