namespace Omnia.Pie.Vtm.Bootstrapper.Configurations
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Forms;
	using System.Windows.Forms.Integration;
	using System.Windows.Interactivity;

	internal class KeyboardBehavior : Behavior<Window>
	{
		private IPinPad _pinPad { get; }

		public KeyboardBehavior(IPinPad pinPad)
		{
			_pinPad = pinPad ?? throw new ArgumentNullException(nameof(pinPad));
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			_pinPad.CancelPressed += PinPad_CancelPressed;
			_pinPad.ClearPressed += PinPad_ClearPressed;
			_pinPad.DigitPressed += PinPad_DigitPressed;
			_pinPad.EnterPressed += PinPad_EnterPressed;

			ElementHost.EnableModelessKeyboardInterop(AssociatedObject);
		}

		protected override void OnDetaching()
		{
			_pinPad.CancelPressed -= PinPad_CancelPressed;
			_pinPad.ClearPressed -= PinPad_ClearPressed;
			_pinPad.DigitPressed -= PinPad_DigitPressed;
			_pinPad.EnterPressed -= PinPad_EnterPressed;

			base.OnDetaching();
		}

		private void PinPad_CancelPressed(object sender, EventArgs e)
		{
			SendKey("{ESC}");
		}

		private void PinPad_ClearPressed(object sender, EventArgs e)
		{
			SendKey("{BACKSPACE}");
		}

		private void PinPad_DigitPressed(object sender, PinPadDigitPressedEventArgs e)
		{
			SendKey(e.Digit.ToString(CultureInfo.InvariantCulture));
		}

		private void PinPad_EnterPressed(object sender, EventArgs e)
		{
			SendKey("{ENTER}");
		}

		private void SendKey(string key)
		{
			if (AssociatedObject?.IsActive == false)
			{
				AssociatedObject.Activate();
			}

			SendKeys.SendWait(key);
		}
	}
}