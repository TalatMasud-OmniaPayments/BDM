using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Applications;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Supervisor.Shell.ViewModels.Devices;
using Omnia.Pie.Supervisor.Shell.ViewModels.Pages;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class SupervisorViewModel : ViewModel
	{
		public SupervisorService Context { get; } = ServiceLocator.Instance.Resolve<SupervisorService>();
        public  ICashAcceptor CashAcceptor { get; } = ServiceLocator.Instance.Resolve<ICashAcceptor>();
        public IReceiptPrinter ReceiptPrinter { get; } = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
        public IPinPad PinPad { get; } = ServiceLocator.Instance.Resolve<IPinPad>();

        private IAuxiliaries auxiliaries;

		public SupervisorViewModel(ApplicationCoordinator applicationCoordinator)
		{

			Pages = new ObservableCollection<PageViewModel>();
			//Pages.Add(LoginViewModel = new LoginViewModel());
			Pages.Add(DashboardViewModel = new DashboardViewModel());
            Pages.Add(Diagnostics = new DiagnosticsViewModel());
			Pages.Add(ClearCashInViewModel = new ClearCashInViewModel());

            //Pages.Add(ClearChecksViewModel = new ClearChecksViewModel());
            Pages.Add(ClearCardsViewModel = new ClearCardsViewModel());
			//Pages.Add(DisplayCashOutViewModel = new DisplayCashOutViewModel());
			//Pages.Add(AddCashViewModel = new AddCashViewModel());
			//Pages.Add(AddCoinViewModel = new AddCoinViewModel());
			Pages.Add(StandardCashViewModel = new StandardCashViewModel());
			Pages.Add(DevicesConf = new DeviceConfigurationViewModel());
			Pages.Add(SystemParametersViewModel = new SystemParametersViewModel());
            //Pages.Add(UserRolesViewModel = new UserRolesViewModel());

            Pages.Add(ConfigurationViewModel = new ConfigurationViewModel());
			Pages.Add(CopyLogsViewModel = new CopyLogsViewModel());
			Pages.Add(ChangePasswordViewModel = new ChangePasswordViewModel());
			Pages.Add(VdmViewModel = new VdmViewModel(applicationCoordinator));
			Pages.Add(RebootViewModel = new RebootViewModel());
            Pages.Add(LoadingViewModel = new LoadingViewModel());
            Pages.Add(ConnectionLost = new ConnectionLostModel());

            SelectedPage = DashboardViewModel;
			Status = new StatusViewModel();

            LoadDevices();

			Context.DoorsOpenChanged += Context_DoorsOpenChanged;
            Context.SupervisoryStatusUpdated += Context_SupervisoryStatusUpdated;
			Context.LoginEvent += Context_LoginEvent;
			Context.LogoutEvent += Context_LogoutEvent;
            CashAcceptor.CashAcceptorStatusChanged += CashAcceptor_CashAcceptorStatusChanged;
            CashAcceptor.CashAcceptorUnitChanged += CashAcceptor_CashAcceptorUnitChanged;
            CardReader.CardReaderStatusChanged += CardReader_CardReaderStatusChanged;
            ReceiptPrinter.ReceiptPrinterDeviceStatusChanged += Printer_ReceiptPrinterDeviceStatusChanged;
            ReceiptPrinter.ReceiptPrinterMediaStatusChanged += ReceiptPrinter_ReceiptPrinterMediaStatusChanged;
            PinPad.PinPadStatusChanged += PinPad_PinPadStatusChanged;

            Load();

			auxiliaries = ServiceLocator.Instance.Resolve<IAuxiliaries>();
			auxiliaries.PowerFailure += Auxiliaries_PowerFailure;
			auxiliaries.PoweredUp += Auxiliaries_PoweredUp;

			var _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			_channelManagementService.InsertEventAsync("InService", "True");

            //Screens.OutOfServiceViewsHide();
        }

        

        private void LoadDevices()
        {
            

            Devices = new ObservableCollection<DeviceViewModel>
                {
                    new GuideLightsViewModel(),
                    new CardReaderViewModel(),
                    new CashAcceptorViewModel(),
					//new CashAcceptorViewModel(),
					//new CashDispenserViewModel(),
					//new CheckAcceptorViewModel(),
					//new EidScannerViewModel(),
					new PinPadViewModel(),
                    //new FingerPrintScannerViewModel(),
					//new SignPadViewModel(),
					new PrinterViewModel(),
                    new DoorsViewModel(),
                    new DeviceSensorsViewModel(),
                };
        }

        private void ReLoadDevices()
        {


            Devices.Clear();
            LoadDevices();

            if (selectedPage == DashboardViewModel) { 
                Context.steps.Logger.Info("DashboardViewModel.Load called1");
                SelectedPage = LoadingViewModel;

                SendWithDelay();
            }
        }
        private void CashAcceptor_CashAcceptorStatusChanged(object sender, string status)
        {
            Context.steps.Logger.Info("CashAcceptor_CashAcceptorStatusChanged called with status: " + status);
            ReLoadDevices();
        }

        private void CardReader_CardReaderStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("CardReader_CardReaderStatusChanged called with status: " + e);
            ReLoadDevices();
        }

        private void Printer_ReceiptPrinterDeviceStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("Printer_ReceiptPrinterDeviceStatusChanged called with status: " + e);
            ReLoadDevices();
        }
        private void ReceiptPrinter_ReceiptPrinterMediaStatusChanged(object sender, PrinterStatus e)
        {
            Context.steps.Logger.Info("Printer_ReceiptPrinterMediaStatusChanged called with status: " + e);
            ReLoadDevices();
        }

        private void PinPad_PinPadStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("Printer_ReceiptPrinterDeviceStatusChanged called with status: " + e);
            ReLoadDevices();
        }

        private void CashAcceptor_CashAcceptorUnitChanged(object sender, string e)
        {

            Context.steps.Logger.Info("SupervisorViewModel CashAcceptor_CashAcceptorUnitChanged Start");

            var username = Context.UserRoles?.UserName ?? "";
            //goto Login page if not logged
            if (username != "CIT")
            {
                SelectedPage = LoadingViewModel;

            SendWithDelay();
            }
        }
        async Task SendWithDelay()
        {
            await Task.Delay(1000);
            SelectedPage = DashboardViewModel;
            Context.steps.Logger.Info("SupervisorViewModel CashAcceptor_CashAcceptorUnitChanged Completed after 1 seconds");
        }

        private void Auxiliaries_PoweredUp(object sender, EventArgs e)
		{
			var _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			_channelManagementService.InsertEventAsync("PoweredUp", "True");
		}

		private void Auxiliaries_PowerFailure(object sender, EventArgs e)
		{
			var _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			_channelManagementService.InsertEventAsync("PowerFailure", "True");
		}


        private void Context_SupervisoryStatusUpdated(object sender, SensorsStatus status)
        {

            if (status == SensorsStatus.Supervisor)
            {
                var username = Context.UserRoles?.UserName ?? "";
                //goto Login page if not logged
                if (username == "CIT")
                {
                    if (Context.steps.isCitStarted) { 
                        SelectedPage = ClearCashInViewModel;
                    }
                    else
                    {
                        SelectedPage = DashboardViewModel;
                    }
                }
                else if (username == "SLM")
                {
                    SelectedPage = Diagnostics;
                    Context.steps.Logger.Info("Context_SupervisoryStatusUpdated Diagnostics");

                    //SelectedPage = DashboardViewModel;
                }
                else
                {
                    SelectedPage = LoadingViewModel;

                    SendWithDelay();
                }

            }
            else if (status == SensorsStatus.Run)
            {
                var username = Context.UserRoles?.UserName ?? "";
                if (username == "CIT" && !Context.steps.isStepFollowed)
                {
                    //SelectedPage = ClearCashInViewModel;
                    if (Context.steps.isCitStarted)
                    {
                        SelectedPage = ClearCashInViewModel;
                    }
                    else
                    {
                        SelectedPage = DashboardViewModel;
                        Context.steps.ResetSteps();
                        Screens.UpdateToMainApp(true);
                    }
                    //var clearCashInViewModel = (ClearCashInViewModel)selectedPage;

                    //if (!Context.steps.isCitStarted)
                    //{
                        //Context.steps.Logger.Info("Reset Steps Called1");
                        //Context.steps.ResetSteps();
                    //}
                }
                else
                {
                    SelectedPage = DashboardViewModel;
                }
            }
        }

        private void Context_DoorsOpenChanged(object sender, EventArgs e)
		{
            //ClearCashInViewModel.Message = "";

            if (Context.DoorsOpen)
			{
                var username = Context.UserRoles?.UserName ?? "";
                //goto Login page if not logged
                if (username == "CIT")
                {
                    SelectedPage = ClearCashInViewModel;
                }
                else
                {
                    SelectedPage = Diagnostics;
                    Context.steps.Logger.Info("Context_DoorsOpenChanged Diagnostics");

                    //SelectedPage = DashboardViewModel;
                }


            }
			else
			{//goto dashboard if all doors closed
                var username = Context.UserRoles?.UserName ?? "";
                if (username == "CIT" && !Context.steps.isStepFollowed)
                {

                    if (Context.steps.isCitStarted)
                    {
                        SelectedPage = ClearCashInViewModel;
                    }
                    else
                    {
                        //SelectedPage = DashboardViewModel;
                        Context.steps.ResetSteps();
                    }

                    /*SelectedPage = ClearCashInViewModel;
                    //var clearCashInViewModel = (ClearCashInViewModel)selectedPage;

                    if (!Context.steps.isCitStarted) { 
                    Context.steps.Logger.Info("Reset Steps Called1");
                    Context.steps.ResetSteps();
                    }*/
                }
                else
                {
                    SelectedPage = DashboardViewModel;
                }
			}
		}

		private async void Context_LoginEvent(object sender, EventArgs e)
		{
            //login page will be hidded -> goto dashboard

            var username = Context.UserRoles?.UserName ?? "";
            if (username == "CIT")
            {
                SelectedPage = ClearCashInViewModel;
                if (!Context.steps.isCitStarted)
                {
                    Context.steps.Logger.Info("Reset Steps Called2");
                    Context.steps.ResetSteps();
                }
            }
            else {
                //SelectedPage = Diagnostics;
                SelectedPage = DashboardViewModel;
            }
			

			var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
			var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			await channelService.SendDeviceStatus(devStatus.GetDevicesStatus());
		}

		private async void Context_LogoutEvent(object sender, EventArgs e)
		{
            //many pages will be hidden -> goto dashboard
            //SelectedPage = Diagnostics;
            SelectedPage = DashboardViewModel;

			var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
			var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			await channelService.SendDeviceStatus(devStatus.GetDevicesStatus());
		}

		private bool _displayProgress;
		public bool DisplayProgress
		{
			get
			{
				return _displayProgress;
			}
			set
			{
				SetProperty(ref _displayProgress, value);
			}
		}

		private readonly ICardReader CardReader = ServiceLocator.Instance.Resolve<ICardReader>();
		//private readonly IChequeAcceptor CheckAcceptor = ServiceLocator.Instance.Resolve<IChequeAcceptor>();
		private readonly ICashDispenser CashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();

		public ObservableCollection<PageViewModel> Pages { get; }
		public DiagnosticsViewModel Diagnostics;
		public DeviceConfigurationViewModel DevicesConf;
		//public LoginViewModel LoginViewModel;
		public DashboardViewModel DashboardViewModel;
        public ConfigurationViewModel ConfigurationViewModel;
		public CopyLogsViewModel CopyLogsViewModel;
		public RebootViewModel RebootViewModel;
		public VdmViewModel VdmViewModel;
		public ClearCashInViewModel ClearCashInViewModel;
		public ClearChecksViewModel ClearChecksViewModel;
		public ClearCardsViewModel ClearCardsViewModel;
		public DisplayCashOutViewModel DisplayCashOutViewModel;
		public AddCashViewModel AddCashViewModel;
		public AddCoinViewModel AddCoinViewModel;
		public ChangePasswordViewModel ChangePasswordViewModel;
		public StandardCashViewModel StandardCashViewModel;
		public SystemParametersViewModel SystemParametersViewModel;
        public LoadingViewModel LoadingViewModel;
        public ConnectionLostModel ConnectionLost;
        //public UserRolesViewModel UserRolesViewModel;

        private PageViewModel selectedPage;
		public PageViewModel SelectedPage
		{
			get
			{
				return selectedPage;
			}
			set
			{
				if (SetProperty(ref selectedPage, value))
					SelectedPage.Load();

                RaisePropertyChanged("SelectedPage");
			}
		}

		public DeviceViewModel SelectedDevice
		{
			get
			{
				return Diagnostics.SelectedDevice;
			}
			set
			{
				/*if (!Context.IsLoggedInMode)
					return;

                var username = Context.UserRoles?.UserName ?? "";
                if (username != "CIT")
                {
                    SelectedPage = Diagnostics;
                    Diagnostics.SelectedDevice = value;
                }*/
                
            }
        
		}

		private ObservableCollection<DeviceViewModel> _devices;
		public ObservableCollection<DeviceViewModel> Devices
		{
			get
			{
				return _devices;
			}
			private set
			{
				SetProperty(ref _devices, value);
			}
		}

		public StatusViewModel Status { get; }
		public CashStatusViewModel CashStatus { get; } = new CashStatusViewModel();
		public MachineDateTimeViewModel MachineDateTime { get; } = new MachineDateTimeViewModel();

		public sealed override void Load()
		{
			foreach (var page in Pages)
			{
				page.Load();
			}
		}
	}
}