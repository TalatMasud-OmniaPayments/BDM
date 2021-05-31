namespace Omnia.Pie.Vtm.Framework.DelegateCommand
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Input;

	/// <summary>
	///  Credits : https://github.com/SonyWWS/ATF/blob/master/Framework/Atf.Gui.Wpf/DelegateCommand.cs
	///  This class allows delegating the commanding logic to methods passed as parameters,
	///  and enables a View to bind commands to objects that are not part of the element tree.
	/// </summary>
	public class DelegateCommand : ICommand
	{
		private readonly Func<bool> _canExecuteMethod;
		private readonly Action _executeMethod;
		private List<WeakReference> _canExecuteChangedHandlers;
		private bool _isAutomaticRequeryDisabled;

		public DelegateCommand(Action executeMethod) : this(executeMethod, null, false)
		{

		}

		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
		{

		}

		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
		{
			_executeMethod = executeMethod ?? throw new ArgumentNullException("executeMethod");

			_canExecuteMethod = canExecuteMethod;
			_isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
		}

		#region Public Events

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (!_isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested += value;
				}

				CommandManagerHelper.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
			}
			remove
			{
				if (!_isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested -= value;
				}

				CommandManagerHelper.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
			}
		}

		#endregion

		public bool IsAutomaticRequeryDisabled
		{
			get
			{
				return _isAutomaticRequeryDisabled;
			}
			set
			{
				if (_isAutomaticRequeryDisabled != value)
				{
					if (value)
						CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
					else
						CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);

					_isAutomaticRequeryDisabled = value;
				}
			}
		}

		#region Public Methods and Operators

		public bool CanExecute()
		{
			return _canExecuteMethod == null || _canExecuteMethod();
		}

		public void Execute()
		{
			_executeMethod?.Invoke();
		}

		public void RaiseCanExecuteChanged()
		{
			OnCanExecuteChanged();
		}

		#endregion

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		void ICommand.Execute(object parameter)
		{
			Execute();
		}

		protected virtual void OnCanExecuteChanged()
		{
			CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
		}
	}

	/// <summary>
	/// This class allows delegating the commanding logic to methods passed as parameters,
	/// and enables a View to bind commands to objects that are not part of the element tree.
	/// </summary>
	/// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
	public class DelegateCommand<T> : ICommand
	{
		private readonly Func<T, bool> _canExecuteMethod;
		private readonly Action<T> _executeMethod;
		private List<WeakReference> _canExecuteChangedHandlers;
		private bool _isAutomaticRequeryDisabled;

		public DelegateCommand(Action<T> executeMethod) : this(executeMethod, null, false)
		{

		}

		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
		{

		}

		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
		{
			_executeMethod = executeMethod ?? throw new ArgumentNullException("executeMethod");

			_canExecuteMethod = canExecuteMethod;
			_isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (!_isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested += value;
				}

				CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
			}
			remove
			{
				if (!_isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested -= value;
				}

				CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
			}
		}

		#region Public Properties

		public bool IsAutomaticRequeryDisabled
		{
			get
			{
				return _isAutomaticRequeryDisabled;
			}
			set
			{
				if (_isAutomaticRequeryDisabled != value)
				{
					if (value)
					{
						CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
					}
					else
					{
						CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
					}

					_isAutomaticRequeryDisabled = value;
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public bool CanExecute(T parameter)
		{
			return _canExecuteMethod == null || _canExecuteMethod(parameter);
		}

		public void Execute(T parameter)
		{
			if (_executeMethod != null)
			{
				_executeMethod(parameter);
			}
		}

		public void RaiseCanExecuteChanged()
		{
			OnCanExecuteChanged();
		}

		#endregion

		#region Explicit Interface Methods

		bool ICommand.CanExecute(object parameter)
		{
			// if T is of value type and the parameter is not
			// set yet, then return false if CanExecute delegate
			// exists, else return true
			if (parameter == null && typeof(T).IsValueType)
			{
				return (this._canExecuteMethod == null);
			}

			// BUGFIX: parameter is T prevents exception on {DisconnectedItem} (when ICommand receiver is unloaded from UI)
			return parameter is T && this.CanExecute((T)parameter);
		}

		void ICommand.Execute(object parameter)
		{
			this.Execute((T)parameter);
		}

		#endregion

		#region Methods

		protected virtual void OnCanExecuteChanged()
		{
			CommandManagerHelper.CallWeakReferenceHandlers(this._canExecuteChangedHandlers);
		}

		#endregion
	}

	/// <summary>
	/// This class contains methods for the CommandManager that help avoid memory leaks by
	/// using weak references.
	/// </summary>
	internal class CommandManagerHelper
	{
		internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = x =>
		{
			if (x != null)
			{
				x.ForEach(
					y =>
					{
						var __Handler = y.Target as EventHandler;

						if (__Handler != null)
						{
							CommandManager.RequerySuggested += __Handler;
						}
					});
			}
		};

		internal static Action<List<WeakReference>> CallWeakReferenceHandlers = x =>
		{
			if (x != null)
			{
				// Take a snapshot of the handlers before we call out to them since the handlers
				// could cause the array to me modified while we are reading it.

				var __Callers = new EventHandler[x.Count];
				int __Count = 0;

				for (int i = x.Count - 1; i >= 0; i--)
				{
					WeakReference __Reference = x[i];
					var __Handler = __Reference.Target as EventHandler;
					if (__Handler == null)
					{
						// Clean up old handlers that have been collected
						x.RemoveAt(i);
					}
					else
					{
						__Callers[__Count] = __Handler;
						__Count++;
					}
				}

				// Call the handlers that we snapshotted
				for (int i = 0; i < __Count; i++)
				{
					EventHandler __Handler = __Callers[i];
					__Handler(null, EventArgs.Empty);
				}
			}
		};

		internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = x =>
		{
			if (x != null)
			{
				x.ForEach(
					y =>
					{
						var __Handler = y.Target as EventHandler;

						if (__Handler != null)
						{
							CommandManager.RequerySuggested -= __Handler;
						}
					});
			}
		};

		internal static Action<List<WeakReference>, EventHandler> RemoveWeakReferenceHandler = (x, y) =>
		{
			if (x != null)
			{
				for (int i = x.Count - 1; i >= 0; i--)
				{
					WeakReference __Reference = x[i];
					var __ExistingHandler = __Reference.Target as EventHandler;
					if ((__ExistingHandler == null) || (__ExistingHandler == y))
					{
						// Clean up old handlers that have been collected
						// in addition to the handler that is to be removed.
						x.RemoveAt(i);
					}
				}
			}
		};

		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
		{
			AddWeakReferenceHandler(ref handlers, handler, -1);
		}

		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
		{
			if (handlers == null)
			{
				handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
			}

			handlers.Add(new WeakReference(handler));
		}
	}
}
