using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Omnia.Pie.Supervisor.UI.Themes.Behaviors
{
	public  class StealFocusBehavior : Behavior<UIElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Focusable = true;
			AssociatedObject.MouseDown += AssociatedObject_MouseDown;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
			AssociatedObject.Focusable = false;

			base.OnDetaching();
		}

		private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e) => AssociatedObject.Focus();
	}
}
