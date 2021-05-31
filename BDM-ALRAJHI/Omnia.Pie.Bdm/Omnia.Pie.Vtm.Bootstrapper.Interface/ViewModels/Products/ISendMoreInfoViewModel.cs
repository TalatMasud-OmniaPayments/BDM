namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface ISendMoreInfoViewModel : IBaseViewModel
	{
		string ProductType { get; set; }
		string Email { get; set; }
		string Mobile { get; set; }
		Action<string, string> SendMoreInfoAction { get; set; }
		ICommand SendMoreInfoCommand { get; }
	}
}