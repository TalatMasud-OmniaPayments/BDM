using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels;
using System.Windows.Media.Imaging;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.RequestLC
{
	internal class SignaturesViewModel : ExpirableBaseViewModel, ISignaturesViewModel
	{
		private BitmapSource _signature;
		public BitmapSource Signature
		{
			get { return _signature; }
			set { SetProperty(ref _signature, value); }
		}
		protected override bool ExecuteDefaultCommand()
		{
			var result = false;

			if (Validate())
			{
				result = true;
				StopTimer();
				DefaultAction?.Invoke();
			}
			else
			{
				Signature = null;
				RaisePropertyChanged(nameof(Signature));
			}

			return result;
		}
		public void Dispose()
		{
			
		}
	}
}