namespace Omnia.Pie.Vtm.Bootstrapper.Configurations
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Linq;
	using System;
	using System.Windows.Controls;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System.Collections.Generic;

	public class NavigationObserver : INavigationObserver
	{
		private readonly IResolver _container;
		private readonly ILogger _logger;

		public event EventHandler<EventArgs> Navigating;
		public event EventHandler<NavigatedEventArgs<object>> Navigated;
		public event EventHandler<EventArgs> NavigationFailed;

		private MainWindow MainWindow { get; }

		public NavigationObserver(IResolver container, MainWindow mainWindow)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
			MainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
			_logger = container.Resolve<ILogger>();
		}

		#region Simple Navigation

		public void RequestNavigationTo<T>(Action<T> action = null)
		{
			var viewModel = _container.Resolve<T>();
			action?.Invoke(viewModel);
			RequestNavigationTo(viewModel);
		}

		public void RequestNavigation(object vm)
		{
			var viewType = GetViewType(vm);
			var view = GetViewControl(viewType);

			view.DataContext = vm;
			MainWindow.PART_Content.Content = view;
		}

		private void RequestNavigationTo(object viewModel)
		{
			if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
			Navigate(viewModel);
		}

		#endregion

		#region Push and Pop

		private Stack<object> NavigationStack { get; } = new Stack<object>();

		public void Push<TViewModel>(Action<TViewModel> configure)
		{
			var viewModel = _container.Resolve<TViewModel>();
			configure?.Invoke(viewModel);
			Push(viewModel);
		}

		public void Push(object viewModel)
		{
			if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

			var previousView = (UserControl)MainWindow.PART_Content.Content;
			var previousViewModel = previousView?.DataContext;

			if (previousViewModel != null)
			{
				NavigationStack.Push(previousViewModel);
			}

			Navigate(viewModel);
		}

		public void Pop()
		{
			if (NavigationStack.Any())
			{
				var viewModel = NavigationStack.Pop();
				Navigate(viewModel);
			}
		}

		#endregion

		private void Navigate(object viewModel)
		{
			Navigating?.Invoke(this, EventArgs.Empty);

			try
			{
				var viewType = GetViewType(viewModel);
				var view = GetViewControl(viewType);
				view.DataContext = viewModel;
				MainWindow.PART_Content.Content = view;

				Navigated?.Invoke(this, new NavigatedEventArgs<object>() { ViewModel = viewModel });
			}
			catch (Exception)
			{
				NavigationFailed?.Invoke(this, EventArgs.Empty);
				throw;
			}
		}

		private Type GetViewType(object viewModel)
		{
			var viewModelTypeName = viewModel.GetType().AssemblyQualifiedName;
			viewModelTypeName = string.Join(",", viewModelTypeName.Split(',').Take(2));
			var viewTypeName = BootstrapperConfiguration.GetViewTypeName(viewModelTypeName);

			return Type.GetType(viewTypeName, throwOnError: true);
		}

		private UserControl GetViewControl(Type viewType)
		{
			UserControl result;

			var previousView = MainWindow.PART_Content.Content;
			if (previousView?.GetType() == viewType)
			{
				result = (UserControl)previousView;
			}
			else
			{
				result = (UserControl)_container.Resolve(viewType);
			}

			return result;
		}

		public void ClearStack()
		{
			NavigationStack.Clear();
		}
	}
}