namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System;
	using System.Timers;
	using System.Windows.Threading;

	public class BankingServicesViewModel : BaseViewModel, IBankingServicesViewModel
	{
		public BankingServicesViewModel()
		{
			var _timer = new Timer(1000);
			_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
			_timer.Enabled = true;
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				var now = DateTime.Now;
				MachineTime = now.ToLongTimeString();
				MachineDate = now.ToShortDateString();
			}));
		}

		public void Dispose()
		{
			
		}

		private string _machineTime;
		public string MachineTime
		{
			get { return _machineTime; }
			set { SetProperty(ref _machineTime, value); }
		}

		private string _machineDate;
		public string MachineDate
		{
			get { return _machineDate; }
			set { SetProperty(ref _machineDate, value); }
		}
	}
}