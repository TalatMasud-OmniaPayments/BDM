namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using System;
    using System.ComponentModel;
    using System.Windows.Threading;

    public class TimedErrorViewModel : BaseViewModel, ITimedErrorViewModel
    {
		public TimedErrorViewModel()
		{
			
		}

		public string ErrorMessage { get; set; }
        public Action ExpiredAction { get; set; }

        private TimeSpan ExpirationTimer { get; set; }
        private DateTime FromDateTime { get; set; }
        private DispatcherTimer Timer { get; set; }

        public TimeSpan TimeRemaining { get { return ExpirationTimer - (DateTime.Now - FromDateTime); } set { } }

        public void Type(ErrorType errorType)
		{
			ErrorMessage = Properties.Resources.ResourceManager.GetString($"Error{errorType}", Properties.Resources.Culture);
			OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage"));
		}

		public void Dispose()
		{

		}

        public void StartTimer(TimeSpan interval)
        {
            StartTimer(interval, DateTime.Now);
        }

        public void StartTimer(TimeSpan interval, DateTime dt)
        {
            ExpirationTimer = interval;
            FromDateTime = dt;
            StartTimer();
        }

        private void StartTimer()
        {
            StopTimer();

            Timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 1000) };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        protected void StopTimer()
        {
            if (Timer == null) return;

            Timer.Stop();
            Timer.Tick -= Timer_Tick;
            Timer = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           
            if (TimeRemaining < TimeSpan.Zero)
            {
                StopTimer();
                ExpiredAction?.Invoke();
            }
        }
    }
}