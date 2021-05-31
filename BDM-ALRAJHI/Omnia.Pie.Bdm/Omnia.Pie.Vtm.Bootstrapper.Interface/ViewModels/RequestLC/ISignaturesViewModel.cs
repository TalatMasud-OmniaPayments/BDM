using System.Windows.Media.Imaging;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	public interface ISignaturesViewModel : IExpirableBaseViewModel
	{
		BitmapSource Signature { get; set; }
	}
}