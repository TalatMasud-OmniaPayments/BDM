namespace Omnia.Pie.Supervisor.Shell.Service
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Supervisor.Shell.Utilities;
    using Omnia.Pie.Supervisor.Shell.ViewModels;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
    using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Base;
	using Omnia.Pie.Vtm.Services.Interface;
	using System;
	using System.Linq;

	public class SupervisorService : BindableBase
	{
		public SupervisorService()
		{
            //_cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();

            if (DoorsOpen)
				Doors_DoorsStatusChanged(null, null);

			_doors.DoorsStatusChanged += Doors_DoorsStatusChanged;
            _deviceSensors.OperatorStatusChanged += DeviceSensors_OperatorStatusChanged;


        }

        

        //public static SupervisorViewModel supervisoryWindow = ServiceLocator.Instance.Resolve<ICashAcceptor>();;
        readonly ICashAcceptor model = ServiceLocator.Instance.Resolve<ICashAcceptor>();
        //public override IDevice Device => model;

        readonly ICashDispenser _cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
        protected readonly IChannelManagementService _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
		private readonly IJournal journal = ServiceLocator.Instance.Resolve<IJournal>();
		private readonly IDoors _doors = ServiceLocator.Instance.Resolve<IDoors>();
        public readonly IDeviceSensors _deviceSensors = ServiceLocator.Instance.Resolve<IDeviceSensors>();
        private readonly IUserRolesStore _userRolesStore = ServiceLocator.Instance.Resolve<IUserRolesStore>();
        public ICashAcceptor CashAcceptor { get; } = ServiceLocator.Instance.Resolve<ICashAcceptor>();
        public CITSteps steps = new CITSteps();
        //public bool DoorsOpen => _doors.AllDoors.Any(i => i.Status == DoorStatus.Open);
        public bool DoorsOpen { get; set; } = true;
        //DoorsOpen => _doors.AllDoors.Any(i => i.Status == DoorStatus.Open);


        public event EventHandler DoorsOpenChanged;
        public event EventHandler<SensorsStatus> SupervisoryStatusUpdated;

        #region Login properties

        private bool _isSupervisorMode;
		public bool IsSupervisorMode => _isSupervisorMode;

		private bool _isOperatorMode;
		public bool IsOperatorMode => _isOperatorMode;

		private bool _isLoggedInMode;
		public bool IsLoggedInMode => _isLoggedInMode;

        private UserRoles _userRoles;
        public UserRoles UserRoles => _userRoles;


        #endregion

        public event EventHandler LoginEvent;
		public event EventHandler LogoutEvent;

		private bool _displayProgress;

		public bool DisplayProgress
		{
			get { return _displayProgress; }
			set { SetProperty(ref _displayProgress, value); }
		}


        private void DeviceSensors_OperatorStatusChanged(object sender, SensorsStatus status)
        {
            Console.Write("DeviceConfiguratio_SensorStatusChanged called with status: " + status);

            var isCIT = this.UserRoles?.ClearCashIn ?? false;
            if (status == SensorsStatus.Run)
            {

                if (!isCIT)
                {
                    var isSLM = this.UserRoles?.Diagnostic ?? false;
                    if (isSLM)
                    {

                        var cessetteStatus = _cashDispenser.GetCashDispenserStatus();

                        if (cessetteStatus == CashDispenserCassetteStatus.MISSING)
                        {
                            Screens.supervisoryWindow.DevicesConf.Message = "Please make sure all the cassettes are installed and after that please initialize the BRM.";
                            Screens.supervisoryWindow.SelectedPage = Screens.supervisoryWindow.DevicesConf;

                            return;
                        }
                    }
                    Logout();
                    Screens.OutOfServiceViewsHide();
                    _channelManagementService.InsertEventAsync("InService", "True");

                    _channelManagementService.InsertEventAsync("Supervisor", "False");
                    Screens.UpdateToMainApp(true);
                }
                //DoorsOpenChanged?.Invoke(this, EventArgs.Empty);
                SupervisoryStatusUpdated?.Invoke(sender, status);
            }
            else if (status == SensorsStatus.Supervisor)
            {
                _channelManagementService.InsertEventAsync("InService", "False");
                _channelManagementService.InsertEventAsync("Supervisor", "True");

                if (IsLoggedInMode)
                {
                    /*DoorsOpenChanged?.Invoke(this, EventArgs.Empty);
                    Screens.SetDoorAccessed();
                    var openDoors = _doors.AllDoors.Where(x => x.Status == DoorStatus.Open).ToList();

                    foreach (var item in openDoors)
                    {
                        journal.DoorOpen(item.Id);
                        _channelManagementService.InsertEventAsync($"Door {item.Id} Opened", "True");
                    }*/

                    /*if (!IsLoggedInMode)
                    {
                        Screens.OutOfServiceViewsShow();
                        _channelManagementService.InsertEventAsync("OutOfService", "True");
                    }*/
                    //SupervisoryStatusUpdated?.Invoke(sender, status);
                    Screens.UpdateToMainApp(false);
                }
                else
                {
                    //SupervisoryStatusUpdated?.Invoke(sender, status);
                    Screens.UpdateToMainApp(false);
                }
                SupervisoryStatusUpdated?.Invoke(sender, status);

            }
        }
        
        private void Doors_DoorsStatusChanged(object sender, EventArgs e)
		{
            DoorsOpen = _doors.AllDoors.Any(i => i.Status == DoorStatus.Open);
            var isAllDoorUnAvailable = true;

            foreach (var item in _doors.AllDoors)
            {
                journal.DoorStatus(item.Id, item.Status);

                if (item.Status != DoorStatus.NotAvailable)
                {
                    isAllDoorUnAvailable = false;
                }
            }

            /*if (isAllDoorUnAvailable && IsLoggedInMode)
            {
                Screens.supervisoryWindow.SelectedPage = Screens.supervisoryWindow.ConnectionLost;
                Screens.UpdateToMainApp(false);
                return;
            }*/

            var isCIT = this.UserRoles?.ClearCashIn ?? false;
            var isSLM = this.UserRoles?.Diagnostic ?? false;
            if (!DoorsOpen)
			{
                
                
                /*else
                {
                    Screens.supervisoryWindow.SelectedPage = Screens.supervisoryWindow.DashboardViewModel;
                }*/
            }
			else
			{

                if (IsLoggedInMode) { 

                    /*if (!isCIT)
                    {
                        DoorsOpenChanged?.Invoke(this, EventArgs.Empty);
                        Screens.UpdateToMainApp(false);
                    }*/
                Screens.SetDoorAccessed();
                var openDoors  = _doors.AllDoors.Where(x => x.Status == DoorStatus.Open).ToList();

				foreach (var item in openDoors)
				{
                        if (item.Id == "SafeDoor")
                        {
                            if (isCIT)
                            {
                                DoorsOpenChanged?.Invoke(this, EventArgs.Empty);
                                Screens.UpdateToMainApp(false);
                            }
                        }
					//journal.DoorStatus(item.Id, item.Status);
					_channelManagementService.InsertEventAsync($"Door {item.Id} Opened", "True");
				}

				/*if (!IsLoggedInMode)
				{
					Screens.OutOfServiceViewsShow();
					_channelManagementService.InsertEventAsync("OutOfService", "True");
				}*/
                
                }

            }

		}
       
        public void UpdateLogout()
        {

        }
        public void Login(bool isSupervisorMode, String username)
		{
			journal.Write("Logged in mode: " + username);
			_channelManagementService.InsertEventAsync( "Logged in mode: ",  username);

			SetProperty(ref _isSupervisorMode, isSupervisorMode);
			SetProperty(ref _isOperatorMode, !isSupervisorMode);
			SetProperty(ref _isLoggedInMode, true);
            SetProperty(ref _userRoles, LoadRoles(username));

            var opStatus = _deviceSensors.GetOperatorStatus();

            if (opStatus == SensorsStatus.Supervisor)
            {
                Screens.UpdateToMainApp(false);
                DeviceSensors_OperatorStatusChanged(this, opStatus);
                //LoginEvent?.Invoke(this, EventArgs.Empty);
            }
            var isCIT = this.UserRoles?.ClearCashIn ?? false;
            if (isCIT)
            {
                steps.oldCassettes = CashAcceptor.GetOldCassettes();
            }
            //Doors_DoorsStatusChanged(null, null);
            //LoginEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Logout()
		{

            
			if (!IsLoggedInMode)
				return;

             
			journal.Write(IsSupervisorMode ? "EXITED SUPERVISOR MODE" : "EXITED OPERATOR MODE");
			_channelManagementService.InsertEventAsync(IsSupervisorMode ? "EXITED SUPERVISOR MODE" : "EXITED OPERATOR MODE", "True");

			SetProperty(ref _isSupervisorMode, false);
			SetProperty(ref _isOperatorMode, false);
			SetProperty(ref _isLoggedInMode, false);
            
            LogoutEvent?.Invoke(this, EventArgs.Empty);
            this._userRoles = null;
            Screens.SetLogout();


        }

        public UserRoles LoadRoles(String usernname)
        {
            return _userRolesStore.GetUserRole(usernname);
        }
        
    }
}