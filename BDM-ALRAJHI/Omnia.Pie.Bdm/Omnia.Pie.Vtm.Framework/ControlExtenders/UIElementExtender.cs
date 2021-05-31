namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System.Windows;

	public static class UIElementExtender
	{
		public static bool GetIsDefaultFocusedElement(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsDefaultFocusedElementProperty);
		}

		public static void SetIsDefaultFocusedElement(DependencyObject obj, bool value)
		{
			obj.SetValue(IsDefaultFocusedElementProperty, value);
		}

		public static readonly DependencyProperty IsDefaultFocusedElementProperty =
			DependencyProperty.RegisterAttached("IsDefaultFocusedElement", typeof(bool), typeof(UIElementExtender), new PropertyMetadata(false, OnIsDefaultFocusedElementChanged));

		private static void OnIsDefaultFocusedElementChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			var element = (UIElement)dp;
			element.IsVisibleChanged -= Element_IsVisibleChanged;
			element.IsVisibleChanged += Element_IsVisibleChanged;
		}

		private static void Element_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => ((UIElement)sender).Focus();
	}
}