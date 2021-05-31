namespace Omnia.Pie.Vtm.Devices.Doors
{
	using System;
	using System.Windows.Forms;
	using AxNXDoorsXLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface;
    using System.Linq;

    public class Doors : Device, IDoors
	{
		AxNXDoorsX ax;

		public readonly Door CabinetDoor = new Door(nameof(CabinetDoor));
		public readonly Door SafeDoor = new Door(nameof(SafeDoor));
		public readonly Door VandalShieldDoor = new Door(nameof(VandalShieldDoor));
		public readonly Door CabinetFrontDoor = new Door(nameof(CabinetFrontDoor));
		public readonly Door CabinetRearDoor = new Door(nameof(CabinetRearDoor));
		//public readonly Door CabinetLeftDoor = new Door(nameof(CabinetLeftDoor));
		//public readonly Door CabinetRightDoor = new Door(nameof(CabinetRightDoor));

		public event EventHandler DoorsStatusChanged;
        //public event EventHandler ShieldStatusChanged;


        //this is a test change2
        public event EventHandler SafeStatusChanged;

        public Doors(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights)
			: base(deviceErrorStore, logger, null, guideLights)
		{
			Logger.Info("Doors Initialized---");
		}

		#region Overridden Functions

		protected override AxHost CreateAx()
		{
			return ax = new AxNXDoorsX();
		}

		protected override void OnInitialized()
		{
			ax.DoorChanged += Ax_DoorChanged;
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.ResetComplete += Ax_ResetComplete;
            //ax.ShieldChanged += Ax_ShieldChanged;
            ax.SafeChanged += Ax_SafeChanged;
			Ax_DoorChanged(null, null);
		}


        protected override int OpenSessionSync(int timeout)
		{
			return ax.OpenSessionSync(timeout);
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

		protected override void OnDisposing()
		{
			ax.DoorChanged -= Ax_DoorChanged;
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.ResetComplete -= Ax_ResetComplete;
            //ax.ShieldChanged -= Ax_ShieldChanged;
            ax.SafeChanged -= Ax_SafeChanged;
        }

		#endregion

		#region Public Functions

		public Door[] AllDoors => new[] {
			CabinetDoor,
			SafeDoor,
			VandalShieldDoor,
			CabinetFrontDoor,
			CabinetRearDoor,
			//CabinetLeftDoor,
			//CabinetRightDoor
		};

		public override Task ResetAsync() => ResetOperation.StartAsync(() => ax.Reset());
        public DoorStatus GetSafeDoorStatus()
        {
            var openDoors = AllDoors.ToList();

            foreach (var item in openDoors)
            {
                if (item.Id == "SafeDoor")
                {
                    return item.Status;
                }
            }
            return DoorStatus.Closed;
        }
        #endregion

        #region Private Functions & Events

        private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
		}

		private void Ax_FatalError(object sender, _DNXDoorsXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXDoorsXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DoorChanged(object sender, _DNXDoorsXEvents_DoorChangedEvent e) => OnEvent(() =>
		{
			CabinetDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(0), true);
			SafeDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(1), true);
			VandalShieldDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(2), true);
			CabinetFrontDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(3), true);
			CabinetRearDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(4), true);
			//CabinetLeftDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(5), true);
			//CabinetRightDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(6), true);
			DoorsStatusChanged?.Invoke(this, EventArgs.Empty);

			foreach (var i in AllDoors) {
                Logger.Info($"{GetType().Name}.{i.Id}.Status={i.Status}");     
            }

				
		});
        /*private void Ax_ShieldChanged(object sender, _DNXDoorsXEvents_ShieldChangedEvent e) => OnEvent(() =>
        {
            CabinetDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(0), true);
            SafeDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(1), true);
            VandalShieldDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(2), true);
            CabinetFrontDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(3), true);
            CabinetRearDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(4), true);
            CabinetLeftDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(5), true);
            CabinetRightDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(6), true);
            ShieldStatusChanged?.Invoke(this, EventArgs.Empty);

            foreach (var i in AllDoors)
                Logger.Info($"{GetType().Name}.{i.Id}.Status={i.Status}");
        });*/

        private void Ax_SafeChanged(object sender, _DNXDoorsXEvents_SafeChangedEvent e)
        {
            
            CabinetDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(0), true);
            SafeDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(1), true);
            VandalShieldDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(2), true);
            CabinetFrontDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(3), true);
            CabinetRearDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(4), true);
            //CabinetLeftDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(5), true);
            //CabinetRightDoor.Status = (DoorStatus)Enum.Parse(typeof(DoorStatus), ax.GetDoorStatus(6), true);
            SafeStatusChanged?.Invoke(this, EventArgs.Empty);
        }


        #endregion
    }
}