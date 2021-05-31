namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
	using System.Windows.Media.Imaging;

	public interface ISignatureConfirmationViewModel : IBaseViewModel
	{
		BitmapSource Signature { get; set; }
	}
}