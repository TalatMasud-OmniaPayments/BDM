using System.Windows.Media.Imaging;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.RequestNLC
{
	public interface ISignaturesViewModel : IExpirableBaseViewModel
	{
		BitmapSource Signature { get; set; }
	}
}
