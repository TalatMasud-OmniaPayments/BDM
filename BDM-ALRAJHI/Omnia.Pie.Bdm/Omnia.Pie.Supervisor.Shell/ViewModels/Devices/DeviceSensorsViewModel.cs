using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
    public class DeviceSensorsViewModel : DeviceViewModel
    {
        public DeviceSensorsViewModel()
        {
            //model.ShieldStatusChanged += (s, e) => Load();
            Load();
        }

        public override string Id => "Sensors";
        private readonly IDeviceSensors model = ServiceLocator.Instance.Resolve<IDeviceSensors>();
        public override IDevice Device => model;

        //public DoorViewModel[] Doors { get; private set; }

        public override void Load()
        {
            /*Doors = model.AllDoors.Select(i => new DoorViewModel
            {
                Door = i
            }).ToArray();
            RaisePropertyChanged(nameof(Doors));*/
        }
    }
}
