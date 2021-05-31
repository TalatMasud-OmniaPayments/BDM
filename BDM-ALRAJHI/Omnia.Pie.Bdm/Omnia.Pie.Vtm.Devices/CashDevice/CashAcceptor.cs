namespace Omnia.Pie.Vtm.Devices.CashDevice
{
	using AxNXCashAcceptorXLib;
	using NXCashAcceptorXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Dto;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.Devices.CashDispenser.Denominate;
    using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
    using Omnia.Pie.Vtm.Framework.Configurations;
    using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Supervisor.Shell;
	using System;
	using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;
    using Newtonsoft.Json;

    public class CashAcceptor : Device, ICashAcceptor
	{
		internal readonly DeviceOperation<bool> StartCashInOperation;
		internal readonly DeviceOperation<Cash> AcceptCashOperation;
		internal readonly DeviceOperation<bool> StoreCashOperation;
		internal readonly DeviceOperation<bool> RollBackCashOperation;
		internal readonly DeviceOperation<bool> RetractCashOperation;
		internal readonly DeviceOperation<bool> ConfigureAcceptableNotesOperations;
		internal readonly DeviceOperation<bool> SetMediaInfoOperation;
        internal readonly DeviceOperation<bool> SetExchangeOperation;
        internal readonly DeviceOperation<bool> SetResetOnInitialisationOperation;
        internal readonly DeviceOperation<bool> CancelAcceptCashOperation;
        public bool CaptureNotes = false;

        protected readonly IJournal _journal;
        AxNXCashAcceptorX ax;
       
		Cash cash;
        Cash backupCash;
        public bool isRefused { get; private set; }
        public bool isCassettteChanged { get; set; }
        public event EventHandler <string> CashAcceptorStatusChanged;
        public event EventHandler<string> CashAcceptorUnitChanged;


        public CashAcceptor(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights, IJournal journal) :
			base(deviceErrorStore, logger, journal, guideLights)
		{
            _journal = Journal;
            //_journal.TransactionStarted(EJTransactionType.Financial, "Cash Acceptor Initialization");
            
            Operations.AddRange(new DeviceOperation[] {
				StartCashInOperation = new DeviceOperation<bool>(nameof(StartCashInOperation), Logger, Journal),
				AcceptCashOperation = new DeviceOperation<Cash>(nameof(AcceptCashOperation), Logger, Journal),
				StoreCashOperation = new DeviceOperation<bool>(nameof(StoreCashOperation), Logger, Journal),
				RollBackCashOperation = new DeviceOperation<bool>(nameof(RollBackCashOperation), Logger, Journal),
				RetractCashOperation = new DeviceOperation<bool>(nameof(RetractCashOperation), Logger, Journal),
				ConfigureAcceptableNotesOperations = new DeviceOperation<bool>(nameof(ConfigureAcceptableNotesOperations), Logger, Journal),
				SetMediaInfoOperation = new DeviceOperation<bool>(nameof(SetMediaInfoOperation), Logger, Journal),
                SetExchangeOperation = new DeviceOperation<bool>(nameof(SetExchangeOperation), Logger, Journal),
                SetResetOnInitialisationOperation = new DeviceOperation<bool>(nameof(SetResetOnInitialisationOperation), Logger, Journal),

                CancelAcceptCashOperation = new DeviceOperation<bool>(nameof(CancelAcceptCashOperation), Logger, Journal)
			});

			Logger.Info("CashAcceptor Initialized");
		}

		public bool HasPendingCashInside => HasMediaInserted;
		public bool IsCashInRunning { get; private set; }
		public bool HasMediaInserted { get; private set; }
		public event EventHandler MediaChanged;

        public List<Interface.Entities.CassetteInfo> GetCessettes()
        {
            string defaultCurrency = TerminalConfiguration.Section?.Currency; //GetDefaultCurrency();
            var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
            var list = denominateOperation.GetCessettes();
            //var list = GetMediaInfo();
            var newList = new List<Interface.Entities.CassetteInfo>();

            foreach (var item in list)
            {
                bool isDuplicate = false;
                foreach (var addedItem in newList)
                {

                    if (addedItem.Type == item.Type && addedItem.Index == item.Index)
                    {
                        isDuplicate = true;
                        break;
                    }

                }
                if (!isDuplicate) {
                    newList.Add(new Interface.Entities.CassetteInfo(item.Value, item.Count, item.Index, item.Type));
                }
            }

            return newList;
        }

        #region Overridden Functions

        protected override AxHost CreateAx() {
            ax = new AxNXCashAcceptorX();
            Logger.Info("CashAcceptor ax created");
            return ax;
        } 

		protected override int OpenSessionSync(int timeout)
		{
			return ax.OpenSessionSync(timeout);
		}

		protected override async void OnInitialized()
		{

            Logger.Info($"CashAcceptor OnInitialized called");

            ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.NoteError += Ax_NoteError;
			ax.CashUnitError += Ax_CashUnitError;
			ax.ItemsRefused += Ax_ItemsRefused;
			ax.AcceptCancelled += Ax_AcceptCancelled;
			ax.AcceptCashComplete += Ax_AcceptCashComplete;
			ax.StoreCashComplete += Ax_StoreCashComplete;
			ax.Timeout += Ax_Timeout;
			ax.RollbackCashComplete += Ax_RollbackCashComplete;
			ax.ItemsTaken += Ax_ItemsTaken;
			ax.ItemsInserted += Ax_ItemsInserted;
			ax.RetractComplete += Ax_RetractComplete;
			ax.ResetComplete += Ax_ResetComplete;
			ax.StartCashInComplete += Ax_StartCashInComplete;
			ax.ShutterStatusChanged += Ax_ShutterStatusChanged;
            ax.OpenShutterComplete += Ax_OpenShutterComplete;
            ax.CloseShutterComplete += Ax_CloseShutterComplete;
            ax.P6Inputed += Ax_P6Inputed;
            ax.CashUnitChanged += Ax_CashUnitChanged;
            ax.MediaDetected += Ax_MediaDetected;
            ax.XFSEvent += Ax_XFSEvent;
            ax.StatusChanged += Ax_StatusChanged;
           

            var isSerialNumerb = ConfigurationManager.AppSettings["CaptureNotesSerialNumner"].ToString();
            bool.TryParse(isSerialNumerb, out CaptureNotes);

            //var temp = ax.SetMixedMode(0);
            //Logger.Info($" SetMixedMode response=> {temp} ");

            if (Status == DeviceStatus.Online)
			{
				ConfigureAcceptableNotes(new[] {
					new CashItemType {
						Currency = TerminalConfiguration.Section?.Currency,
						Value = 5,
						Configured = true
                    },
                    new CashItemType {
                        Currency = TerminalConfiguration.Section?.Currency,
                        Value = 1,
                        Configured = false
                    },
                    new CashItemType {
                        Currency = TerminalConfiguration.Section?.Currency,
                        Value = 20,
                        Configured = false
                    },

                    new CashItemType {
						Currency = TerminalConfiguration.Section?.Currency,
						Value = 200,
						Configured = false
					},
                    new CashItemType {
                        Currency = TerminalConfiguration.Section?.Currency,
                        Value = 1000,
                        Configured = false
                    }
                });
			}
            
            await ExchangeAndResetOnInitialisation();

        }

        

        private void Ax_MediaDetected(object sender, _DNXCashAcceptorXEvents_MediaDetectedEvent e)
        {
            Logger.Info($"CashAcceptor status changed{sender.ToString()} AndEventType: {e.unitNumber} => Ax_MediaDetected");
        }

        private void Ax_XFSEvent(object sender, _DNXCashAcceptorXEvents_XFSEventEvent e)
        {
            Logger.Info($"CashAcceptor status changed{sender.ToString()} AndEventType: {e.eventType} AndCommandCode: {e.commandCode} AndHResult: {e.hResult} => Ax_XFSEvent");

        }

        private void Ax_StatusChanged(object sender, _DNXCashAcceptorXEvents_StatusChangedEvent e)
        {
            //this.Status = e.newValue == "DEVONLINE" ? DeviceStatus.DevOnline : DeviceStatus.NoDevice;
            
            Logger.Info($"CashAcceptor status changed1{sender.ToString()} AndEvent: {e.newValue} => Ax_StatusChanged");
            CashAcceptorStatusChanged?.Invoke(sender, e.newValue);
        }



        private void Ax_CashUnitChanged(object sender, _DNXCashAcceptorXEvents_CashUnitChangedEvent e)
        {

            isCassettteChanged = true;
            Logger.Info($"Cassette changed{sender.ToString()} => Ax_CashUnitChanged");
            Logger.Info($"Cassette changed unitNumber: {e.unitNumber.ToString()} => Ax_CashUnitChanged");
            //Journal.CashAcceptorCassetteChanged(e.unitNumber.ToString());
            var cassetteIds = ax.GetExtraStatus("CassetteIDs");
            Logger.Info($"Cassette changed cassetteIds: {cassetteIds} => Ax_CashUnitChanged");
            //ax.SetCashUnitInfo();

            for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
            {
                var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);

                //var id = logicalUnit.Number;
                
                if (logicalUnit.Number == e.unitNumber)
                {

                    if (logicalUnit.Status.ToUpper() == "MISSING")
                    {
                        Journal.CashAcceptorCassetteRemoved(logicalUnit.UnitID);
                    }
                    else if (logicalUnit.Status.ToUpper() == "OK" )
                    {
                        Journal.CashAcceptorCassetteInstalled(logicalUnit.UnitID, logicalUnit.Status);
                    }
                    //Logger.Info2($"LogicalUnit Id to reset: {id}");
                    Logger?.Info($"LogicalUnit Cassette Status={logicalUnit.Status} And ItemID:{(object[])logicalUnit.ItemID} AndCassetteType:{logicalUnit.Type}  AndUnitID:{logicalUnit.UnitID}  AndCurrencyID:{logicalUnit.CurrencyID} ");
                    //int[] idNumb = { e.unitNumber};
                    //SetMediaInfo(idNumb, null);
                }
            }

            CashAcceptorUnitChanged?.Invoke(sender, e.unitNumber.ToString());
        }

        public MediaUnit[] GetMediaInfo()
        {
            var currencies = new Dictionary<int, string>();
            for (var i = 0; i < ax.NumberOfItemType; i++)
            {
                var itemType = (IItemType)ax.get_ItemType(i);
                if (itemType.Configured == 0 || string.IsNullOrWhiteSpace(itemType.Currency))
                    continue;
                if (!currencies.ContainsKey(itemType.Value))
                    currencies.Add(itemType.Value, itemType.Currency);
            }

            var x = new List<MediaUnit>();
            for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
            {
                var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);


                var cassette = ToMediaUnit(logicalUnit);
                if (logicalUnit.Type == CashAcceptorUnitType.Retract)
                    cassette.RetractedCount = logicalUnit.CashInCount;
                else if (logicalUnit.Type == CashAcceptorUnitType.Reject)
                    cassette.RejectedCount = logicalUnit.CashInCount;
                else
                    cassette.Count = logicalUnit.CashInCount;
                cassette.Value = -1; // flag of cassette
                                     //TODO Temporary while we donot have correct value in logicalUnit.CurrencyID
                if (string.IsNullOrWhiteSpace(cassette.Currency) && currencies.Count > 0)
                    cassette.Currency = currencies.FirstOrDefault().Value;
                x.Add(cassette);
                //Logger?.Info($"Cassette Status1={cassette.Status} AndCassetteType:{cassette.Type}  AndItemValue:{cassette.Value} ");

                var subMediaUnits = new List<MediaUnit>();
                var values = (dynamic)logicalUnit.ItemValue;

                /*foreach(var ii in values)
                {
                    Logger?.Info($"CashAcceptorConfiguredValues= {ii}");
                }*/

                var counts = (dynamic)logicalUnit.ItemCount;
                var configured = (dynamic)logicalUnit.ItemConfigured;
                var itemCount = (counts as object[])?.Length;

                for (var ii = 0; ii < itemCount; ii++)
                {
                    var value = (int)values[ii];
                    if (value == 0)
                        continue;
                    var count = (int)counts[ii];
                    var enabled = (int)configured[ii];
                    if (enabled == 0 && count == 0)
                        continue; //skip not configurated and empty counts
                    var same = subMediaUnits.FirstOrDefault(iii => iii.Id == logicalUnit.Number && iii.Value == value);

                    if (same != null)
                    {
                        if (logicalUnit.Type == CashAcceptorUnitType.Retract)
                            same.RetractedCount += count;
                        else if (logicalUnit.Type == CashAcceptorUnitType.Reject)
                            same.RejectedCount += count;
                        else
                            same.Count += count;
                        same.TotalCount += count;
                    }
                    else
                    {
                        var mediaUnit = ToMediaUnit(logicalUnit);
                        mediaUnit.Value = value;

                        if (currencies.ContainsKey(value))
                            mediaUnit.Currency = currencies[value];
                        else if (!string.IsNullOrWhiteSpace(logicalUnit.CurrencyID))
                            mediaUnit.Currency = logicalUnit.CurrencyID;
                        else
                            mediaUnit.Currency = "---";

                        if (logicalUnit.Type == CashAcceptorUnitType.Retract)
                            mediaUnit.RetractedCount = count;
                        else if (logicalUnit.Type == CashAcceptorUnitType.Reject)
                            mediaUnit.RejectedCount = count;
                        else
                            mediaUnit.Count = count;
                        mediaUnit.TotalCount = count;
                        subMediaUnits.Add(mediaUnit);

                        Logger?.Info($"CashAcceptor Cassette Status2={mediaUnit.Status} AndCassetteType:{mediaUnit.Type}  AndItemValue:{mediaUnit.Value} ");

                        x.Add(mediaUnit);
                    }
                }
            }

            return x.ToArray();
        }
        private void Ax_P6Inputed(object sender, EventArgs e)
        {
            Logger.Info($"{sender.ToString()} => Ax_P6Inputed");
            //throw new NotImplementedException();
        }

        protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
            
		}

		protected override IGuideLight GuideLight
		{
			get
			{
				return GuideLights.CashDispenser;
			}
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

        protected override void OnDisposing()
        {
            IsCashInRunning = false;
            HasMediaInserted = false;

            ax.DeviceError -= Ax_DeviceError;
            ax.FatalError -= Ax_FatalError;
            ax.NoteError -= Ax_NoteError;
            ax.CashUnitError -= Ax_CashUnitError;
            ax.ItemsRefused -= Ax_ItemsRefused;
            ax.AcceptCancelled -= Ax_AcceptCancelled;
            ax.AcceptCashComplete -= Ax_AcceptCashComplete;
            ax.StoreCashComplete -= Ax_StoreCashComplete;
            ax.Timeout -= Ax_Timeout;
            ax.RollbackCashComplete -= Ax_RollbackCashComplete;
            ax.ItemsInserted -= Ax_ItemsInserted;
            ax.ItemsTaken -= Ax_ItemsTaken;
            ax.RetractComplete -= Ax_RetractComplete;
            ax.ResetComplete -= Ax_ResetComplete;
            ax.StartCashInComplete -= Ax_StartCashInComplete;
            ax.ShutterStatusChanged -= Ax_ShutterStatusChanged;
            ax.OpenShutterComplete -= Ax_OpenShutterComplete;
            ax.CloseShutterComplete -= Ax_CloseShutterComplete;
            ax.P6Inputed -= Ax_P6Inputed;
            ax.CashUnitChanged -= Ax_CashUnitChanged;
            ax.XFSEvent -= Ax_XFSEvent;
            ax.StatusChanged -= Ax_StatusChanged;

        }
		#endregion

		#region Public Functions

		public override async Task ResetAsync()
		{
            Logger.Info($"{GetType()} => Reset Cash Acceptor");

            GuideLight?.TurnOff();
			await ResetOperation.StartAsync(() => ax.Reset((short?)ax.GetRetractUnit()?.Number ?? 0));
			HasMediaInserted = false;
			IsCashInRunning = false;
            
		}

		public Task<Cash> AcceptCashAsync() => AcceptCash(true);

		public Task<Cash> AcceptMoreCashAsync() => AcceptCash(false);

        public string GetOldCassettes()
        {
            var oldCassettes = ax.GetExtraStatus("CassetteIDs");
            Logger.Info($" Old cassetteIds: {oldCassettes}");
            return oldCassettes;
        }
        public string GetUnchangedCassettes(string oldCassettes)
        {
            Logger.Info($"Verify Old cassetteIds: {oldCassettes}");


            var unchangedCassettes = "";
            var newCassettes= ax.GetExtraStatus("CassetteIDs");
            Logger.Info($"Verify New cassetteIds: {newCassettes}");


            //oldCassettes = "CGQA204749,CGQA205044,NULLSERIALNUMBER,CGQA205408,CGQA204677,CGQA204230,CGIS201634,NULLSERIALNUMBER";
            //newCassettes = "CGQA204749,CGQA205044,NULLSERIALNUMBER,CGQA205408,CGQA204677,CGQA204230,CGIS201634,NULLSERIALNUMBER";

            string[] oldIds = oldCassettes.Split(',');

            //string[] newIds = newCassettes.Split(',');

            var lastOldId = oldIds.Last();
            //var lastNewIds = newIds.Last();

            if (lastOldId == "NULLSERIALNUMBER")
            {
                Array.Resize(ref oldIds, oldIds.Length - 1);
            }

            /*if (lastNewIds == "NULLSERIALNUMBER")
            {
                Array.Resize(ref newIds, newIds.Length - 1);
            }*/

            foreach (var oldId in oldIds)
            {
                if (newCassettes.Contains(oldId))
                {
                    if (unchangedCassettes.Length == 0)
                    {
                        unchangedCassettes = oldId;

                    }
                    else
                    {
                        unchangedCassettes = unchangedCassettes + ", " + oldId;

                    }
                }
            }
            
            return unchangedCassettes;
        }
		public void CancelAcceptCash()
		{
            Logger.Info($"{GetType()} => Cancel Accept Cash");

            GuideLight?.TurnOff();
			CancelAcceptCashOperation.Start(() => ax.CancelAccept());
			Journal.CashAcceptorDepositCanceled();
			HasMediaInserted = false;  // BUG: we can cancel accepting of additional cash
			cash = null;               // BUG: we can cancel accepting of additional cash
            backupCash = null;
		}

		public async Task StoreCashAsync()
		{
            Logger.Info($"{GetType()} => Store Accept Cash");
            //
            await StoreCashOperation.StartAsync(() =>

            ax.StoreCash()
            );
            HasMediaInserted = false;
			IsCashInRunning = false;
			if (cash != null) {
                LogNotesSerialNUmbers();
                Journal.CashAcceptorCashStored(cash.Denominations.ToCashInStatusInfoDto(), 0 /*No way for now to get an exact count of refused*/);
                
            }
            if (backupCash != null)
                backupCash.Denominations.ToCashInStatusInfoDto();

            backupCash = null;
        }

        public async Task RollbackCashAndWaitTakenAsync()
		{
            Logger.Info($"{GetType()} => Rollback Cash and wait");

            GuideLight?.TurnOn();

            /*
            if (IsCashInRunning)
            {
                Logger.Info($"{GetType()} => Cancelling Accept Cash before Rollback");
                ax.CancelAccept();
            }
            */
                
			await RollBackCashOperation.StartAsync(() => ax.RollbackCash(Timeout.AwaitTaken));
			HasMediaInserted = false;
			IsCashInRunning = false;
			cash = null;
            backupCash = null;

        }

		public async Task<bool> RetractCashAsync()
		{
            Logger.Info($"{GetType()} => Retract Cash");
            
            bool cashRetracted = await RetractCashOperation.StartAsync(() => ax.Retract((short)CashAcceptorRetractArea.Retract));
            //LogNotesSerialNUmbers();
            HasMediaInserted = false;
			IsCashInRunning = false;
			if (cash != null && cashRetracted)
			{
                LogNotesSerialNUmbers();
                Journal.CashAcceptorCashRetracted(cash.Denominations.ToCashInStatusInfoDto(), 0);
			}
			cash = null;
            backupCash = null;
            return cashRetracted;
		}

		public void SetMediaInfo(int[] ids, int[] counts)
		{
			SetMediaInfoOperation.Start(() =>
			{
                Logger.Info2($"SetMediaInfoOperation Started");
                var _ids = new List<object>();

				if (ids != null)
				{
					foreach (var i in ids)
					{
						_ids.Add(i);
                        Logger.Info2($"MediaInfo Id to reset: {i}");
                    }
				}
				else
				{
					for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
					{
                        var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);

                        var id = logicalUnit.Number;

                        _ids.Add(id);
                        Logger.Info2($"LogicalUnit Id to reset: {id}");
                        //Logger?.Info($"LogicalUnit Cassette Status={logicalUnit.Status} AndCassetteType:{logicalUnit.Type}  AndItemValue:{logicalUnit.ItemValue}  AndCurrencyID:{logicalUnit.CurrencyID} ");

                    }
                }

				var result = ax.StartExchangeSync(_ids.ToArray() as object);
				Logger.Info2($"{nameof(ax.StartExchangeSync)}: {nameof(result)}={result}");

				// it only sets zeros to counters, that all
				result = ax.EndExchangeSync();
				Logger.Info2($"{nameof(ax.EndExchangeSync)}: {nameof(result)}={result}");

				return result;
			});
		}

        public async Task ExchangeAndResetOnInitialisation()
        {

            try
            {
                PerformExchange();
            }
            catch (Exception ex)
            {
                await ResetAsync();
            }

        }
        public void PerformExchange()
        {

            //var exchangeCompletionTask = new TaskCompletionSource<int>();
            SetExchangeOperation.Start(() =>
            {
                Logger.Info2($"SetExchangeOperation Started");


                object[] cashunitToExchange = new object[0];

                int result = ax.StartExchangeSync(cashunitToExchange);
                Logger.Info2($"{nameof(ax.StartExchangeSync)}: {nameof(result)}={result}");

                result = ax.EndExchangeSync();
                Logger.Info2($"{nameof(ax.EndExchangeSync)}: {nameof(result)}={result}");

                if (result == 0)
                {
                    result = ax.EndExchangeSync();
                    Logger.Info2($"{nameof(ax.EndExchangeSync)}: {nameof(result)}={result}");
                }
                // it only sets zeros to counters, that all

                //exchangeCompletionTask.TrySetResult(result);
                return result;
            });
        }

        public CashAcceptorCassetteStatus GetCashAcceptorStatus(List<CassetteStatus> cassetteStatus)
        {
            var status = CashAcceptorCassetteStatus.FULL;
            //var unhealthyCsts = new List<ILogicalUnit>();


            


            for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
            {
                var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);
                //Logger?.Info($"Cassette Status={logicalUnit.Status} AndCassetteType:{logicalUnit.Type}  AndItemValue:{logicalUnit.ItemValue}  AndCurrencyID:{logicalUnit.CurrencyID} ");
                Logger?.Info($"LogicalUnit Cassette Status={logicalUnit.Status} And ItemID:{(object[])logicalUnit.ItemID} AndCassetteType:{logicalUnit.Type}  AndUnitID:{logicalUnit.UnitID} AndItemValue:{logicalUnit.ItemValue}  AndCurrencyID:{logicalUnit.CurrencyID} ");

                


                    /*string json = JsonConvert.SerializeObject(logicalUnit);
                    Logger?.Info($"LogicalUnit Cassette json1={json}");

                    foreach (var itemId in logicalUnit.ItemID)
                    {
                        var property = itemId.Value;

                        if (property.Value != null)
                        {
                            Console.WriteLine(property.Name + "\t(Type: {0})", property.Value.GetType().ToString());
                        }
                    }*/

                    if (logicalUnit.Type == "CASHIN" || logicalUnit.Type == "RECYCLING") {
                   

                    switch (logicalUnit.Status.ToUpper()) {
                        case "OK":
                            status = CashAcceptorCassetteStatus.OK;
                            break;
                        case "HIGH":
                            status = CashAcceptorCassetteStatus.HIGH;
                            break;
                        case "FULL":
                            status = CashAcceptorCassetteStatus.FULL;
                            break;
                        case "FATAL":
                            status = CashAcceptorCassetteStatus.FATAL;
                            break;
                        case "MISSING":
                            status = CashAcceptorCassetteStatus.MISSING;
                            break;
                        default:
                            status = CashAcceptorCassetteStatus.UNKNOWN;
                            break;
                    }

                    /*for (var j = 0; j < cassetteStatuses.Count; j++)
                    {
                        var cassette = cassetteStatuses[j];

                        int cassetteIndex = cassette.unitId - 1;
                        string unitId = logicalUnit.UnitID.Substring(logicalUnit.UnitID.Length - 1);

                        if (cassetteIndex.ToString() == unitId && status == CashAcceptorCassetteStatus.FATAL)
                        {
                            cassetteStatuses.RemoveAt(j);
                            status = CashAcceptorCassetteStatus.OK;
                        }

                    }*/

                    if (status != CashAcceptorCassetteStatus.OK)
                    {
                        break;
                    }
                    
                }
            }

            return status;
        }

        public int GetMaxStackerLimit()
        {

            return ax.MaxStackerItem;
        }
        public CashAcceptorStackerStatus GetStackerStatus()
        {
            var status = CashAcceptorStackerStatus.FULL;

            for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
            {
                var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);

                if (logicalUnit.Type == "CASHIN" || logicalUnit.Type == "RECYCLING")
                {
                    var tempTotalCount = logicalUnit.PCUTotalCount;
                    var tempCashInCount = logicalUnit.PCUCashInCount;
                    var tempCashInMaxCount = logicalUnit.PCUMaximumCount;

                    switch (ax.StackerStatus.ToUpper())
                    {
                        case "EMPTY":
                            status = CashAcceptorStackerStatus.EMPTY;
                            break;
                        case "NOTEMPTY":
                            status = CashAcceptorStackerStatus.NOTEMPTY;
                            break;
                        case "FULL":
                            status = CashAcceptorStackerStatus.FULL;
                            break;
                        case "NOTSUPP":
                            status = CashAcceptorStackerStatus.NOTSUPP;
                            break;
                        
                        default:
                            status = CashAcceptorStackerStatus.UNKNOWN;
                            break;
                    }
                    break;
                }
            }

            return status;
        }
        public int AvailableToDepositCashIn()
        {
            var capacity = 0;
            //ax.MaxStackerItem;
            for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
            {
                var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);

                if (logicalUnit.Type == "CASHIN")
                {
                    var tempTotalCount = logicalUnit.PCUTotalCount[0];
                    capacity = ax.MaxCashInItem - (int)tempTotalCount;
                    //GetMediaInfo();
                    break;
                }
            }

            return capacity;
        }

        
        public void CheckCassetteStatus()
        {
            var cassettes = GetMediaInfo();

            foreach (var cassette in cassettes)
            {
                Logger?.Info($"CashAcceptor Cassette Status={cassette.Status} Cassette UnitStatus={cassette.UnitStatus} AndCassetteType:{cassette.Type} ");
            }

        }
        #endregion

        #region Private Functions & Events

        private void Ax_ShutterStatusChanged(object sender, _DNXCashAcceptorXEvents_ShutterStatusChangedEvent e)
		{
			bool isMainShutter = (e.position == "OUTPOSITION");
			if (isMainShutter)
			{
				switch (e.newValue)
				{
					case "OPEN":
						Journal.CashAcceptorShutterOpened();
						break;

					case "CLOSED":
						Journal.CashAcceptorShutterClosed();
						break;
				}
			}
		}

        private void Ax_OpenShutterComplete(object sender, _DNXCashAcceptorXEvents_OpenShutterCompleteEvent e)
        {
            Journal.CashAcceptorShutterOpenedEvent();
        }
        private void Ax_CloseShutterComplete(object sender, _DNXCashAcceptorXEvents_CloseShutterCompleteEvent e)
        {
            Journal.CashAcceptorShutterClosedEvent();
        }



        private void Ax_StartCashInComplete(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Start Cash-In Event");
            StartCashInOperation.Stop(true);
            Logger?.Info($"{GetType()} => Start Cash-In");
            

        }

		private void Ax_AcceptCashComplete(object sender, EventArgs e)
		{
			AcceptCashOperation.Stop(GetCash());
            Logger?.Info($"{GetType()} => Accept Cash Completed");
            //LogNotesSerialNUmbers();
        }

		private void Ax_StoreCashComplete(object sender, _DNXCashAcceptorXEvents_StoreCashCompleteEvent e)
		{
			StoreCashOperation.Stop(true);
			WriteDevicePropertiesToLog();
            //LogNotesSerialNUmbers();
            Logger?.Info($"{GetType()} => Store Cash Complete");
        }

		private void Ax_RetractComplete(object sender, EventArgs e)
		{
			GuideLight?.TurnOff();
			RetractCashOperation.Stop(true);
            Logger?.Info($"{GetType()} => Retract Complete");
        }

		private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
            Logger?.Info($"{GetType()} => Reset Complete");
        }

		private void Ax_ItemsInserted(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Notes Inserted");
            //LogNotesSerialNUmbers();
            OnUserAction();
			GuideLight?.TurnOff();
			Journal.CashAcceptorItemsInserted();
			HasMediaInserted = true;
		}

		private void Ax_ItemsTaken(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Notes Taken");
            //LogNotesSerialNUmbers();
            OnUserAction();
			GuideLight?.TurnOff();
			Journal.CashAcceptorItemsTaken();
			HasMediaInserted = false;
			RollBackCashOperation.Stop(true);
			RetractCashOperation.Stop(false);

		}

		private void Ax_ItemsRefused(object sender, EventArgs e)
		{
            isRefused = true;
            Logger?.Info($"{GetType()} => Items Refused: {ax.LastRefusedCount}");
            StartCashInOperation.Stop(true);
            LogNotesSerialNUmbers();
        }

		private void Ax_RollbackCashComplete(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Rollback Complete");
            if (HasMediaInserted && cash != null)
			{
				Journal.CashAcceptorCashRefunded(cash.Denominations.ToCashInStatusInfoDto(), 0);
			}
			if (!HasMediaInserted)
			{
				// If there's no cash, rollback operation should not wait until customer collects it, so rollback operation should be stopped on rollback complete event
				RollBackCashOperation.Stop(false);
			}

           // LogNotesSerialNUmbers();
        }

		private void Ax_AcceptCancelled(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Accept Cash Cancelled");
            AcceptCashOperation.Stop(new DeviceOperationCanceledException(nameof(AcceptCashOperation)));
		}

		private void Ax_Timeout(object sender, EventArgs e)
		{
            Logger?.Info($"{GetType()} => Timeout Error");
            OnError(new DeviceTimeoutException(nameof(CashAcceptor)));
		}

		private void Ax_CashUnitError(object sender, _DNXCashAcceptorXEvents_CashUnitErrorEvent e)
		{
            Logger?.Info($"Cash Unit Error: Operation={e.unitNumber}, Unit Number={e.unitNumber}");
            OnError(new DeviceMalfunctionException(nameof(AxNXCashAcceptorX.CashUnitError), e.unitNumber));
		}

		private void Ax_NoteError(object sender, _DNXCashAcceptorXEvents_NoteErrorEvent e)
		{
            Logger?.Info($"Note Error: Reason={e.reason}");
            OnError(new NoteErrorException(nameof(AxNXCashAcceptorX.NoteError), e.reason));
        }

		private void Ax_FatalError(object sender, _DNXCashAcceptorXEvents_FatalErrorEvent e)
		{
            Logger?.Info($"CashAcceptor Fatal Error: Action={e.action}, Result={e.result}");
            OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXCashAcceptorXEvents_DeviceErrorEvent e)
		{
            StartCashInOperation.Stop(true);
            Logger?.Info($"CashAcceptor Device Error: Action={e.action}, Result={e.result}");
            OnError(new DeviceMalfunctionException(e.action, e.result));
		}

        private void LogNotesSerialNUmbers()
        {

            if (CaptureNotes)
            {
                var noteSerial = ax.GetExtraStatus("SerialNumberLists");
                //noteSerial = "123";
                //Logger?.Info($"Extra Serial numbers={noteSerial}");
                _journal.NotesInserted($"{noteSerial}");
                //journal.TransactionStarted(EJTransactionType.Financial, "Notes Insterted");
                /*Logger?.Info($"TotaltNumberOfP6Info={ax.NumberOfP6Info}");
                for (var i = 0; i < ax.NumberOfP6Info; i++)
                    {

                
                        var p6Info = (IP6Info)ax.get_P6Info(i);

                    _journal.NotesInserted($"Note Id={p6Info.NoteID}");

                    /*Logger.Properties(new
                        {
                            p6Info.Level,
                            p6Info.NumOfNotes,
                            p6Info.NoteID,
                            p6Info.NoteCount,
                            p6Info.NumOfSignature
                        });
                    }*/
            }
        }
		private async Task<Cash> AcceptCash(bool isStartCashIn)
		{
            Logger.Info($"{GetType()} => Start Accept Cash");
            isRefused = false;
            GuideLight?.TurnOn();
			if (isStartCashIn)
			{
				await StartCashInOperation.StartAsync(() => ax.StartCashIn());
				Journal.CashAcceptorDepositActivated();
			}
			IsCashInRunning = true;
			try
			{
				await PlayBeepAsync();
                //Journal.CashAcceptorShutterOpened();
				cash = await AcceptCashOperation.StartAsync
				(() =>
					ax.AcceptCash(Timeout._30Sec, Timeout.AwaitTaken)
				);
                //cash = tempcash;
                if (!isRefused || backupCash == null) {

                    backupCash = new Cash(cash);
                }
                else
                {
                    cash.TotalNotes = backupCash.TotalNotes;
                    cash.TotalAmount = backupCash.TotalAmount;
                    cash.Denominations = backupCash.Denominations;
                }

            }
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				await StopBeepAsync();
			}
			
			Journal.CashAcceptorCashAccepted(cash.Denominations.ToCashInStatusInfoDto(), 0 /*No way for now to get an exact count of refused*/);
			return cash;
		}

		private MediaUnit ToMediaUnit(ILogicalUnit logicalUnit) => new MediaUnit
		{
			MediaDevice = this,
			Id = logicalUnit.Number,
			Type = logicalUnit.Type,
			Status = logicalUnit.Status,
            UnitId = logicalUnit.UnitID,
            MaxCount = logicalUnit.MaximumCount,
			InitialCount = logicalUnit.InitialCount,
			DispensedCount = logicalUnit.DispensedCount,
			PresentedCount = logicalUnit.PresentedCount,
			RejectedCount = logicalUnit.RejectCount,
			RetractedCount = logicalUnit.RetractedCount,
			TotalCount = logicalUnit.TotalCount,
		};

		private void ConfigureAcceptableNotes(CashItemType[] cashItemTypes)
		{
			ConfigureAcceptableNotesOperations.Start(() =>
			{
				foreach (var i in ax.GetItemTypes())
				{
					var cashItemType = cashItemTypes.FirstOrDefault(ii => ii.Currency == i.Currency && ii.Value == i.Value);
                    //Logger?.Info($"i.Value={i.Value}, cashItemType={cashItemType}");
                    if (cashItemType != null) {
                        //Logger?.Info($"i.Value={i.Value}, CurrencyType={cashItemType.Currency}, cashItemType.Configured={cashItemType.Configured}");
                        i.Configured = cashItemType.Configured ? 1 : 0;
                        //Logger?.Info($"i.Value={i.Value}, if i.Configured={i.Configured}, if cashItemType.Configured={cashItemType.Configured}");
                    }
                    Logger?.Info($"CurrencyValue={i.Value}, Configured={i.Configured}");
                }
				return ax.ConfigureNoteTypeSync();
			});
		}

		private Cash GetCash()
		{
			var items = new List<CashDenomination>();
			for (var i = 0; i < ax.NumberOfCashInStatus; i++)
			{
				var cashInStatus = ax.get_CashInStatus(i) as ICashInStatus;
				items.Add(new CashDenomination
				{
					Count = cashInStatus.ItemCount,
					Value = cashInStatus.Value,
					Amount = cashInStatus.ItemCount * cashInStatus.Value
				});
			}
			return new Cash
			{
				Denominations = items.ToArray(),
				TotalAmount = items.Sum(i => i.Amount),
				TotalNotes = items.Sum(i => i.Count),
			};
		}

		private void WriteDevicePropertiesToLog()
		{
			Logger.Properties(new
			{
				ax.AcceptorStatus,
				ax.CanPowerSaveControl,
				ax.CanShutterControl,
				ax.CashUnitChangeInprogress,
				ax.DevicePositionStatus,
				ax.DeviceStatus,
				ax.DeviceType,
				ExchangeType = string.Join(",", (object[])ax.ExchangeType),
				ax.HasInsertedSensor,
				HasPositions = string.Join(",", (object[])ax.HasPositions),
				ax.HasSafeDoor,
				ax.HasShutter,
				ax.HasTakenSensor,
				ax.LastCashInStatus,
				ax.LastRefusedCount,
				ax.MaxCashInItem,
				ax.MaxStackerItem,
				ax.NumberOfCashInStatus,
				ax.NumberOfItemType,
				ax.NumberOfLogicalUnit,
				ax.NumberOfP6Info,
				ax.NumberOfPositions,
				ax.PositionStatus,
				ax.PowerSaveRecoveryTime,
				ax.ReaderStatus,
				ax.RefusedReason,
				RetractAreas = string.Join(",", (object[])ax.RetractAreas),
				ax.SafeDoorStatus,
				ax.ShutterStatus,
				ax.StackerStatus,
				ax.TransportStatus
			});

			for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
			{
				var logicalUnit = (ILogicalUnit)ax.get_LogicalUnit(i);
				Logger.Properties(new
				{
					CashUnitName = logicalUnit.get_CashUnitName(),
					logicalUnit.Number,
					logicalUnit.Type,
					logicalUnit.TotalCount,
					logicalUnit.MaximumCount,
					logicalUnit.Status,
					logicalUnit.CashInCount,
					logicalUnit.UnitID,
					logicalUnit.CurrencyID,
					logicalUnit.NumberOfItem,
					ItemID = string.Join(",", (object[])logicalUnit.ItemID),
					ItemCount = string.Join(",", (object[])logicalUnit.ItemCount),
					ItemValue = string.Join(",", (object[])logicalUnit.ItemValue),
					ItemConfigured = string.Join(",", (object[])logicalUnit.ItemConfigured),
					logicalUnit.NumberOfPCU,
					PCUID = string.Join(",", (object[])logicalUnit.PCUID),
					PCUCashInCount = string.Join(",", (object[])logicalUnit.PCUCashInCount),
					PCUTotalCount = string.Join(",", (object[])logicalUnit.PCUTotalCount),
					PCUMaximumCount = string.Join(",", (object[])logicalUnit.PCUMaximumCount),
					PCUStatus = string.Join(",", (object[])logicalUnit.PCUStatus),
					PCUSensor = string.Join(",", (object[])logicalUnit.PCUSensor),
					Delimeter = "------------------------------",
					logicalUnit.InitialCount,
					logicalUnit.DispensedCount,
					logicalUnit.RetractedCount,
					PCURetractedCount = string.Join(",", (object[])logicalUnit.PCURetractedCount),
					PCUDispensedCount = string.Join(",", (object[])logicalUnit.PCUDispensedCount),
					PCUInitialCount = string.Join(",", (object[])logicalUnit.PCUInitialCount),
					PCUPositionName = string.Join(",", (object[])logicalUnit.PCUPositionName)
				});
			}

			for (var i = 0; i < ax.NumberOfItemType; i++)
			{
				var itemType = (IItemType)ax.get_ItemType(i);
				Logger.Properties(new
				{
					itemType.Currency,
					itemType.Value,
					itemType.Configured,
					itemType.Exponent,
					itemType.ID
				});
			}

			for (var i = 0; i < ax.NumberOfCashInStatus; i++)
			{
				var cashInStatus = (ICashInStatus)ax.get_CashInStatus(i);
				Logger.Properties(new
				{
					cashInStatus.Currency,
					cashInStatus.Value,
					cashInStatus.ItemCount,
					cashInStatus.Exponent,
					cashInStatus.ID
				});
			}

			for (var i = 0; i < ax.NumberOfP6Info; i++)
			{
				var p6Info = (IP6Info)ax.get_P6Info(i);
				Logger.Properties(new
				{
					p6Info.Level,
					p6Info.NumOfNotes,
					p6Info.NoteID,
					p6Info.NoteCount,
					p6Info.NumOfSignature
				});
			}

			for (var i = 0; i < ax.NumberOfPositions; i++)
			{
				var positions = (IPositions)ax.get_Positions(i);
				Logger.Properties(new
				{
					positions.Type,
					positions.PositionStatus,
					positions.ShutterStatus,
					positions.TransportStatus
				});
			}
		}

		#endregion
	}

	internal static class CashAcceptorExtensions
	{
		public static IEnumerable<CashInStatusDto> ToCashInStatusInfoDto(this CashDenomination[] cashDenomination) =>
			cashDenomination.Select(i => new CashInStatusDto(i.Value, CurrencyName.Sar, i.Count));

		internal static IEnumerable<ILogicalUnit> GetLogicalUnits(this AxNXCashAcceptorX ax)
		{
			for (var i = 0; i < ax.NumberOfLogicalUnit; i++)
				yield return (ILogicalUnit)ax.get_LogicalUnit(i);
		}

		internal static ILogicalUnit GetRetractUnit(this AxNXCashAcceptorX ax)
		{
			return ax.GetLogicalUnits().FirstOrDefault(i => i.Type == CashAcceptorUnitType.Retract);
		}

		internal static IEnumerable<IItemType> GetItemTypes(this AxNXCashAcceptorX ax)
		{
			for (var i = 0; i < ax.NumberOfItemType; i++)
				yield return (IItemType)ax.get_ItemType(i);
		}
	}
}