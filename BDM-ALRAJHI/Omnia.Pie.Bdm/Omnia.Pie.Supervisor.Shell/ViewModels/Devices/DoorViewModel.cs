using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Supervisor.Shell.Views;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public class DoorViewModel : MonitorViewModel
	{
		Door door;
		public Door Door
		{
			get { return door; }
			set
			{
				door = value;
				var s = Door?.Id;
				if (s == null)
					return;
				id = Door?.Id.TrimEnd("Door").ToHumanString();
			}
		}

		string id;
		public override string Id => id;

		public override string StatusText => Door?.Status.ToString() ?? "n/a";

		public override MonitorStatus Status
		{
			get
			{
				switch (Door?.Status)
				{
					case DoorStatus.Open:
						return MonitorStatus.Alarm;
					case DoorStatus.Jammed:
						return MonitorStatus.Error;
					case DoorStatus.NotAvailable:
					case null:
						return MonitorStatus.Off;
					default:
						return MonitorStatus.On;
				}
			}
		}
	}
}