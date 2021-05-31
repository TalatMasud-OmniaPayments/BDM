using System;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface ITermsAndConditionsViewModel : IBaseViewModel
	{
		Action AcceptAction { get; set; }
		bool IsChecked { get; set; }
		string CustomerName { get; set; }
	}
}