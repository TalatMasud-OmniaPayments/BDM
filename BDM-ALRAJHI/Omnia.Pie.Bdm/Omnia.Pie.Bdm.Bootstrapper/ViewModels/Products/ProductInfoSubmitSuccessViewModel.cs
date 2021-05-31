namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.Products
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class ProductInfoSubmitSuccessViewModel : ExpirableBaseViewModel, IProductInfoSubmitSuccessViewModel
	{
		public Action YesAction { get; set; }
		public Action NoAction { get; set; }

		private ICommand _yesCommand;
		public ICommand YesCommand
		{
			get
			{
				if (_yesCommand == null)
					_yesCommand = new DelegateCommand(
						() =>
						{
							StopTimer();
							YesAction?.Invoke();
						}
				);

				return _yesCommand;
			}
		}

		private ICommand _noCommand;
		public ICommand NoCommand
		{
			get
			{
				if (_noCommand == null)
					_noCommand = new DelegateCommand(
						() =>
						{
							StopTimer();
							NoAction?.Invoke();
						}
				);

				return _noCommand;
			}
		}

		public void Dispose()
		{
			
		}
	}
}