namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System.Windows;

	public static class ListBoxExtender
	{
		public static DataTemplate GetHeaderTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(HeaderTemplateProperty);
		}

		public static void SetHeaderTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(HeaderTemplateProperty, value);
		}

		public static readonly DependencyProperty HeaderTemplateProperty =
			DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(ListBoxExtender), new PropertyMetadata(null));

		public static string GetEmptyMessage(DependencyObject obj)
		{
			return (string)obj.GetValue(EmptyMessageProperty);
		}

		public static void SetEmptyMessage(DependencyObject obj, string value)
		{
			obj.SetValue(EmptyMessageProperty, value);
		}

		public static readonly DependencyProperty EmptyMessageProperty =
			DependencyProperty.RegisterAttached("EmptyMessage", typeof(string), typeof(ListBoxExtender), new PropertyMetadata(null));

		public static DataTemplate GetFooterTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(FooterTemplateProperty);
		}

		public static void SetFooterTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(FooterTemplateProperty, value);
		}

		public static readonly DependencyProperty FooterTemplateProperty =
			DependencyProperty.RegisterAttached("FooterTemplate", typeof(DataTemplate), typeof(ListBoxExtender), new PropertyMetadata(null));
	}
}
