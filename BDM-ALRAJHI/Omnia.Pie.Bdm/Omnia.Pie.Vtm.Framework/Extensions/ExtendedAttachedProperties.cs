using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.Pie.Vtm.Framework.Extensions
{
	public class ExtendedAttachedProperties
	{
		#region Icon Height

		public static string GetIconHeight(DependencyObject obj)
		{
			return (string)obj.GetValue(IconHeightProperty);
		}

		public static void SetIconHeight(DependencyObject obj, string value)
		{
			obj.SetValue(IconHeightProperty, value);
		}

		public static readonly DependencyProperty IconHeightProperty =
			DependencyProperty.RegisterAttached("IconHeight", typeof(string), typeof(ExtendedAttachedProperties), new PropertyMetadata("45", IconHeightChanged));

		private static void IconHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetIconHeight(d, e.NewValue.ToString());
		}

		#endregion

		#region Icon Width

		public static string GetIconWidth(DependencyObject obj)
		{
			return (string)obj.GetValue(IconWidthProperty);
		}

		public static void SetIconWidth(DependencyObject obj, string value)
		{
			obj.SetValue(IconWidthProperty, value);
		}

		public static readonly DependencyProperty IconWidthProperty =
			DependencyProperty.RegisterAttached("IconWidth", typeof(string), typeof(ExtendedAttachedProperties), new PropertyMetadata("35", IconWidthChanged));

		private static void IconWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetIconWidth(d, e.NewValue.ToString());
		}

		#endregion

		#region Icon

		public static string GetIcon(DependencyObject obj)
		{
			return (string)obj.GetValue(IconProperty);
		}

		public static void SetIcon(DependencyObject obj, string value)
		{
			obj.SetValue(IconProperty, value);
		}

		public static readonly DependencyProperty IconProperty =
			DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(ExtendedAttachedProperties), new PropertyMetadata("CreditCard", IconChanged));

		private static void IconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetIcon(d, e.NewValue.ToString());
		}

		#endregion

		#region English Content

		public static string GetEnglishContent(DependencyObject obj)
		{
			return (string)obj.GetValue(EnglishContentProperty);
		}

		public static void SetEnglishContent(DependencyObject obj, string value)
		{
			obj.SetValue(EnglishContentProperty, value);
		}

		public static readonly DependencyProperty EnglishContentProperty =
			DependencyProperty.RegisterAttached("EnglishContent", typeof(string), typeof(ExtendedAttachedProperties), new PropertyMetadata("", EnglishContentChanged));

		private static void EnglishContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetEnglishContent(d, e.NewValue.ToString());
		}

		#endregion
	}
}