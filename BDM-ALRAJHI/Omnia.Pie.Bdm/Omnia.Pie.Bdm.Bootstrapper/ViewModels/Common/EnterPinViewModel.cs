namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using System.ComponentModel.DataAnnotations;
	using System;

	public class EnterPinViewModel : ExpirableBaseViewModel, IEnterPinViewModel
	{
		public Action FourDigitLength { get; set; }

		private string pin;

		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		[MinLength(4, ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationInvalidPin))]
		[MaxLength(12, ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationInvalidPin))]
		public string Pin
		{
			get { return pin; }
			set {
				SetProperty(ref pin, value);
				if(pin?.Length == 4)
				{
					FourDigitLength?.Invoke();
				}
			}
		}

		//public string PinBlock => Pin.PadRight(16, 'F').ToTripleDESHexString("B2D724CC1EAAB8BE48667484FC18D26C"); // Old block
		public string PinBlock => Pin.PadRight(16, 'F').ToTripleDESHexString("1F682351D0F1126D23DF6165B3E03B7F"); // New Block

		public void Dispose()
		{

		}
	}
}