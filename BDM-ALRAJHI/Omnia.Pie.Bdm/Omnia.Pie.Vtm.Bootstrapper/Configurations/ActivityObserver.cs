namespace Omnia.Pie.Vtm.Bootstrapper
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System;
	using System.Windows;
	using System.Windows.Input;

	public class ActivityObserver : IActivityObserver
	{
		public DateTime LastActivityTime { get; private set; }

		public ActivityObserver()
		{
			OnActive();

			EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));
			EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown));
			EventManager.RegisterClassHandler(typeof(Window), Window.MouseMoveEvent, new MouseEventHandler(OnMouseMove));
		}

		private void OnMouseMove(object sender, MouseEventArgs e) => OnActive();

		private void OnActive() => LastActivityTime = DateTime.Now;

		private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e) => OnActive();

		private void OnPreviewKeyDown(object sender, KeyEventArgs e) => OnActive();
	}
}