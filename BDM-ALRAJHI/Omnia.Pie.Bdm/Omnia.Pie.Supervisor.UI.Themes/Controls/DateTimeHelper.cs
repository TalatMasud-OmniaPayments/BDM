using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.UI.Themes.Controls.DateTimeHelper
{
	public enum TextBoxDataType
	{
		None,
		Numeric,
		Alphanumeric,
		AlphanumericWithoutSpace,
		AlphabeticOnly, // Alphabetic characters only.
		Date
	}

	public static class TextBoxHelper
	{
		#region ClearWhenBackspace

		public static bool GetClearWhenBackspace(DependencyObject obj)
		{
			return (bool)obj.GetValue(ClearWhenBackspaceProperty);
		}

		public static void SetClearWhenBackspace(DependencyObject obj, bool value)
		{
			obj.SetValue(ClearWhenBackspaceProperty, value);
		}

		public static readonly DependencyProperty ClearWhenBackspaceProperty =
			DependencyProperty.RegisterAttached("ClearWhenBackspace", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnClearWhenBackspaceChanged));

		private static void OnClearWhenBackspaceChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.PreviewKeyDown -= TextBox_PreviewKeyDown_Back;
				if ((bool)e.NewValue)
				{
					textBox.PreviewKeyDown += TextBox_PreviewKeyDown_Back;
				}
			}
			catch (Exception)
			{
			}
		}
		private static void TextBox_PreviewKeyDown_Back(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back)
			{
				var textBox = (TextBox)sender;
				textBox.Text = null;
				e.Handled = true;
			}
		}

		#endregion ClearWhenBackspace

		#region IgnoreFirstSpaces

		public static bool GetIgnoreFirstSpaces(DependencyObject obj)
		{
			return (bool)obj.GetValue(IgnoreFirstSpacesProperty);
		}

		public static void SetIgnoreFirstSpaces(DependencyObject obj, bool value)
		{
			obj.SetValue(IgnoreFirstSpacesProperty, value);
		}

		public static readonly DependencyProperty IgnoreFirstSpacesProperty =
			DependencyProperty.RegisterAttached("IgnoreFirstSpaces", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnIgnoreFirstSpacesChanged));

		private static void OnIgnoreFirstSpacesChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (TextBox)dp;
			textBox.TextChanged -= TextBox_TextChanged;
			if ((bool)e.NewValue)
			{
				textBox.TextChanged += TextBox_TextChanged;
			}
		}

		private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = (TextBox)sender;
			if (textBox.Text?.Length > 0 && string.IsNullOrWhiteSpace(textBox.Text))
			{
				textBox.Text = null;
				e.Handled = true;
			}
		}

		#endregion IgnoreFirstSpaces

		#region ClearFocusWhenEnter

		public static bool GetClearFocusWhenEnter(DependencyObject obj)
		{
			return (bool)obj.GetValue(ClearFocusWhenEnterProperty);
		}

		public static void SetClearFocusWhenEnter(DependencyObject obj, bool value)
		{
			obj.SetValue(ClearFocusWhenEnterProperty, value);
		}

		public static readonly DependencyProperty ClearFocusWhenEnterProperty =
			DependencyProperty.RegisterAttached("ClearFocusWhenEnter", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnClearFocusWhenEnterChanged));

		private static void OnClearFocusWhenEnterChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.PreviewKeyDown -= TextBox_PreviewKeyDown_Return;
				if ((bool)e.NewValue)
				{
					textBox.PreviewKeyDown += TextBox_PreviewKeyDown_Return;
				}
			}
			catch (Exception)
			{
			}
		}

		private static void TextBox_PreviewKeyDown_Return(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				var textBox = (TextBox)sender;
				System.Windows.Input.Keyboard.ClearFocus();
				e.Handled = GetHandleEnter(textBox);
			}
		}

		#endregion ClearFocusWhenEnter

		#region CaretAtEndWhenSelectionChanged

		public static bool GetCaretAtEndWhenSelectionChanged(DependencyObject obj)
		{
			return (bool)obj.GetValue(CaretAtEndWhenSelectionChangedProperty);
		}

		public static void SetCaretAtEndWhenSelectionChanged(DependencyObject obj, bool value)
		{
			obj.SetValue(CaretAtEndWhenSelectionChangedProperty, value);
		}

		public static readonly DependencyProperty CaretAtEndWhenSelectionChangedProperty =
			DependencyProperty.RegisterAttached("CaretAtEndWhenSelectionChanged", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnCaretAtEndWhenSelectionChangedChanged));

		private static void OnCaretAtEndWhenSelectionChangedChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (TextBox)dp;
			textBox.SelectionChanged -= TextBox_SelectionChanged;
			if ((bool)e.NewValue)
			{
				textBox.SelectionChanged += TextBox_SelectionChanged;
			}
		}

		private static void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			var textBox = (TextBox)sender;
			var length = textBox.Text?.Length ?? 0;

			if (textBox.CaretIndex == length) return;

			textBox.CaretIndex = length;
		}

		#endregion CaretAtEndWhenSelectionChanged

		public static bool GetHandleEnter(DependencyObject obj)
		{
			return (bool)obj.GetValue(HandleEnterProperty);
		}

		public static void SetHandleEnter(DependencyObject obj, bool value)
		{
			obj.SetValue(HandleEnterProperty, value);
		}

		public static readonly DependencyProperty HandleEnterProperty =
			DependencyProperty.RegisterAttached("HandleEnter", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false));

		public static TextBoxDataType GetDataType(DependencyObject obj)
		{
			return (TextBoxDataType)obj.GetValue(DataTypeProperty);
		}

		public static void SetDataType(DependencyObject obj, TextBoxDataType value)
		{
			obj.SetValue(DataTypeProperty, value);
		}

		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.RegisterAttached("DataType", typeof(TextBoxDataType), typeof(TextBoxHelper), new PropertyMetadata(TextBoxDataType.None, OnDataTypeChanged));

		private static void OnDataTypeChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;

				textBox.TextChanged -= TextBox_Date_TextChanged;

				if ((TextBoxDataType)e.NewValue == TextBoxDataType.Date)
				{
					textBox.MaxLength = "dd/MM/yyyy hh:mm:ss".Length;
					textBox.TextChanged += TextBox_Date_TextChanged;
				}
			}
			catch (Exception)
			{
			}
		}

		private static void TextBox_Date_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)sender;

				var length = textBox.Text?.Length;
				if (length == 2 || length == 5)
				{
					textBox.AppendText("/");
					textBox.CaretIndex = textBox.Text.Length;
				}
				if (length == 13 || length == 16)
				{
					textBox.AppendText(":");
					textBox.CaretIndex = textBox.Text.Length;
				}
				if (length == 10)
				{
					textBox.AppendText(" ");
					textBox.CaretIndex = textBox.Text.Length;
				}

				DateTime date;
				if (DateTime.TryParseExact(textBox.Text, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					SetSelectedDate(textBox, date);
				}
				else
				{
					SetSelectedDate(textBox, null);
				}
			}
			catch (Exception)
			{
			}
		}

		#region SelectedDate

		public static DateTime? GetSelectedDate(DependencyObject obj)
		{
			return (DateTime?)obj.GetValue(SelectedDateProperty);
		}

		public static void SetSelectedDate(DependencyObject obj, DateTime? value)
		{
			obj.SetValue(SelectedDateProperty, value);
		}

		public static readonly DependencyProperty SelectedDateProperty =
			DependencyProperty.RegisterAttached("SelectedDate", typeof(DateTime?), typeof(TextBoxHelper), new PropertyMetadata(null, OnSelectedDateChanged));

		private static void OnSelectedDateChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (TextBox)dp;
			textBox.Text = ((DateTime?)e.NewValue)?.ToString("dd/MM/yyyy hh:mm:ss");
		}

		#endregion SelectedDate
	}
}
