namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface IProductsInfoViewModel : IBaseViewModel
	{
		Action<string> ProductSelectedAction { get; set; }
		ICommand ProductSelectedCommand { get; }
	}
}