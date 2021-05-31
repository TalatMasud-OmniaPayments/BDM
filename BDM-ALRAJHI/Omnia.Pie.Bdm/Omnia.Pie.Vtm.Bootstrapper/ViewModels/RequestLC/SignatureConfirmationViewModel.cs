using System.Windows.Media.Imaging;
using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.RequestLC
{
	public class SignatureConfirmationViewModel : BaseViewModel, ISignatureConfirmationViewModel
	{
		private BitmapSource _signature;
		public BitmapSource Signature
		{
			get { return _signature; }
			set { SetProperty(ref _signature, value); }
		}

		public void Dispose()
		{

		}
	}
}