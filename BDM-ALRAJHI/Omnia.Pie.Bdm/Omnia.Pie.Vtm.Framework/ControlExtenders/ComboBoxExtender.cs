namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System.Collections;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;

	public class ComboBoxExtender : Control
	{
		static ComboBoxExtender()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxExtender), new FrameworkPropertyMetadata(typeof(ComboBoxExtender)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		public bool IsDropDownOpen
		{
			get { return (bool)GetValue(IsDropDownOpenProperty); }
			set { SetValue(IsDropDownOpenProperty, value); }
		}

		public static readonly DependencyProperty IsDropDownOpenProperty =
			DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ComboBoxExtender), new PropertyMetadata(false));

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(ComboBoxExtender), new PropertyMetadata(null));

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBoxExtender), new PropertyMetadata(null, OnSelectedItemChanged));

		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var lookup = (ComboBoxExtender)d;
			lookup.IsDropDownOpen = false;
		}

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ComboBoxExtender), new PropertyMetadata(null, OnItemsSourceChanged));

		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var lookup = (ComboBoxExtender)d;
			lookup.IsEmpty = (lookup.ItemsSource?.Cast<object>().Any() != true);
		}

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ComboBoxExtender), new PropertyMetadata(null));

		public DataTemplate SelectedItemTemplate
		{
			get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
			set { SetValue(SelectedItemTemplateProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemTemplateProperty =
			DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(ComboBoxExtender), new PropertyMetadata(null));

		public bool IsEmpty
		{
			get { return (bool)GetValue(IsEmptyProperty); }
			private set { SetValue(IsEmptyPropertyKey, value); }
		}

		private static readonly DependencyPropertyKey IsEmptyPropertyKey =
			DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(ComboBoxExtender), new PropertyMetadata(true));

		public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

		public string EmptyMessage
		{
			get { return (string)GetValue(EmptyMessageProperty); }
			set { SetValue(EmptyMessageProperty, value); }
		}

		public static readonly DependencyProperty EmptyMessageProperty =
			DependencyProperty.Register("EmptyMessage", typeof(string), typeof(ComboBoxExtender), new PropertyMetadata(null));
	}
}