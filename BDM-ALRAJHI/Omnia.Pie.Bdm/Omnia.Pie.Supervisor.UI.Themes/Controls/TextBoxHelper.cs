using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.UI.Themes.Controls
{
	public enum TextBoxDataType
	{
		None,
		Numeric,
		Alphanumeric
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
			if (dp is TextBox)
			{
				var textBox = dp as TextBox;
				if (textBox != null)
				{
					textBox.PreviewKeyDown -= TextBox_PreviewKeyDown_Back;
					if ((bool)e.NewValue)
					{
						textBox.PreviewKeyDown += TextBox_PreviewKeyDown_Back;
					}
				}
			}
			else if (dp is PasswordBox)
			{
				var passwordBox = dp as PasswordBox;
				if (passwordBox != null)
				{
					passwordBox.PreviewKeyDown -= PasswordBox_PreviewKeyDown_Back;
					if ((bool)e.NewValue)
					{
						passwordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown_Back;
					}
				}
			}
		}

		private static void PasswordBox_PreviewKeyDown_Back(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back)
			{
				var passwordBox = (PasswordBox)sender;
				passwordBox.Password = null;
				e.Handled = true;
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
			if (dp is TextBox)
			{
				var textBox = dp as TextBox;
				if (textBox != null)
				{
					textBox.PreviewKeyDown -= TextBox_PreviewKeyDown_Return;
					if ((bool)e.NewValue)
					{
						textBox.PreviewKeyDown += TextBox_PreviewKeyDown_Return;
					}
				}
			}
			else if (dp is PasswordBox)
			{
				var passwordBox = dp as PasswordBox;
				if (passwordBox != null)
				{
					passwordBox.PreviewKeyDown -= PasswordBox_PreviewKeyDown_Back;
					if ((bool)e.NewValue)
					{
						passwordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown_Return;
					}
				}
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

		private static void PasswordBox_PreviewKeyDown_Return(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				var passwordBox = (PasswordBox)sender;
				System.Windows.Input.Keyboard.ClearFocus();
				e.Handled = GetHandleEnter(passwordBox);
			}
		}

		#endregion ClearFocusWhenEnter

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
			DependencyProperty.RegisterAttached("DataType", typeof(TextBoxDataType), typeof(TextBoxHelper), new PropertyMetadata(TextBoxDataType.None));
	}
}
