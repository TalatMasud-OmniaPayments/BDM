using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.UI.Themes.Controls
{
	public enum KeyboardView
	{
		Keypad,

		Alphabetic, // -> Numeric
		Numeric, // -> Alphabetic, Symbols
		Symbols, // -> Alphabetic, Numeric

		Alphanumeric,

		Default = Alphabetic
	}

	public class Keyboard : Control
	{
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

		public ICommand SendKeyCommand { get; }
		public ICommand SetKeyboardViewCommand { get; }
		public ICommand SetShiftCommand { get; }

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

		public KeyboardView KeyboardView
		{
			get { return (KeyboardView)GetValue(KeyboardViewProperty); }
			set { SetValue(KeyboardViewProperty, value); }
		}

		public static readonly DependencyProperty KeyboardViewProperty =
			DependencyProperty.Register("KeyboardView", typeof(KeyboardView), typeof(Keyboard), new PropertyMetadata(KeyboardView.Default));

		public bool IsShift
		{
			get { return (bool)GetValue(IsShiftProperty); }
			set { SetValue(IsShiftProperty, value); }
		}

		public static readonly DependencyProperty IsShiftProperty =
			DependencyProperty.Register("IsShift", typeof(bool), typeof(Keyboard), new PropertyMetadata(false));

		private void SendKey(string key)
		{
			if (key?.Length == 1 && Char.IsUpper(key[0]))
			{
				IsShift = false;
			}

			System.Windows.Forms.SendKeys.SendWait(key);
		}	
	}
}
