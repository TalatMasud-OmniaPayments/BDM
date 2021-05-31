namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.RequestNLC
{
	using System.Windows.Media.Imaging;
	
	public interface ISignatureConfirmationViewModel : IBaseViewModel
	{
		BitmapSource Signature { get; set; }
	}
}