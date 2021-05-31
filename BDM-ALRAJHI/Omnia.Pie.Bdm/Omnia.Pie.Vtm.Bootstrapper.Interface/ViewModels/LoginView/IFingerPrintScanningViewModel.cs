using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface IFingerprintScanningViewModel: IExpirableBaseViewModel
    {

        Finger SelectedFinger { get; set; }
        void Type(AnimationType type);
    }
    
}
