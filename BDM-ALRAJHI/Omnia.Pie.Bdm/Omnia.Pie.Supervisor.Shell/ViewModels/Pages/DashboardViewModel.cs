using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Framework.Interface;
using System.Windows.Input;
using Omnia.Pie.Vtm.DataAccess.Interface;
using System.Configuration;
using System;
using System.Timers;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class DashboardViewModel : PageViewModel
	{
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Dashboard == true ? true : false);

        private readonly ICardReader CardReader = ServiceLocator.Instance.Resolve<ICardReader>();
		private readonly ICashDispenser CashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
		private readonly ICashAcceptor CashAcceptor = ServiceLocator.Instance.Resolve<ICashAcceptor>();
		private readonly IReceiptPrinter ReceiptPrinter = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
		private readonly IJournalPrinter JournalPrinter = ServiceLocator.Instance.Resolve<IJournalPrinter>();
        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());

        public DashboardConfiguration CardData { get; set; }
		public DashboardConfiguration CheckData { get; set; }

        public ICommand Clear { get; }
        //public ICommand ExchangeCassettes { get; }
        //private MediaUnit[] _mediaInfo;

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                //RaisePropertyChanged(nameof(Message));
                RaisePropertyChanged("Message");
            }
        }

        private int _totalAmount;
        public int TotalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
        }

        private MediaUnitViewModel[] _detailedCassettes;
        public MediaUnitViewModel[] DetailedCassettes
        {
            get { return _detailedCassettes; }
            set { SetProperty(ref _detailedCassettes, value); }
        }

        public string ReceiptPaperStatus { get; set; }
		public string JournalPaperStatus { get; set; }


        public DashboardConfiguration _cashDispenserData;
        public DashboardConfiguration CashDispenserData
        {
            get { return _cashDispenserData; }
            set
            {
                SetProperty(ref _cashDispenserData, value);
                RaisePropertyChanged(nameof(CashDispenserData));
                //Logger.Info("CashDispenserData Set called");


            }
        }

        public DashboardConfiguration _cashAcceptorData;
        public DashboardConfiguration CashAcceptorData
        {
            get { return _cashAcceptorData; }
            set
            {
                SetProperty(ref _cashAcceptorData, value);
                //RaisePropertyChanged(nameof(CashAcceptorData));
                RaisePropertyChanged("CashAcceptorData");
                //Logger.Info("CashAcceptorData Set called");
            }
        }

        public DashboardViewModel()
        {
            //Logger.Info("DashboardViewModel constructor called1");
            //CashAcceptor.CashAcceptorUnitChanged += CashAcceptor_CashAcceptorUnitChanged;

            //CashAcceptor.CashAcceptorStatusChanged += CashAcceptor_CashAcceptorStatusChanged;
            //CashAcceptor.CashAcceptorUnitChanged += CashAcceptor_CashAcceptorUnitChanged;
            //CardReader.CardReaderStatusChanged += CardReader_CardReaderStatusChanged;
            //ReceiptPrinter.ReceiptPrinterDeviceStatusChanged += Printer_ReceiptPrinterDeviceStatusChanged;
            
            /*ExchangeCassettes = new DelegateCommand(async () => {
                await CashAcceptor.ResetAsync();
                CashAcceptor.PerformExchange();
            });*/
        }



        private DeviceData[] GetCashDispenserMediaInfo()
		{
			List<DeviceData> CashUnitsInfo = new List<DeviceData>();
			foreach (var item in CashDispenser.GetMediaInfo())
				if (item.Type == "BILLCASSETTE")
					CashUnitsInfo.Add(new DeviceData()
					{
						Title = "Cash",
						CurrentCount = item.InitialCount,
						MaxCount = item.MaxCount
					});
			return CashUnitsInfo.ToArray<DeviceData>();
		}
        /*
		private DeviceData[] GetCashAcceptorMediaInfo()
		{
			List<DeviceData> CashUnitsInfo = new List<DeviceData>();

            foreach (var cassette in CashAcceptor.GetMediaInfo().GroupBy(x => x.UnitId))
            {
                int currentCount = 0;
				int maxCount = 0;
				string title = "";
                string status = "";

                //CashAcceptor.GetCashAcceptorStatus();

                foreach (var itemchild in cassette)
				{
                    //Logger.Info("Start group");
                    //Logger.Info("Type: " + itemchild.Type + " itemchild.Value: " + itemchild.Value + " itemchild.UNitId: " + itemchild.UnitId + "Status: " + itemchild.Status + "currentCount: " + itemchild.InitialCount + "MaxCount: " + itemchild.MaxCount + "Count: " + itemchild.Count + "TotalCount: " + itemchild.TotalCount);
                    if (itemchild.Value != -1 || (itemchild.Value == -1 && itemchild.UnitId == "LCU00") ) {
                        //Logger.Info("Counted");
                        currentCount += itemchild.InitialCount;
					maxCount += itemchild.TotalCount;
					title = itemchild.Type;
                    status = itemchild.Status;
                    }
                }

                if (title == "REJECT")
                {
                    CashUnitsInfo.Insert(0, new DeviceData()
                    {
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }
                else
                {
                    CashUnitsInfo.Add(new DeviceData()
                    {
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }
				
			}
			return CashUnitsInfo.ToArray<DeviceData>();
		}
        */
        private DeviceData[] GetCashAcceptorMediaInfo()
        {
            List<DeviceData> CashUnitsInfo = new List<DeviceData>();
            Logger.Info("GetMediaInfoLength: " + CashAcceptor.GetMediaInfo().Length);
            Logger.Info("GetMediaInfoGroupByLength: " + CashAcceptor.GetMediaInfo().GroupBy(x => x.Id).Count());

            //foreach (var item in CashAcceptor.GetMediaInfo().GroupBy(x => x.Id))
            //{
            var cstByUnitId = CashAcceptor.GetMediaInfo().GroupBy(x => x.UnitId);

            foreach (var cassette in cstByUnitId)
            {

                int currentCount = 0;
                int maxCount = 0;
                string unitId = "";
                string title = "";
                string status = "";

                //CashAcceptor.GetCashAcceptorStatus();

                foreach (var itemchild in cassette)         // Getting the subcassette with any -1 value
                {
                    //Logger.Info("Start group");
                    Logger.Info("Type1: " + itemchild.Type + " itemchild.Value1: " + itemchild.Value + " itemchild.UNitId1: " + itemchild.UnitId + "Status1: " + itemchild.Status + "currentCount1: " + itemchild.InitialCount + "MaxCount1: " + itemchild.MaxCount + "Count1: " + itemchild.Count + "TotalCount1: " + itemchild.TotalCount);
                    if (itemchild.Value == -1)
                    {
                        Logger.Info("Counted1");


                        currentCount = 0;
                        maxCount = 0;
                        unitId = itemchild.UnitId;
                        title = itemchild.Type;
                        status = itemchild.Status;
                    }
                }

                /*
                 foreach (var itemchild in cassette)
				{
                    //Logger.Info("Start group");
                    Logger.Info("Type: " + itemchild.Type + " itemchild.Value: " + itemchild.Value + " itemchild.UNitId: " + itemchild.UnitId + "Status: " + itemchild.Status + "currentCount: " + itemchild.InitialCount + "MaxCount: " + itemchild.MaxCount + "Count: " + itemchild.Count + "TotalCount: " + itemchild.TotalCount);
                    if (itemchild.Value != -1 || (itemchild.Value == -1 && itemchild.UnitId == "LCU00") ) {
                        Logger.Info("Counted");
                        currentCount += itemchild.InitialCount;
					maxCount += itemchild.TotalCount;
					title = itemchild.Type;
                    status = itemchild.Status;
                    }
                }
                */

                if (title == "REJECT")
                {
                    CashUnitsInfo.Insert(0, new DeviceData()
                    {
                        UnitId = unitId,
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }
                else
                {
                    CashUnitsInfo.Add(new DeviceData()
                    {
                        UnitId = unitId,
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }

            }

            foreach (var cassette in cstByUnitId)
            {

                int currentCount = 0;
                int maxCount = 0;
                string unitId = "";
                string title = "";
                string status = "";

                //CashAcceptor.GetCashAcceptorStatus();


                foreach (var itemchild in cassette)
                {
                    Logger.Info("Type2: " + itemchild.Type + " itemchild.Value2: " + itemchild.Value + " itemchild.UNitId2: " + itemchild.UnitId + "Status2: " + itemchild.Status + "currentCount2: " + itemchild.InitialCount + "MaxCount2: " + itemchild.MaxCount + "Count2: " + itemchild.Count + "TotalCount2: " + itemchild.TotalCount);

                    if (itemchild.Value != -1 || (itemchild.Value == -1 && itemchild.Type == "LCU00"))
                    {
                        Logger.Info("Counted2");
                        currentCount += itemchild.InitialCount;
                        maxCount += itemchild.TotalCount;
                        unitId = itemchild.UnitId;
                    }
                }


                foreach (var unitInfo in CashUnitsInfo)
                {

                    if (unitId == unitInfo.UnitId)
                    {
                        unitInfo.CurrentCount = currentCount;
                        unitInfo.MaxCount = maxCount;
                    }
                }

                /*if (title == "REJECT")
                {
                    CashUnitsInfo.Insert(0, new DeviceData()
                    {
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }
                else
                {
                    CashUnitsInfo.Add(new DeviceData()
                    {
                        Title = title,
                        CurrentCount = currentCount,
                        MaxCount = maxCount,
                        Status = status
                    });
                }*/

            }

            return CashUnitsInfo.ToArray<DeviceData>();
        }
        public override void Load()
		{


            Context.steps.Logger.Info("DashboardViewModel.Load called2");

            ReceiptPaperStatus = ReceiptPrinter.GetPaperStatus();
            JournalPaperStatus = JournalPrinter.GetPaperStatus();
            //Context.DoorsOpen = true;
            //Context.Login(true, "Supervisor");
            CardData = new DashboardConfiguration()
            {
                DeviceTitle = "Card Reader",
                Status = CardReader.Status.ToString().ToUpper(),
                DeviceData = new[]
                {
                    new DeviceData()
                    {
                        Title = "Retain",
                        CurrentCount = CardReader.GetRetainCount(),
                        MaxCount = CardReader.GetRetainCount(),
                        Status = ""
					}
				}
			};

            Context.steps.Logger.Info("DashboardViewModel.Load called3");
            /*
			CheckData = new DashboardConfiguration()
			{
				DeviceTitle = "Check Acceptor",
				Status = CheckAcceptor.Status.ToString().ToUpper(),
				DeviceData = new[]
				{
					new DeviceData()
					{
						Title = "Media Bin",
						CurrentCount = CheckAcceptor.GetMediaCount("2", OperationType.CurrentCount),
						MaxCount = CheckAcceptor.GetMediaCount("2", OperationType.MaxCount),
					},
					new DeviceData()
					{
						Title = "Retract Bin",
						CurrentCount = CheckAcceptor.GetMediaCount("1", OperationType.CurrentCount),
						MaxCount = CheckAcceptor.GetMediaCount("1", OperationType.MaxCount),
					}
				}
			};
            */
            //Logger.Info("Loading CashAcceptorData");
            CashAcceptorData = new DashboardConfiguration()
			{
				DeviceTitle = "Cash Acceptor",
				Status = CashAcceptor.Status.ToString().ToUpper(),
				DeviceData = GetCashAcceptorMediaInfo()
			};
            
			CashDispenserData = new DashboardConfiguration()
			{
				DeviceTitle = "Cash Dispenser",
				Status = CashDispenser.Status.ToString().ToUpper(),
				DeviceData = GetCashDispenserMediaInfo()
			};

            Context.steps.Logger.Info("DashboardViewModel.Load called4");

            if (CashAcceptor.isCassettteChanged)
            {
                Message = "Cassette status update is required. Please Reset it in Device Configurations";
            }
            else
            {
                Message = "";
            }

            /*foreach (var itemchild in CashAcceptorData.DeviceData)
            {
                //Logger.Info("Start group");
                Logger.Info("DeviceData.Status: " + itemchild.Status + " DeviceData.Title: " + itemchild.Title );
                
            }*/
        }


        /*private void ReLoadDevices()
        {


            Load();
        }
        private void CashAcceptor_CashAcceptorStatusChanged(object sender, string status)
        {
            Context.steps.Logger.Info("CashAcceptor_Dashboard called with status: " + status);
            ReLoadDevices();
        }

        private void CardReader_CardReaderStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("CashAcceptor_Dashboard called with status: " + e);
            ReLoadDevices();
        }

        private void Printer_ReceiptPrinterDeviceStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("CashAcceptor_Dashboard called with status: " + e);
            ReLoadDevices();
        }*/

    }
}
