namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Microsoft.Practices.EnterpriseLibrary.Logging;

	public static class PasswordBoxExtender
	{
		#region Password

		public static string GetPassword(DependencyObject obj)
		{
			return (string)obj.GetValue(PasswordProperty);
		}

		public static void SetPassword(DependencyObject obj, string value)
		{
			obj.SetValue(PasswordProperty, value);
		}

		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxExtender), new PropertyMetadata(string.Empty, OnPasswordChanged));

		public static string GetMaskedPassword(DependencyObject obj)
		{
			return (string)obj.GetValue(MaskedPasswordProperty);
		}

		public static void SetMaskedPassword(DependencyObject obj, string value)
		{
			obj.SetValue(MaskedPasswordProperty, value);
		}

		public static readonly DependencyProperty MaskedPasswordProperty =
			DependencyProperty.RegisterAttached("MaskedPassword", typeof(string), typeof(PasswordBoxExtender), new PropertyMetadata(string.Empty));

		private static bool GetIsUpdating(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsUpdatingProperty);
		}

		private static void SetIsUpdating(DependencyObject obj, bool value)
		{
			obj.SetValue(IsUpdatingProperty, value);
		}

		private static readonly DependencyProperty IsUpdatingProperty =
			DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxExtender));

		private static void OnPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var passwordBox = (PasswordBox)dp;

				if (passwordBox != null)
				{
					passwordBox.PasswordChanged -= HandlePasswordChanged;

					var newPassword = (string)e.NewValue;

					if (!GetIsUpdating(passwordBox))
					{
						passwordBox.Password = newPassword;
					}

					passwordBox.PasswordChanged += HandlePasswordChanged;

					SetMaskedPassword(passwordBox, new String(passwordBox.PasswordChar, newPassword?.Length ?? 0));
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
		{
			try
			{
				var passwordBox = (PasswordBox)sender;

				SetIsUpdating(passwordBox, true);
				SetPassword(passwordBox, passwordBox.Password);
				SetIsUpdating(passwordBox, false);
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		#endregion Password

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
			DependencyProperty.RegisterAttached("ClearWhenBackspace", typeof(bool), typeof(PasswordBoxExtender), new PropertyMetadata(false, OnClearWhenBackspaceChanged));

		private static void OnClearWhenBackspaceChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var passwordBox = (PasswordBox)dp;
				passwordBox.PreviewKeyDown -= PasswordBox_PreviewKeyDown;
				if ((bool)e.NewValue)
				{
					passwordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Back)
				{
					var passwordBox = (PasswordBox)sender;
					SetPassword(passwordBox, null);
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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
			DependencyProperty.RegisterAttached("ClearFocusWhenEnter", typeof(bool), typeof(PasswordBoxExtender), new PropertyMetadata(false, OnClearFocusWhenEnterChanged));

		private static void OnClearFocusWhenEnterChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				var passwordBox = (PasswordBox)dp;
				passwordBox.PreviewKeyDown -= PasswordBox_PreviewKeyDown_Return;
				if ((bool)e.NewValue)
				{
					passwordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown_Return;
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}

		private static void PasswordBox_PreviewKeyDown_Return(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					var passwordBox = (PasswordBox)sender;
					System.Windows.Input.Keyboard.ClearFocus();
					e.Handled = GetHandleEnter(passwordBox);
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
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
			DependencyProperty.RegisterAttached("HandleEnter", typeof(bool), typeof(PasswordBoxExtender), new PropertyMetadata(false));

		public static PasswordBoxDataType GetDataType(DependencyObject obj)
		{
			return (PasswordBoxDataType)obj.GetValue(DataTypeProperty);
		}

		public static void SetDataType(DependencyObject obj, PasswordBoxDataType value)
		{
			obj.SetValue(DataTypeProperty, value);
		}

		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.RegisterAttached("DataType", typeof(PasswordBoxDataType), typeof(PasswordBoxExtender), new PropertyMetadata(PasswordBoxDataType.None));

		public static string GetKeyboardTitle(DependencyObject obj)
		{
			return (string)obj.GetValue(KeyboardTitleProperty);
		}

		public static void SetKeyboardTitle(DependencyObject obj, string value)
		{
			obj.SetValue(KeyboardTitleProperty, value);
		}

		public static readonly DependencyProperty KeyboardTitleProperty =
			DependencyProperty.RegisterAttached("KeyboardTitle", typeof(string), typeof(PasswordBoxExtender), new PropertyMetadata(null));
	}

	public enum PasswordBoxDataType
	{
		None,
		Numeric
	}
}