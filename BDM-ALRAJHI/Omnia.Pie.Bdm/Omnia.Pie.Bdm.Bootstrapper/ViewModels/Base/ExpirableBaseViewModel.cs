namespace Omnia.Pie.Bdm.Bootstrapper
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System;
	using System.IO;
	using System.Media;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;

	public abstract class ExpirableBaseViewModel : BaseViewModel
	{
		private short InactivityTimer = 30;
		private DispatcherTimer Timer { get; set; }
		private SoundPlayer _player;
		private readonly IActivityObserver _userActivityService;

		public TimeSpan ExpirationTimer { get; set; }
		public DateTime FromDateTime { get; set; }
		public TimeSpan TimeRemaining { get { return ExpirationTimer - (DateTime.Now - FromDateTime); } set { } }
		public Action ExpiredAction { get; set; }

		public ExpirableBaseViewModel()
		{
			short.TryParse(SystemParametersConfiguration.GetElementValue("InactivityTimeLimit"), out InactivityTimer);
			_userActivityService = new ActivityObserver();

			var codeBase = Assembly.GetExecutingAssembly().CodeBase;
			UriBuilder uri = new UriBuilder(codeBase);
			var path = Uri.UnescapeDataString(uri.Path);
			path = Path.GetDirectoryName(path);
			var filePath = Path.Combine(path, "Resources\\Sounds\\click.wav");

			if (File.Exists(filePath))
			{
				_player = new SoundPlayer(filePath);
			}
		}

		public void StartTimer(TimeSpan interval)
		{
			StartTimer(interval, DateTime.Now);
		}

		public void StartTimer(TimeSpan interval, DateTime dt)
		{
			ExpirationTimer = interval;
			FromDateTime = dt;
			RaisePropertyChanged(nameof(TimeRemaining));
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
			RaisePropertyChanged(nameof(TimeRemaining));

			_player?.Play();

			if (TimeRemaining < TimeSpan.Zero)
			{
				StopTimer();
				ExpiredAction?.Invoke();
			}
		}

		public async void StartUserActivityTimer(CancellationToken cancellationToken)
		{
			StopTimer();

			try
			{
				await Task.Run(async () =>
				{
					var trackingStartTime = DateTime.Now;
					bool timeLimitExceeded;
					do
					{
						await Task.Delay(100);
						var lastActivityTime = _userActivityService.LastActivityTime;
						if (lastActivityTime < trackingStartTime)
						{
							lastActivityTime = trackingStartTime;
						}
						var idlePeriod = DateTime.Now - lastActivityTime;
						timeLimitExceeded = idlePeriod.Seconds > InactivityTimer;

						cancellationToken.ThrowIfCancellationRequested();
					}
					while (!timeLimitExceeded);

					Application.Current.Dispatcher.Invoke(() =>
					{
						StopTimer();
						ExpiredAction?.Invoke();
					});
				}, cancellationToken);
			}
			catch (Exception ex)
			{
				// CancellationToken has been canceled. so stop the task and go back
			}
		}

		protected override void ExecuteBackCommand()
		{
			StopTimer();
			BackAction?.Invoke();
		}

		protected override void ExecuteCancelCommand()
		{
			StopTimer();
			CancelAction?.Invoke();
		}

		protected override bool ExecuteDefaultCommand()
		{
			var result = false;

			if (Validate())
			{
				result = true;
				StopTimer();
				DefaultAction?.Invoke();
			}

			return result;
		}
	}
}