namespace Omnia.Pie.Supervisor.Shell.Service
{
	using Microsoft.Practices.Unity;
    using Newtonsoft.Json;
    using Omnia.Pie.Client.Journal.Interface;
    using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities.ChannelManagement;
	using System;
	using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
	using System.Windows.Threading;
	using CMEntities = Vtm.Services.Interface.Entities.ChannelManagement;
    //using Newtonsoft.Json;

    public class ChannelManagementDeviceStatusService
	{
		private readonly DispatcherTimer _timer;

        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());

        private static IJournal _journal;
        private static IJournal Journal => _journal ?? (_journal = ServiceLocator.Instance.Resolve<IJournal>());

        public ChannelManagementDeviceStatusService()
		{

            var time = ConfigurationManager.AppSettings["MonitoringTime"].ToString();
            int synchTime;

            if (!Int32.TryParse(time, out synchTime))
            {
                synchTime = 5;
            }

            _timer = new DispatcherTimer(DispatcherPriority.Background)
			{
				Interval = new TimeSpan(0, synchTime, 0)
			};

			_timer.Tick += _timer_Tick;
			_timer.Start();
		}

		private async void _timer_Tick(object sender, EventArgs e)
		{
			var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
			await channelService.SendDeviceStatus(GetDevicesStatus());
		}

