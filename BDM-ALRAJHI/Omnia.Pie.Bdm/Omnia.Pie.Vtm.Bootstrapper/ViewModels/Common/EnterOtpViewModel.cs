namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.ComponentModel.DataAnnotations;

	public class EnterOtpViewModel : ExpirableBaseViewModel, IEnterOtpViewModel
	{
		private string otp;

		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		[StringLength(6, MinimumLength = 6, ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationInvalidOtp))]
		public string Otp
		{
			get { return otp; }
			set { SetProperty(ref otp, value); }
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
				otp = null;
				RaisePropertyChanged(nameof(Otp));
			}

			return result;
		}

		public void Dispose()
		{

		}
	}
}