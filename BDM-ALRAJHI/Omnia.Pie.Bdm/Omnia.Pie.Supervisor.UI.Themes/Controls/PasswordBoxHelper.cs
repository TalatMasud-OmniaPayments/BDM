namespace Omnia.Pie.Supervisor.UI.Themes.Controls
{
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	public static class PasswordBoxHelper
	{
		public static string GetPassword(DependencyObject obj)
		{
			return (string)obj.GetValue(PasswordProperty);
		}

		public static void SetPassword(DependencyObject obj, string value)
		{
			obj.SetValue(PasswordProperty, value);
		}

		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnPasswordChanged));

		private static bool GetIsUpdating(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsUpdatingProperty);
		}

		private static void SetIsUpdating(DependencyObject obj, bool value)
		{
			obj.SetValue(IsUpdatingProperty, value);
		}

		private static readonly DependencyProperty IsUpdatingProperty =
			DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxHelper));

		private static void OnPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
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
			}
		}

		private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = (PasswordBox)sender;

			SetIsUpdating(passwordBox, true);
			SetPassword(passwordBox, passwordBox.Password);
			SetIsUpdating(passwordBox, false);
		}

		public static bool GetClearWhenBackspace(DependencyObject obj)
		{
			return (bool)obj.GetValue(ClearWhenBackspaceProperty);
		}

		public static void SetClearWhenBackspace(DependencyObject obj, bool value)
		{
			obj.SetValue(ClearWhenBackspaceProperty, value);
		}

		public static readonly DependencyProperty ClearWhenBackspaceProperty =
			DependencyProperty.RegisterAttached("ClearWhenBackspace", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnClearWhenBackspaceChanged));

		private static void OnClearWhenBackspaceChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			var passwordBox = (PasswordBox)dp;
			passwordBox.PreviewKeyDown -= PasswordBox_PreviewKeyDown;
			if ((bool)e.NewValue)
			{
				passwordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown;
			}
		}

		private static void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back)
			{
				var passwordBox = (PasswordBox)sender;
				SetPassword(passwordBox, null);
				e.Handled = true;
			}
		}
	}
}
