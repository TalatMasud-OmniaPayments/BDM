using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using System;
using System.Windows.Controls;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Services.Interface;
using System.Configuration;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class ClearCashInViewModel : PageViewModel
	{
        //public override bool IsEnabled => Context.IsLoggedInMode;
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.ClearCashIn == true ? true : false);

        public SupervisorService supervisorService { get; } = ServiceLocator.Instance.Resolve<SupervisorService>();

        private readonly IDeviceSensors _deviceSensors = ServiceLocator.Instance.Resolve<IDeviceSensors>();
        private readonly IDoors _doors = ServiceLocator.Instance.Resolve<IDoors>();
        private readonly ICashAcceptor _cashAcceptor = ServiceLocator.Instance.Resolve<ICashAcceptor>();
        private readonly ICashDispenser _cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
        private readonly IReceiptPrinter _receiptPrinter = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
        //public CITSteps steps = new CITSteps();
        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());
        private MediaUnitViewModel[] oldCassettes;

        public ClearCashInViewModel()
        {

            
            //logger.Info("ClearCashInViewModel Initialized");
            /*_doors.DoorsStatusChanged -= Doors_DoorsStatusChanged;
            _doors.ShieldStatusChanged -= Doors_ShieldStatusChangedAsync;
            _doors.SafeStatusChanged -= Doors_SafeStatusChanged;*/
            //_cashDispenser.MediaChanged += (s, e) => Load();

            _doors.DoorsStatusChanged += Doors_DoorsStatusChanged;
            //_doors.ShieldStatusChanged += Doors_ShieldStatusChangedAsync;
            _doors.SafeStatusChanged += Doors_SafeStatusChangedAsync;
            _receiptPrinter.ReceiptPrinterMediaStatusChanged += ReceiptPrinter_ReceiptPrinterMediaStatusChanged;
            _cashAcceptor.MediaChanged += (s, e) => Load();
            Logout = new DelegateCommand(() => { 

                Console.WriteLine("Logout user");

                LogoutCIT();
            });
            Proceed = new DelegateCommand(() => {

                Console.WriteLine("Procedd user");

                if (Context.steps.isStepFollowed)
                {
                    Message = "Replenishment successful, You can logout now.";
                    // Go to main menu.
                }
                else {
                    Message = "To proceed please follow the above steps.";
                }
            });
            Clear = new DelegateCommand<MediaUnitViewModel>(
				async x =>
				{
					Context.DisplayProgress = true;

                    Logger.Info("Start Exchange 1");
                    _mediaInfo = _cashAcceptor.GetMediaInfo();

                    Cassettes = _mediaInfo.
                        Where(i => i.Value == -1).
                        Select(i => new MediaUnitViewModel { Model = i }).ToArray();
                    Logger.Info("Start Exchange 2");
                    try
					{
						var receipt = new ClearCashInReceipt
						{
							IsView = false,
							CashInUnits = Cassettes.
								Where(i => x == null || i.Model.Id == x.Model.Id).
								Select(
									i => new CashInUnit
									{
										Name = i.Model.Type,
										Currency = i.Model.Currency,
										Count = i.Model.TotalCount
									}
							).ToList(),
							DenominationRecords = x == null ?
								DetailedCassettes.
									Select(
										d => new DenominationRecord
										{
											Value = d.Model.Value,
											Count = d.Model.Count,
											Retracted = d.Model.RetractedCount,
											Rejected = d.Model.RejectedCount,
											Total = d.Model.TotalCount
										}
								).ToList() :
								_mediaInfo.
									Where(i => i.Id == x.Model.Id && i.Value > 0).
									Select(
										d => new DenominationRecord
										{
											Value = d.Value,
											Count = d.Count,
											Retracted = d.RetractedCount,
											Rejected = d.RejectedCount,
											Total = d.TotalCount
										}
								).ToList()
                               
                    };
                        Logger.Info("Start Exchange 3");
                        _cashAcceptor.SetMediaInfo(x != null ? new[] { x.Model.Id } : null, null);
                        _cashAcceptor.isCassettteChanged = false;
                        //await PrintAsync(receipt);
                        Logger.Info("Start Exchange 4");
                        Load();
                        Logger.Info("Start Exchange 5");
                        //await PrintCassettesCount();

                        //
                    }
					finally
					{
						Context.DisplayProgress = false;
                        //
                        try
                        {
                            await _channelManagementService.InsertEventAsync("CIT: Clear Cash", "True");// needs to uncomment once the service starts working
                        }
                        catch (Exception ex)
                        {
                            Logger.Exception(ex);

                        }

                    }
                }, x => x.Model.TotalCount > 0);
            ExchangeCassettes = new DelegateCommand(
                () => Clear.Execute(null)
            );

        }

        private void ReceiptPrinter_ReceiptPrinterMediaStatusChanged(object sender, PrinterStatus e)
        {

            


            Context.steps.isReceiptPaperAvailable = false;

            if (e == PrinterStatus.Present)
            {
                //_channelManagementService.InsertEventAsync("RECEIPT_PRINTER_PEPPER_PRESENT", "True");
                Context.steps.isReceiptPaperAvailable = true;
                Message = "";
                if (Context.steps.shouldAutoValidateAndProcessCit)
                {
                    Logger.Info("ValidateAndProcessCITSteps from ReceiptPrinter_ReceiptPrinterMediaStatusChanged");
                    
                    ValidateAndProcessCITSteps();
                }
               
            }
            //else
            //{
                //_channelManagementService.InsertEventAsync("RECEIPT_PRINTER_PEPPER_PRESENT", "True");
            //}
        }

        void UpdateOldCassettes()
        {
            _mediaInfo = _cashAcceptor.GetMediaInfo();

            oldCassettes = _mediaInfo.
                Where(i => i.Value == -1).
                Select(i => new MediaUnitViewModel { Model = i }).ToArray();
            
        }

        void UpdateNewCassettes()
        {
            _mediaInfo = _cashAcceptor.GetMediaInfo();

            Cassettes = _mediaInfo.
                Where(i => i.Value == -1).
                Select(i => new MediaUnitViewModel { Model = i }).ToArray();

        }
        async Task PrintCassettesCount(MediaUnitViewModel[] printCassette)
        {
            /*_mediaInfo = _cashAcceptor.GetMediaInfo();

            Cassettes = _mediaInfo.
                Where(i => i.Value == -1).
                Select(i => new MediaUnitViewModel { Model = i }).ToArray();*/

            Context.DisplayProgress = true;
            /*Load();*/
            try
            {
                await PrintAsync(new ClearCashInReceipt
                {
                    IsView = true,
                    CashInUnits = printCassette.
                        Select(
                            i => new CashInUnit
                            {
                                Name = i.Model.Type,
                                Currency = i.Model.Currency,
                                Count = i.Model.TotalCount
                            }
                    ).ToList(),
                    DenominationRecords = DetailedCassettes.
                                    Select(
                                        d => new DenominationRecord
                                        {
                                            Value = d.Model.Value,
                                            Count = d.Model.Count,
                                            Retracted = d.Model.RetractedCount,
                                            Rejected = d.Model.RejectedCount,
                                            Total = d.Model.TotalCount
                                        }
                                ).ToList()
                });
            }
            finally
            {
                Context.DisplayProgress = false;
                try {
                    await _channelManagementService.InsertEventAsync("CIT: Clear Cash", "True");// needs to uncomment once the service starts working
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);

                }
            }
        }

        async Task PrintAsync<T>(T receipt)
		{
			var formattedReceipt = await receiptFormatter?.FormatAsync(receipt);

			await Printer?.PrintAndEjectAsync(formattedReceipt);
			await _journal?.WriteReceiptAsync(receiptFormatter, receipt);
		}

		public override void Load()
		{

            _mediaInfo = _cashAcceptor.GetMediaInfo();

            Cassettes = _mediaInfo.
                Where(i => i.Value == -1).
                Select(i => new MediaUnitViewModel { Model = i }).ToArray();

            foreach (var cst in Cassettes)
            {
                Console.WriteLine("Cst");
            }

            TotalAmount = 0;

            DetailedCassettes = _mediaInfo.
                Where(i => i.Value != -1).
                GroupBy(i => new { i.Currency, i.Value }, (key, group) =>
                {
                    var mediaUnits = group as MediaUnit[] ?? group.ToArray();
                    var result = new MediaUnit
                    {
                        Currency = key.Currency,
                        Value = key.Value,
                        RetractedCount = mediaUnits.Sum(g => g.RetractedCount),
                        RetractTitle = "Retract: " + mediaUnits.Sum(g => g.RetractedCount),
                        RejectedCount = mediaUnits.Sum(g => g.RejectedCount),
                        RejectTitle = "Reject: " + mediaUnits.Sum(g => g.RejectedCount),
                        Count = mediaUnits.Sum(g => g.Count),
                        CountTitle = "Count: " + mediaUnits.Sum(g => g.Count),

                        TotalCount = mediaUnits.Sum(g => g.TotalCount),
                        TotalTitle = "Total: " + mediaUnits.Sum(g => g.TotalCount),
                    };
                    TotalAmount += result.Value * result.TotalCount;
                    return result;
                }).
                Select(i => new MediaUnitViewModel { Model = i }).ToArray();
        }

        
        private readonly IReceiptPrinter Printer = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
		readonly IReceiptFormatter receiptFormatter = ServiceLocator.Instance.Resolve<IReceiptFormatter>();
		
		public ICommand Logout { get; }
        public ICommand Clear { get; }
        public ICommand ExchangeCassettes { get; }
        public ICommand Proceed { get; }
		public ICommand Print { get; }

		private MediaUnit[] _mediaInfo;
		private MediaUnitViewModel[] _cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return _cassettes; }
			set { SetProperty(ref _cassettes, value); }
		}

		private MediaUnitViewModel[] _detailedCassettes;
		public MediaUnitViewModel[] DetailedCassettes
		{
			get { return _detailedCassettes; }
			set { SetProperty(ref _detailedCassettes, value); }
		}


		private int _totalAmount;
		public int TotalAmount
		{
			get { return _totalAmount; }
			set { SetProperty(ref _totalAmount, value); }
		}

        private string _message;
        public string Message
        {
            get { return _message; }
            set {
                _message = value;
                //RaisePropertyChanged(nameof(Message));
                RaisePropertyChanged("Message");
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }


        private void Doors_DoorsStatusChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Doors changed event");
            /*var openDoors = _doors.AllDoors.ToList();
            foreach (var item in openDoors)
            {

                if (item.Id == "CabinetDoor")
                {
                    if (item.Status == DoorStatus.Open)
                    {
                        var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
                        var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
                        channelService.SendDeviceStatus(devStatus.GetDevicesStatus(), true, false);
                         //_channelManagementService.InsertEventAsync("CIT: Clear Cash", "Steps followed");
                    }
                }
            }*/
        }

        private async void Doors_SafeStatusChangedAsync(object sender, EventArgs e)
        {

            var isClearCashIn = Context.UserRoles?.ClearCashIn ?? false;

            if (isClearCashIn)
            {
                var openDoors = _doors.AllDoors.ToList();

                foreach (var item in openDoors)
                {

                    if (item.Id == "SafeDoor")
                    {
                        if (item.Status == DoorStatus.Open)
                        {

                            if (_receiptPrinter.GetPrinterStatus() != PrinterStatus.Present)
                            {
                                Message = "Receipt peper not present, Please check the receipt printer and reload the papers.";
                                Context.steps.isReceiptPaperAvailable = false;
                            }
                            //Console.WriteLine("Shield Open");
                            //if (Context.steps.isShieldOpened == false && Context.steps.isSafeDoorOpened == false && Context.steps.isCassettesRemoved == false && Context.steps.isCassettesReplaced == false && Context.steps.isSafedoorClosed == true && Context.steps.isSafedoorLocked == true) {
                            if (Context.steps.isSafeDoorOpened == false)
                            {

                                UpdateOldCassettes();
                                Context.steps.isSafeDoorOpened = true;
                                _cashAcceptor.isCassettteChanged = false;
                                Context.steps.isCitStarted = true;
                                Context.steps.shouldAutoValidateAndProcessCit = false;
                                Status = "safedoor open: valid";
                                Logger.Info(Status);
                                Console.WriteLine(Status);

                            }
                            else
                            {
                                //invalid step
                                Status = "safedoor open: invalid";
                                Logger.Info(Status);
                                Console.WriteLine(Status);
                                Context.steps.isStepFollowed = false;
                                try
                                {
                                    //await _channelManagementService.InsertEventAsync("CIT: Clear Cash", "Step not followed");// needs to uncomment once the service starts working
                                    await _channelManagementService.InsertEventAsync("CIT_CLEAR_CASH_STEP_NOT_FOLLOWED", "True");
                                }
                                catch (Exception ex)
                                {
                                    Logger.Exception(ex);

                                }
                            }

                        }

                        if (item.Status == DoorStatus.Closed)
                        {

                            if (_receiptPrinter.GetPrinterStatus() != PrinterStatus.Present)
                            {
                                Message = "Receipt paper not present, Please check the receipt printer and reload the papers.";
                                Context.steps.isReceiptPaperAvailable = false;
                                Context.steps.shouldAutoValidateAndProcessCit = true;
                            }
                            else
                            {
                                if (Context.steps.isSafeDoorOpened == true) { 
                                    Logger.Info("ValidateAndProcessCITSteps from Doors_SafeStatusChangedAsync");
                                    ValidateAndProcessCITSteps();
                                }
                                else
                                {
                                    Message = "Steps have not followed properly. Please close all the doors and replenish the cassettes again.";

                                }
                            }

                        }
                    }


                }
            }
        }

        private async void ValidateAndProcessCITSteps()
        {
            
            Context.steps.shouldAutoValidateAndProcessCit = false;
            //if ()
            //var cessetteStatus = _cashAcceptor.GetCashAcceptorStatus();
            //_cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
            //_cashDispenser.MediaChanged += (s, e) => Load();
            
            Context.DisplayProgress = true;
            //_cashDispenser.Dispose();
            //CashDispenser.Dispose();

            //CashAcceptor.Initialize();
            try
            {
                Logger.Info("_cashAcceptor.Status2:" + _cashAcceptor.Status);
                //var cessetteStatus2 = _cashDispenser.GetCashDispenserStatus();
                //Logger.Info("GetCashDispenserStatus2:" + cessetteStatus2);

                await _cashAcceptor.ResetAsync();
                await _cashDispenser.ResetAsync();
                var cessetteStatus = _cashDispenser.GetDispensableCassettesStatus();
                //_cashDispenser.CheckCassetteStatus();
                //var cessettes = _cashAcceptor.GetMediaInfo();
                //_cashDispenser.CheckCassetteStatus();
                var cessetteStatus1 = _cashAcceptor.GetCashAcceptorStatus(cessetteStatus);
                _cashAcceptor.CheckCassetteStatus();
                var unchangedCst = _cashAcceptor.GetUnchangedCassettes(Context.steps.oldCassettes);
                Logger.Info("GetCashDispenserStatusCount:" + cessetteStatus.Count());
                Logger.Info("_cashAcceptor.Status:" + _cashAcceptor.Status);
                Logger.Info("isCassettteChanged:" + _cashAcceptor.isCassettteChanged);
                Logger.Info("isSafeDoorOpened:" + Context.steps.isSafeDoorOpened);
                Logger.Info("UnchangedCassetes:" + unchangedCst);
                //unchangedCst = "";
                var mediaStatus = _receiptPrinter.GetReceiptPaperStatus();

                if ((cessetteStatus.Count == 0 || cessetteStatus1 == CashAcceptorCassetteStatus.FATAL) && _cashAcceptor.isCassettteChanged == true && unchangedCst.Length == 0 && mediaStatus == "PRESENT")
                //if (cessetteStatus == CashDispenserCassetteStatus.EMPTY && _cashAcceptor.Status == DeviceStatus.Online && _cashAcceptor.isCassettteChanged == true && Context.steps.isSafeDoorOpened == true)
                //if (Context.steps.isSafeDoorOpened == true)

                {
                    Context.steps.isSafeDoorOpened = false;


                    Context.steps.isStepFollowed = true;
                    Logger.Info("Cassettes status before clear cash.");
                    //_cashDispenser.CheckCassetteStatus();

                    await PrintCassettesCount(oldCassettes);


                    try
                    {
                        var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
                        var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
                        await channelService.SendDeviceStatus(devStatus.GetDevicesStatus(), true, false);
                        //await _channelManagementService.InsertEventAsync("CIT:", "Steps followed");
                        await _channelManagementService.InsertEventAsync("CIT_CLEAR_CASH_STEP_FOLLOWED", "TRUE");

                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(ex);

                    }
                    finally
                    {
                        Clear.Execute(null);
                        Logger.Info("Cassettes status after clear cash.");
                        UpdateNewCassettes();
                        await PrintCassettesCount(Cassettes);

                        LogoutCIT();


                        await _channelManagementService.InsertEventAsync("InService", "True");
                        Context.steps.ResetSteps();
                        Status = "safedoor close: valid";
                        Logger.Info(Status);
                        Console.WriteLine(Status);
                        Context.DisplayProgress = false;
                    }

                    // needs to uncomment once the service starts working

                    //await PrintCassettesCount();
                    //Context.steps.ResetSteps();

                    //Load();
                    //await PrintCassettesCount();



                }
                else
                {
                    //invalid step
                    Context.DisplayProgress = false;
                    Status = "safedoor close: invalid";
                    Logger.Info(Status);
                    Console.WriteLine(Status);

                    Context.steps.isStepFollowed = false;
                    Message = "Steps have not followed properly. Please close all the doors and replenish the cassettes again.";

                    if (_cashAcceptor.Status != DeviceStatus.Online)
                    {
                        Message = "Steps have not followed properly. Please insert the empty cassettes and check the Cash acceptor.";
                    }
                    else if (cessetteStatus.Count > 0)
                    {
                        var cassette = cessetteStatus[0];

                        if (cassette.status.ToUpper() == "MISSING")
                        {
                            Message = "Steps have not followed properly. Some of the cassettes are missing";

                        }
                        else
                        {
                            Message = "Steps have not followed properly. All the cassettes should have to be empty and installed properly.";
                        }
                    }

                    else if (unchangedCst.Length > 0)
                    {
                        //Message = "Please replace all the previous cassettes. The Cassette(s)" + unchangedCst + "needs to be replaced.";
                        Message = "Please replace all the old cassettes with new empty cassettes and try again.";

                    }
                    else if (mediaStatus != "PRESENT")
                    {
                        //Message = "Please replace all the previous cassettes. The Cassette(s)" + unchangedCst + "needs to be replaced.";
                        Message = "Receipt peper not present, Please check the receipt printer and reload the papers.";
                        Context.steps.isReceiptPaperAvailable = false;
                        Context.steps.shouldAutoValidateAndProcessCit = true;

                    }
                    else
                    {
                        Message = "Steps have not followed properly. Please insert the empty cassettes";
                    }

                    try
                    {
                        await _channelManagementService.InsertEventAsync("CIT_CLEAR_CASH_STEP_NOT_FOLLOWED", "True");// needs to uncomment once the service starts working

                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(ex);

                    }

                }

            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                Message = "Unable to reset the BRM. Please check the cassettes and try replenishment again.";
            }
            finally
            {
               
                Context.DisplayProgress = false;
            }
        }


        public void LogoutCIT()
        {
            supervisorService.Logout();
            var opStatus = _deviceSensors.GetOperatorStatus();

            if (opStatus != SensorsStatus.Supervisor)
            {
                Screens.OutOfServiceViewsHide();
                Screens.UpdateToMainApp(true);
            }
        }
    }
}