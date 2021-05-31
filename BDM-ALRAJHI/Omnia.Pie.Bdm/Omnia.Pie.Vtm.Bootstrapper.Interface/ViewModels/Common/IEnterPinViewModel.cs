using System;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface IEnterPinViewModel : IExpirableBaseViewModel
	{
		string PinBlock { get; }
		Action FourDigitLength { get; set; }
	}
}