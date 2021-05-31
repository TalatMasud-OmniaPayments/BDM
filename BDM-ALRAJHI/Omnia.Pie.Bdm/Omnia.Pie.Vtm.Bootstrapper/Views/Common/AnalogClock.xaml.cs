namespace Omnia.Pie.Vtm.Bootstrapper.Views
{
	using MohammadDayyanCalendar;
	using System;
	using System.Timers;
	using System.Windows.Controls;
	using System.Windows.Threading;

	public partial class AnalogClock : UserControl
	{
		public AnalogClock()
		{
			InitializeComponent();

			var _date = DateTime.Now;
			var _timer = new Timer(1000);
			var _calendar = new MDCalendar();
			var _time = TimeZone.CurrentTimeZone;
			var _difference = _time.GetUtcOffset(_date);
			var _currentTime = _calendar.Time() + (uint)_difference.TotalSeconds;

			lunarCalendar.Text = _calendar.Date("Y/m/D  W", _currentTime, true);
			solarCalendar.Text = _calendar.Date("P Z/e/d", _currentTime, false);

			_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
			_timer.Enabled = true;
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				secondHand.Angle = DateTime.Now.Second * 6;
				minuteHand.Angle = DateTime.Now.Minute * 6;
				hourHand.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5);
			}));
		}
	}
}