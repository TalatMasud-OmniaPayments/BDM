using System.Windows.Media.Imaging;

namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.RequestNLC
{
	public class SignaturesViewModel : ExpirableBaseViewModel, Interface.ViewModels.RequestNLC.ISignaturesViewModel
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