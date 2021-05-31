namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using System;
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;

	public class ProductsInfoViewModel : BaseViewModel, IProductsInfoViewModel
	{
		public Action<string> ProductSelectedAction { get; set; }

		private ICommand _productSelectedCommand;
		public ICommand ProductSelectedCommand
		{
			get
			{
				if (_productSelectedCommand == null)
					_productSelectedCommand = new DelegateCommand<string>(ProductSelectedAction);
				return _productSelectedCommand;
			}
		}

		public void Dispose()
		{

		}
	}
}