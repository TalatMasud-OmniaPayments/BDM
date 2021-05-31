using System;
using System.Windows;
using System.Windows.Input;

namespace Omnia.PIE.VTA.Common
{
	public interface IUserActivityService
	{
		DateTime LastActivityTime { get; }
	}

	internal class UserActivityService : IUserActivityService
	{
		public UserActivityService()
		{
			OnActive();

			EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));
			EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown));
			EventManager.RegisterClassHandler(typeof(Window), Window.MouseMoveEvent, new MouseEventHandler(OnMouseMove));
		}

		private void OnMouseMove(object sender, MouseEventArgs e) => OnActive();

		public DateTime LastActivityTime { get; private set; }

		private void OnActive() => LastActivityTime = DateTime.Now;

		private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e) => OnActive();

		private void OnPreviewKeyDown(object sender, KeyEventArgs e) => OnActive();
	}
}
