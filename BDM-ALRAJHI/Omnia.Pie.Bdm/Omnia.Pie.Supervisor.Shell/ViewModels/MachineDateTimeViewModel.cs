using System.Windows.Forms;
using System;
using Omnia.Pie.Vtm.Framework.Base;

namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class MachineDateTimeViewModel : BindableBase
	{
		private Timer _timer;

		public MachineDateTimeViewModel()
		{
			SetupTimer();
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

		private void SetupTimer()
		{
			_timer = new Timer();
			_timer.Tick += _timer_Tick;
			_timer.Interval = 1000;
			_timer.Start();
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			var now = DateTime.Now;
			MachineTime = now.ToLongTimeString();
			MachineDate = now.ToShortDateString();
		}
	}
}
