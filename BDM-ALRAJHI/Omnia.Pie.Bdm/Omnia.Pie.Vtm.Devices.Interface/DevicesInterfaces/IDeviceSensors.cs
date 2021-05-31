

namespace Omnia.Pie.Vtm.Devices.Interface
{
    using System;
    using Omnia.Pie.Vtm.Devices.Interface.Entities;
    public interface IDeviceSensors : IDevice
    {
        event EventHandler<SensorsStatus> OperatorStatusChanged;
        SensorsStatus GetOperatorStatus();
    }
}
