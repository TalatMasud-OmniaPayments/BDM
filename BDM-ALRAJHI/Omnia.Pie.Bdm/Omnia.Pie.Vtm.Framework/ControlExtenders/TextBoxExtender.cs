namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Microsoft.Practices.EnterpriseLibrary.Logging;

	public static class TextBoxExtender
	{
		public static string GetPlaceholder(DependencyObject obj)
		{
			return (string)obj.GetValue(PlaceholderProperty);
		}

		public static void SetPlaceholder(DependencyObject obj, string value)
		{
			obj.SetValue(PlaceholderProperty, value);
		}

		public static readonly DependencyProperty PlaceholderProperty =
			DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBoxExtender), new PropertyMetadata(null));

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
			DependencyProperty.RegisterAttached("ClearWhenBackspace", typeof(bool), typeof(TextBoxExtender), new PropertyMetadata(false, OnClearWhenBackspaceChanged));

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
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void TextBox_PreviewKeyDown_Back(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Back)
				{
					var textBox = (TextBox)sender;

					if (textBox != null && !string.IsNullOrEmpty(textBox?.Text))
					{
						textBox.Text = textBox?.Text?.Substring(0, textBox.Text.Length - 1);
					}

					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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
			DependencyProperty.RegisterAttached("IgnoreFirstSpaces", typeof(bool), typeof(TextBoxExtender), new PropertyMetadata(false, OnIgnoreFirstSpacesChanged));

		private static void OnIgnoreFirstSpacesChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.TextChanged -= TextBox_TextChanged;
				if ((bool)e.NewValue)
				{
					textBox.TextChanged += TextBox_TextChanged;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)sender;
				if (textBox.Text?.Length > 0 && string.IsNullOrWhiteSpace(textBox.Text))
				{
					textBox.Text = null;
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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
			DependencyProperty.RegisterAttached("ClearFocusWhenEnter", typeof(bool), typeof(TextBoxExtender), new PropertyMetadata(false, OnClearFocusWhenEnterChanged));

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
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void TextBox_PreviewKeyDown_Return(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					var textBox = (TextBox)sender;
					System.Windows.Input.Keyboard.ClearFocus();
					e.Handled = GetHandleEnter(textBox);
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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
			DependencyProperty.RegisterAttached("CaretAtEndWhenSelectionChanged", typeof(bool), typeof(TextBoxExtender), new PropertyMetadata(false, OnCaretAtEndWhenSelectionChangedChanged));

		private static void OnCaretAtEndWhenSelectionChangedChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.SelectionChanged -= TextBox_SelectionChanged;
				if ((bool)e.NewValue)
				{
					textBox.SelectionChanged += TextBox_SelectionChanged;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)sender;
				var length = textBox.Text?.Length ?? 0;

				if (textBox.CaretIndex == length)
					return;

				textBox.CaretIndex = length;
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
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
			DependencyProperty.RegisterAttached("HandleEnter", typeof(bool), typeof(TextBoxExtender), new PropertyMetadata(false));

		public static TextBoxDataType GetDataType(DependencyObject obj)
		{
			return (TextBoxDataType)obj.GetValue(DataTypeProperty);
		}

		public static void SetDataType(DependencyObject obj, TextBoxDataType value)
		{
			obj.SetValue(DataTypeProperty, value);
		}

		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.RegisterAttached("DataType", typeof(TextBoxDataType), typeof(TextBoxExtender), new PropertyMetadata(TextBoxDataType.None, OnDataTypeChanged));

		private static void OnDataTypeChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.KeyUp -= TextBox_Date_TextChanged;

				if ((TextBoxDataType)e.NewValue == TextBoxDataType.Date)
				{
					textBox.MaxLength = BootstrapperConfiguration.DateFormat.Length;
					textBox.KeyUp += TextBox_Date_TextChanged;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void TextBox_Date_TextChanged(object sender, KeyEventArgs e)
		{
			try
			{
				var textBox = (TextBox)sender;
				if (e.Key == Key.Back)
				{
					textBox.Text = string.Empty;
					e.Handled = true;
					return;
				}
				var length = textBox.Text?.Length;
				if (length == 4 || length == 7)
				{
					textBox.AppendText("/");
					textBox.CaretIndex = textBox.Text.Length;
				}

				DateTime date;
				if (DateTime.TryParseExact(textBox.Text, BootstrapperConfiguration.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					SetSelectedDate(textBox, date);
				}
				else
				{
					SetSelectedDate(textBox, null);

					if (length > 10)
					{
						textBox.Text = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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

		public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.RegisterAttached("SelectedDate", typeof(DateTime?), typeof(TextBoxExtender), new PropertyMetadata(null, OnSelectedDateChanged));

		private static void OnSelectedDateChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var textBox = (TextBox)dp;
				textBox.Text = ((DateTime?)e.NewValue)?.ToString(BootstrapperConfiguration.DateFormat);
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		#endregion SelectedDate
	}

	public enum TextBoxDataType
	{
		None,
		Numeric,
		Alphanumeric,
		AlphanumericWithoutSpace,
		AlphabeticOnly,
		Date
	}
}