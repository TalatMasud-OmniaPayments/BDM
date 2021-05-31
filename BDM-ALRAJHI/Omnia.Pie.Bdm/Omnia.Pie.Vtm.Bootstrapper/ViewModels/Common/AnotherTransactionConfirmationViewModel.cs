namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels.Common
{
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;

	public class AnotherTransactionConfirmationViewModel : ExpirableBaseViewModel, IAnotherTransactionConfirmationViewModel
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