		public List<CMEntities.DeviceStatus> GetDevicesStatus()
		{
			var CardReader = ServiceLocator.Instance.Resolve<ICardReader>();
			var CashAcceptor = ServiceLocator.Instance.Resolve<ICashAcceptor>();
			//var CashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
			//var CoinDispenser = ServiceLocator.Instance.Resolve<ICoinDispenser>();
			//var ChequeAcceptor = ServiceLocator.Instance.Resolve<IChequeAcceptor>();
			//var EmiratesIdScanner = ServiceLocator.Instance.Resolve<IEmiratesIdScanner>();
			var PinPad = ServiceLocator.Instance.Resolve<IPinPad>();
			var JournalPrinter = ServiceLocator.Instance.Resolve<IJournalPrinter>();
			var ReceiptPrinter = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
			//var StatementPrinter = ServiceLocator.Instance.Resolve<IStatementPrinter>();
			//var SignpadScanner = ServiceLocator.Instance.Resolve<ISignpadScanner>();
            var FingerprintScanner = ServiceLocator.Instance.Resolve<IFingerPrintScanner>();
            var errorStore = ServiceLocator.Instance.Resolve<IDeviceErrorStore>();

			var status = new List<CMEntities.DeviceStatus>
			{
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(CardReader),
					OperationalStatus = new OperationalStatus()
					{
						Cassettes = new List<Cassette>()
						{
							new Cassette()
							{
								Name = "Retain",
								Count = CardReader.GetRetainCount().ToString()
							}
						}
					},
					ErrorCode = CardReader.GetDeviceError(DeviceShortName.CRD)?.Code,
					Status = CardReader.Status.ToString(),
				},
				new CMEntities.DeviceStatus()
				{
                    
                    DeviceName = nameof(CashAcceptor),
					OperationalStatus = new OperationalStatus()
					{
						Cassettes = (
										from cst
										in CashAcceptor.GetCessettes()
										select new Cassette()
										{
											Count = cst.Count.ToString(),
											Name = cst.Index.ToString(),
											Type = cst.Type
                                        }
									).ToList()
					},
					ErrorCode = CashAcceptor.GetDeviceError(DeviceShortName.BRM)?.Code,
					Status = CashAcceptor.Status.ToString(),

				},
                
				/*{
					DeviceName = nameof(CashDispenser),
					OperationalStatus = new OperationalStatus()
					{
						Cassettes = (
										from cst
										in CashDispenser.GetCessettes()
										select new Cassette()
										{
											Count = cst.Count.ToString(),
											Name = cst.Index.ToString(),
											Type = ""
										}
									).ToList()
					},
					ErrorCode = CashDispenser.GetDeviceError(DeviceShortName.CDM)?.Code,
					Status = CashDispenser.Status.ToString(),
				},
				new CMEntities.DeviceStatus(){
					DeviceName = nameof(CoinDispenser),
					OperationalStatus = new OperationalStatus()
					{
						Cassettes = (from cst in CoinDispenser.GetCessettes()
									 select new Cassette()
									 {
										 Count = cst.Count.ToString(),
										 Name = cst.Index.ToString(),
										 Type = ""
									 }).ToList()
					},
					ErrorCode = CoinDispenser.GetDeviceError(DeviceShortName.COD)?.Code,
					Status = CoinDispenser.Status.ToString(),
				},
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(ChequeAcceptor),
					OperationalStatus = new OperationalStatus()
					{
						Cassettes = new List<Cassette>() {new Cassette(){ Count =  ChequeAcceptor.GetMediaCount(OperationType.CurrentCount).ToString(), Name = "", Type = "" } }
					},
					ErrorCode = ChequeAcceptor.GetDeviceError(DeviceShortName.CCIM)?.Code,
					Status = ChequeAcceptor.Status.ToString(),
				},
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(EmiratesIdScanner),
					OperationalStatus = new OperationalStatus()
					{
					},
					ErrorCode = EmiratesIdScanner.GetDeviceError(DeviceShortName.IDS)?.Code,
					Status = EmiratesIdScanner.Status.ToString(),
				},*/
                
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(PinPad),
					OperationalStatus = new OperationalStatus()
					{
					},
					ErrorCode = PinPad.GetDeviceError(DeviceShortName.PINPAD)?.Code,
					Status = PinPad.Status.ToString(),
				},
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(JournalPrinter),
					OperationalStatus = new OperationalStatus()
					{
						InkStatus = JournalPrinter.GetPaperStatus(),
                        PaperStatus = JournalPrinter.GetReceiptPaperStatus()
                    },
					ErrorCode = JournalPrinter.GetDeviceError(DeviceShortName.JPR)?.Code,
					Status = JournalPrinter.Status.ToString(),
				},
                new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(ReceiptPrinter),
					OperationalStatus = new OperationalStatus()
					{
						InkStatus = ReceiptPrinter.GetPaperStatus(),
                        PaperStatus = ReceiptPrinter.GetReceiptPaperStatus()

                    },
					ErrorCode = ReceiptPrinter.GetDeviceError(DeviceShortName.SPR)?.Code,
					Status = ReceiptPrinter.Status.ToString(),
				},
                new CMEntities.DeviceStatus()
                {
                    DeviceName = nameof(FingerprintScanner),
                    OperationalStatus = new OperationalStatus()
                    {
                    },
                    ErrorCode = FingerprintScanner.GetFingerPrintErrorCode(),
                    Status = FingerprintScanner.GetFingerPrintStatus().ToString(),
                },
                
				/*new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(StatementPrinter),
					OperationalStatus = new OperationalStatus()
					{
						InkStatus = StatementPrinter.GetPaperStatus(),
						PaperStatus = StatementPrinter.GetPaperStatus(),
					},
					ErrorCode = StatementPrinter.GetDeviceError(DeviceShortName.SPR)?.Code,
					Status = StatementPrinter.GetPrinterStatus().ToString(),
				},
				new CMEntities.DeviceStatus()
				{
					DeviceName = nameof(SignpadScanner),
					OperationalStatus = new OperationalStatus()
					{
					},
					ErrorCode = SignpadScanner.GetDeviceError(DeviceShortName.SIGN)?.Code,
					Status = SignpadScanner.Status.ToString(),
				}
                */
			};

            UpdateJournal(status);

            string json = JsonConvert.SerializeObject(status);
            Logger.Info("GetDevicesStatus: " + json);
			return status;
		}
         private void UpdateJournal(List<CMEntities.DeviceStatus> status)
        {

            var jStatus = new List<CMEntities.DeviceJournalStatus> ();
            
            foreach (var device in status)
            {
                if (device.Status != "Online")
                {
                    Journal.DeviceError(device.DeviceName, device.Status, device.ErrorCode);
                }
                //jStatus.Add(new CMEntities.DeviceJournalStatus(device.DeviceName, device.Status, device.ErrorCode));
            }
            //newList.Add(new Interface.Entities.CassetteInfo(item.Value, item.Count, item.Index, item.Type));
            //string json = JsonConvert.SerializeObject(jStatus);
            //Logger.Info("GetDevicesStatus: " + json);
            //Journal.Write(json);
        }

	}
}