namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;

	public interface IDoors : IDevice
	{
		event EventHandler DoorsStatusChanged;
        //event EventHandler ShieldStatusChanged;
        event EventHandler SafeStatusChanged;
        DoorStatus GetSafeDoorStatus();
        Door[] AllDoors { get; }
	}
}