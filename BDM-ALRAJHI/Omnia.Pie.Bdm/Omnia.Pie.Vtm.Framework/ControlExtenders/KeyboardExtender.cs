namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Microsoft.Practices.EnterpriseLibrary.Logging;

	public class Keyboard : Control
	{
		public ICommand SendKeyCommand { get; }
		public ICommand SetKeyboardViewCommand { get; }
		public ICommand SetShiftCommand { get; }
		public KeyboardView KeyboardView
		{
			get { return (KeyboardView)GetValue(KeyboardViewProperty); }
			set { SetValue(KeyboardViewProperty, value); }
		}

		public bool IsShift
		{
			get { return (bool)GetValue(IsShiftProperty); }
			set { SetValue(IsShiftProperty, value); }
		}

		public Keyboard()
		{
			SendKeyCommand = new DelegateCommand<string>(SendKey);
			SetKeyboardViewCommand = new DelegateCommand<KeyboardView>(v => KeyboardView = v);
			SetShiftCommand = new DelegateCommand<bool>(v => IsShift = v);
		}

		static Keyboard()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Keyboard), new FrameworkPropertyMetadata(typeof(Keyboard)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		public static string GetTitle(DependencyObject obj)
		{
			return (string)obj.GetValue(TitleProperty);
		}

		public static void SetTitle(DependencyObject obj, string value)
		{
			obj.SetValue(TitleProperty, value);
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.RegisterAttached("Title", typeof(string), typeof(Keyboard), new PropertyMetadata(null));

		public static readonly DependencyProperty KeyboardViewProperty =
			DependencyProperty.Register("KeyboardView", typeof(KeyboardView), typeof(Keyboard), new PropertyMetadata(KeyboardView.Default));

		public static readonly DependencyProperty IsShiftProperty =
			DependencyProperty.Register("IsShift", typeof(bool), typeof(Keyboard), new PropertyMetadata(false));

		private void SendKey(string key)
		{
			try
			{
				Logger.Writer.Write(key, "General", -1, 1, System.Diagnostics.TraceEventType.Information);

				if (key?.Length == 1 && Char.IsUpper(key[0]))
				{
					IsShift = false;
				}

				System.Windows.Forms.SendKeys.SendWait(key);
			}
			catch (Exception ex)
			{
				Logger.Writer.Write(ex.GetBaseException().Message);
			}
		}
	}

	public enum KeyboardView
	{
		Keypad,
		Alphabetic,
		Numeric,
		Symbols,
		Alphanumeric,
		AlphanumericWithoutSpace,
		AlphabeticOnly,
		Default = Alphabetic
	}
}