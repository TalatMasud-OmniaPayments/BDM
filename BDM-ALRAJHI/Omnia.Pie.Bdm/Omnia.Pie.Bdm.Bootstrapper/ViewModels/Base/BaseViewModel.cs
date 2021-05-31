namespace Omnia.Pie.Bdm.Bootstrapper
{
	using Omnia.Pie.Vtm.Framework.Base;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
    using System.Windows;
    using System.Windows.Input;

	public abstract class BaseViewModel : ValidatableBindableBase
	{
		public Action BackAction { get; set; }
		public Action CancelAction { get; set; }
		public Action DefaultAction { get; set; }

		public bool BackVisibility { get; set; }
		//public bool CancelVisibility { get; set; }
        private bool _cancelVisibility;

        public bool CancelVisibility
        {
            get { return _cancelVisibility; }
            set {
                _cancelVisibility = value;
                RaisePropertyChanged("CancelVisibility");
            }
        }

        public bool DefaultVisibility { get; set; }
		
		private ICommand _cancelCommand;
		public ICommand CancelCommand
		{
			get
			{
				if (_cancelCommand == null)
				{
					_cancelCommand = new DelegateCommand(() => ExecuteCancelCommand(),
						() => CancelAction != null);
				}

				return _cancelCommand;
			}
		}

		private ICommand _backCommand;
		public ICommand BackCommand
		{
			get
			{
				if (_backCommand == null)
				{
					_backCommand = new DelegateCommand(() => ExecuteBackCommand(),
						() => BackAction != null);
				}

				return _backCommand;
			}
		}

		private ICommand _defaultCommand;
		public ICommand DefaultCommand
		{
			get
			{
				if (_defaultCommand == null)
				{
					_defaultCommand = new DelegateCommand
					(
						() => ExecuteDefaultCommand(), () => DefaultAction != null
					);
				}

				return _defaultCommand;
			}
		}

		protected virtual bool ExecuteDefaultCommand()
		{
			var result = false;

			if (Validate())
			{
				result = true;
				DefaultAction();
			}

			return result;
		}

		protected virtual void ExecuteBackCommand()
		{
			BackAction?.Invoke();
		}

		protected virtual void ExecuteCancelCommand()
		{
			CancelAction?.Invoke();
		}
	}
}