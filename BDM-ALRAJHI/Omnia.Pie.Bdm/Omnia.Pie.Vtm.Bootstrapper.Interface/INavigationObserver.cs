namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;

	public interface INavigationObserver
	{
		event EventHandler<EventArgs> Navigating;
		event EventHandler<NavigatedEventArgs<object>> Navigated;
		event EventHandler<EventArgs> NavigationFailed;

		void RequestNavigationTo<T>(Action<T> action = null);
		void RequestNavigation(object vm);

		void Push<T>(Action<T> action = null);
		void Push(object vm);
		void Pop();
		void ClearStack();
	}

	public class NavigatedEventArgs<T> : EventArgs
	{
		public T ViewModel { get; set; }
	}
}