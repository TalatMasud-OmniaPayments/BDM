using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
	public class DoorsViewModel : DeviceViewModel
	{
		public DoorsViewModel()
		{
			model.DoorsStatusChanged += (s, e) => Load();
            //model.ShieldStatusChanged += (s, e) => Load();
            model.SafeStatusChanged += (s, e) => Load();
            Load();
		}

		public override string Id => "Doors";
		private readonly IDoors model = ServiceLocator.Instance.Resolve<IDoors>();
		public override IDevice Device => model;

		public DoorViewModel[] Doors { get; private set; }

		public override void Load()
		{
			Doors = model.AllDoors.Select(i => new DoorViewModel
			{
				Door = i
			}).ToArray();
			RaisePropertyChanged(nameof(Doors));
		}
	}
}
