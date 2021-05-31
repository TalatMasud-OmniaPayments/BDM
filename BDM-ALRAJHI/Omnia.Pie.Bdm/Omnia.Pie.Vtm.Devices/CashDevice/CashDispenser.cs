namespace Omnia.Pie.Vtm.Devices.CashDevice
{
	using AxNXCashDispenserXLib;
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
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public class CashDispenser : Device, ICashDispenser
	{
		private const string NotEmpty = "NOTEMPTY";
		private const string NotEmptyWithCustomerAccess = "NOTEMPTYCUST";
		public AxNXCashDispenserX ax;

		internal readonly DeviceOperation<bool> DispenseCashOperation;
		internal readonly DeviceOperation<bool> PresentCashAndWaitTakenOperation;
		internal readonly DeviceOperation<bool> RetractCashOperation;
		internal readonly DeviceOperation<bool> SetMediaInfoOperation;
		public event EventHandler MediaChanged;

		public CashDispenser(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights, IJournal journal)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				DispenseCashOperation = new DeviceOperation<bool>(nameof(DispenseCashOperation), Logger, Journal),
				PresentCashAndWaitTakenOperation = new DeviceOperation<bool>(nameof(PresentCashAndWaitTakenOperation), Logger, Journal),
				RetractCashOperation = new DeviceOperation<bool>(nameof(RetractCashOperation), Logger, Journal),
				SetMediaInfoOperation = new DeviceOperation<bool>(nameof(SetMediaInfoOperation), Logger, Journal)
			});

			Logger.Info("CashDispenser Initialized");
		}

		#region Overridden Functions

		protected override void OnInitialized()
		{
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.CashUnitChanged += Ax_CashUnitChanged;
			ax.DevicePositionChanged += Ax_DevicePositionChanged;
			ax.PositionStatusChanged += Ax_PositionStatusChanged;
			ax.StackerStatusChanged += Ax_StackerStatusChanged;
			ax.CashUnitError += Ax_CashUnitError;
			ax.NotDispensable += Ax_NotDispensable;
			ax.DispenseComplete += Ax_DispenseComplete;
			ax.PresentComplete += Ax_PresentComplete;
			ax.Timeout += Ax_Timeout;
			ax.ItemsTaken += Ax_ItemsTaken;
			ax.RetractComplete += Ax_RetractComplete;
			ax.ResetComplete += Ax_ResetComplete;
			ax.TestCashUnitComplete += Ax_TestCashUnitComplete;
			ax.RejectComplete += Ax_RejectComplete;
		}

		protected override IGuideLight GuideLight => GuideLights.CashDispenser;

		protected override AxHost CreateAx()
		{
			return ax = new AxNXCashDispenserX();
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

		protected override int OpenSessionSync(int timeout)
		{
			return ax.OpenSessionSync(timeout);
		}

		protected override void OnDisposing()
		{
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.CashUnitChanged -= Ax_CashUnitChanged;
			ax.DevicePositionChanged -= Ax_DevicePositionChanged;
			ax.PositionStatusChanged -= Ax_PositionStatusChanged;
			ax.StackerStatusChanged -= Ax_StackerStatusChanged;
			ax.CashUnitError -= Ax_CashUnitError;
			ax.NotDispensable -= Ax_NotDispensable;
			ax.DispenseComplete -= Ax_DispenseComplete;
			ax.PresentComplete -= Ax_PresentComplete;
			ax.Timeout -= Ax_Timeout;
			ax.ItemsTaken -= Ax_ItemsTaken;
			ax.RetractComplete -= Ax_RetractComplete;
			ax.ResetComplete -= Ax_ResetComplete;
			ax.TestCashUnitComplete -= Ax_TestCashUnitComplete;
			ax.RejectComplete -= Ax_RejectComplete;
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		#endregion

		#region Public Functions

		public MediaUnit[] GetMediaInfo()
		{
			var x = new List<MediaUnit>();
			var counts = (dynamic)ax.PhysicalCount;
			var dispensedCounts = (dynamic)ax.PhysicalDispensedCount;
			var numbers = (dynamic)ax.UnitNumber;
			var types = (dynamic)ax.UnitType;
			var currencies = (dynamic)ax.UnitCurrencyID;
			var initialCounts = (dynamic)ax.PhysicalInitialCount;
			var maximums = (dynamic)ax.PhysicalMaximum;
			var presentedCounts = (dynamic)ax.PhysicalPresentedCount;
			var rejectCounts = (dynamic)ax.PhysicalRejectCount;
			var retractedCounts = (dynamic)ax.PhysicalRetractedCount;
			var statuses = (dynamic)ax.PhysicalStatus;
			var values = (dynamic)ax.UnitValue;
            var unitStatuses = (dynamic)ax.UnitStatus;

            for (var i = 0; i < ax.NumberOfLogicalUnits; i++)
			{
				var mediaUnit = new MediaUnit
				{
					MediaDevice = this,
					Id = (int)numbers[i],
					Type = (string)types[i],
					Currency = (string)currencies[i],
					Value = (int)values[i],
					Status = (string)statuses[i],
                    UnitStatus = (string)unitStatuses[i],
                    Count = (int)counts[i],
					MaxCount = (int)maximums[i],
					InitialCount = (int)initialCounts[i],
					DispensedCount = (int)dispensedCounts[i],
					PresentedCount = (int)presentedCounts[i],
					RejectedCount = (int)rejectCounts[i],
					RetractedCount = (int)retractedCounts[i],
					TotalCount = (int)counts[i]
				};

				//TODO Temporary while PhysicalDispensedCount shows 0,0,..
				if (mediaUnit.Type == DispenserUnitType.BillCassette)
				{
					mediaUnit.DispensedCount = mediaUnit.InitialCount - mediaUnit.TotalCount;
					mediaUnit.TotalCount = mediaUnit.InitialCount;
				}
				x.Add(mediaUnit);
			}

			return x.ToArray();
		}

		public void SetMediaInfo(int[] ids, int[] counts)
		{
			SetMediaInfoOperation.Start(() =>
			{
				object[] unitNumber = ax.UnitNumber as object[];
				var _ids = ids?.ToList();
				var physicalCount = (object[])ax.PhysicalCount;
				var physicalInitialCount = (object[])ax.PhysicalInitialCount;
				if (unitNumber != null && _ids != null)
				{
					for (int i = 0; i < unitNumber.Length; i++)
					{
						var ii = _ids.IndexOf((short)unitNumber[i]);
						if (ii != -1)
						{
							physicalCount[i] = counts?[ii] ?? 0;
							physicalInitialCount[i] = counts?[ii] ?? 0;
						}
					}
				}
				else
				{
					for (var i = 0; i < ax.NumberOfPhysicalUnits; i++)
					{
						physicalCount[i] = 0;
						physicalInitialCount[i] = 0;
					}
				}

				var result = ax.StartExchangeSync(unitNumber, 0);

				Logger.Info2($"{nameof(ax.StartExchangeSync)}: {nameof(result)}={result}");

				ax.PhysicalCount = (object)physicalCount;
				ax.PhysicalInitialCount = (object)physicalInitialCount;
				result = ax.EndExchangeSync();

				Logger.Info2($"{nameof(ax.EndExchangeSync)}: {nameof(result)}={result}");

				return result;
			});
		}

        public CashDispenserCassetteStatus GetCashDispenserStatus()
        {
            var status = CashDispenserCassetteStatus.FULL;

            var mediaInfo = GetMediaInfo();

            for (var i = 0; i < mediaInfo.Count(); i++)
            {
                var mediaUnit = mediaInfo[i];
                Logger?.Info($"CashDispenser Cassette Status={mediaUnit.Status} AndCassetteType:{mediaUnit.Type}  AndItemValue:{mediaUnit.Value}   AndMediaUnitId:{mediaUnit.Id} ");

                if (mediaUnit.Type == "CASHIN" || mediaUnit.Type == "RECYCLING" || mediaUnit.Type == "REJECTCASSETTE")
                {


                    switch (mediaUnit.Status.ToUpper())
                    {
                        case "OK":
                            status = CashDispenserCassetteStatus.OK;
                            break;
                        case "HIGH":
                            status = CashDispenserCassetteStatus.HIGH;
                            break;
                        case "FULL":
                            status = CashDispenserCassetteStatus.FULL;
                            break;
                        case "LOW":
                            status = CashDispenserCassetteStatus.LOW;
                            break;
                        case "EMPTY":
                            status = CashDispenserCassetteStatus.EMPTY;
                            break;
                        case "FATAL":
                            {

                                if (mediaUnit.Type == "REJECTCASSETTE") { 
                                    status = CashDispenserCassetteStatus.FATAL;
                                }
                                else
                                {
                                    status = CashDispenserCassetteStatus.FATAL;
                                }
                            }
                            break;
                        case "MISSING":
                            status = CashDispenserCassetteStatus.MISSING;
                            break;
                        case "MANIP":
                            status = CashDispenserCassetteStatus.MANIP;
                            break;
                        case "NOVALUES":
                            status = CashDispenserCassetteStatus.NOVALUES;
                            break;
                        default:
                            status = CashDispenserCassetteStatus.NOREFERENCE;
                            break;
                    }

                    if (status != CashDispenserCassetteStatus.EMPTY)
                    {
                        break;
                    }

                }
            }

            return status;
        }


        public List<CassetteStatus> GetDispensableCassettesStatus()
        {
            var dispenserStatuses = new List<CassetteStatus>();

            var status = CashDispenserCassetteStatus.FULL;

            var mediaInfo = GetMediaInfo();

            for (var i = 0; i < mediaInfo.Count(); i++)
            {
				status = CashDispenserCassetteStatus.EMPTY;
				var mediaUnit = mediaInfo[i];
                Logger?.Info($"CashDispenser Cassette Status={mediaUnit.Status} AndCassetteType:{mediaUnit.Type}  AndItemValue:{mediaUnit.Value}   AndMediaUnitId:{mediaUnit.Id} ");

                if (mediaUnit.Type == "CASHIN" || mediaUnit.Type == "RECYCLING" || mediaUnit.Type == "REJECTCASSETTE")
                {


                    //cassetteStatus.unitId = mediaUnit.Id;

                    switch (mediaUnit.Status.ToUpper())
                    {
                        case "OK":
                            status = CashDispenserCassetteStatus.OK;
                            break;
                        case "HIGH":
                            status = CashDispenserCassetteStatus.HIGH;
                            break;
                        case "FULL":
                            status = CashDispenserCassetteStatus.FULL;
                            break;
                        case "LOW":
                            status = CashDispenserCassetteStatus.LOW;
                            break;
                        case "EMPTY":
                            status = CashDispenserCassetteStatus.EMPTY;
                            break;
                        case "FATAL":
                            {

                                if (mediaUnit.Type == "REJECTCASSETTE")			// overflow
                                {
                                    status = CashDispenserCassetteStatus.FATAL;
                                }
                                else
                                {
                                    status = CashDispenserCassetteStatus.FATAL;
                                }
                            }
                            break;
                        case "MISSING":
                            status = CashDispenserCassetteStatus.MISSING;
                            break;
                        case "MANIP":
                            status = CashDispenserCassetteStatus.MANIP;
                            break;
                        case "NOVALUES":
                            status = CashDispenserCassetteStatus.NOVALUES;
                            break;
                        default:
                            status = CashDispenserCassetteStatus.NOREFERENCE;
                            break;
                    }


                    if (status != CashDispenserCassetteStatus.EMPTY)
                    {
                        var cassetteStatus = new CassetteStatus();
                        cassetteStatus.unitId = mediaUnit.Id;
                        cassetteStatus.status = mediaUnit.Status.ToUpper();
                        dispenserStatuses.Add(cassetteStatus);
                    }

                }
            }

            return dispenserStatuses;
        }

        public int[] GetDenominationBreakDown(List<Denomination> denom)
		{
			int cassCount = GetCassettesCount();
			int[] notesCount = new int[cassCount];
			int x = 0;
			for (int i = 0; i < cassCount; i++)
			{
				if (cassCount > denom.Count && i < (cassCount - denom.Count))
				{
					notesCount[i] = 0;
				}
				else
				{
					notesCount[i] = denom[x++].Count;
				}
			}

			return notesCount;
		}

		public int GetCassettesCount()
		{
			string defaultCurrency = GetDefaultCurrency();

			var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
			return denominateOperation.GetCasssettesCount();
		}

		public List<List<Denomination>> GetDenominations(int highest, int sum, int goal, List<int> notes = null)
		{
			var denoms = new List<List<Denomination>>();
			if (goal > GetAvailableCashAmount())
			{
				throw new DenominateException("Max amount reached.");
			}

			var defaultCurrency = GetDefaultCurrency();
			var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
			notes = new List<int>();

			var availableNotes = GetCessettes();

			denominateOperation.Cassettes.Reverse();
			denominateOperation.GetDenominations(highest, sum, goal, notes);
			denominateOperation.Cassettes.Reverse();
			denominateOperation.Denominations?.Reverse();

			if (denominateOperation?.Denominations != null)
			{
				foreach (var item in denominateOperation.Denominations)
				{
					item.Reverse();
					denoms.Add(item);
				}
			}

			var denList = new List<Denomination>();

			foreach (var item in availableNotes)
			{
				var den = new Denomination() { Value = item.Value, Count = item.Count, Amount = item.Count * item.Value };
				denList.Add(den);
			}

			if (denList?.Count > 0)
				denoms.Add(denList);

			if (denominateOperation.Denominations == null || denoms.Count == 0)
			{
				throw new Exception("Denominations not found.");
			}

			return denoms;
		}

		public async Task PresentCashAndWaitTakenAsync(int amount, int[] notesCount)
		{
			GuideLight?.TurnOn();

			await DispenseCashOperation.StartAsync(() =>
			{
				var defaultCurrency = GetDefaultCurrency();

				object[] deviceNotesCount = notesCount.Cast<object>().ToArray();
				Logger.Info($"Dispense : {string.Join(", ", deviceNotesCount)}");
				var res = ax.Dispense(MixAlgorithm.None, defaultCurrency, 0, deviceNotesCount, 0, Timeout.Infinite);
				if (res == 0) { Journal.CashDispenserNotesDispensed(notesCount); }

				return res;
			});

			try
			{
				await PlayBeepAsync();
				await PresentCashAndWaitTakenOperation.StartAsync(() => ax.Present(Timeout.AwaitTaken));
			}
			catch (DeviceTimeoutException)
			{
				Journal.CashDispenserNotesNotTaken();
				throw;
			}
			finally
			{
				await StopBeepAsync();
			}
		}

		public List<Interface.Entities.CassetteInfo> GetCessettes()
		{
			string defaultCurrency = GetDefaultCurrency();
			var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
			var list = denominateOperation.GetCessettes();
			var newList = new List<Interface.Entities.CassetteInfo>();

			foreach (var item in list)
			{
				newList.Add(new Interface.Entities.CassetteInfo(item.Value, item.Count, item.Index, item.Type));
			}

			return newList;
		}

		public async Task PresentCashAndWaitTakenAsync(int amount)
		{
			GuideLight?.TurnOn();

			await DispenseCashOperation.StartAsync(() =>
			{
				var defaultCurrency = GetDefaultCurrency();
				var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
				int[] notesCount = denominateOperation.Execute(amount);

				Logger.Info($"[{this}]: amount {amount} denominated as {string.Join(",", notesCount)}.");

				object[] deviceNotesCount = notesCount.Cast<object>().ToArray();
				Logger.Info($"Dispense : {string.Join(", ", deviceNotesCount)}");

				var res = ax.Dispense(MixAlgorithm.None, defaultCurrency, 0, deviceNotesCount, 0, Timeout.Infinite);
				if (res == 0) { Journal.CashDispenserNotesDispensed(notesCount); }

				return res;
			});
			try
			{
				await PresentCashAndWaitTakenOperation.StartAsync(() => ax.Present(Timeout.AwaitTaken));
			}
			catch (DeviceTimeoutException)
			{
				Journal.CashDispenserNotesNotTaken();
				throw;
			}
		}

		public async Task<bool> RetractCashAsync()
		{
			JournalCashCassetteStatuses();

			try
			{
				return await RetractCashOperation.StartAsync(() => ax.Retract((int)CashDispenserRetractArea.REJECT));
			}
			finally
			{
				JournalCashCassetteStatuses();
			}
		}

		public override Task ResetAsync()
		{
			return ResetOperation.StartAsync(() => ax.Reset((short)CashAcceptorRetractArea.Retract));
		}

		public override Task TestAsync()
		{
			return TestOperation.StartAsync(() => ax.TestCashUnit(1, CashDispenserRetractArea.RETRACT.ToString(),
																		CashDispenserOutputPosition.POSNULL.ToString()));
		}

		public int GetAvailableCashAmount()
		{
			MediaUnit[] cashCassettes = GetMediaInfo();

			var availableCashAmount = (
				from cashCassette in cashCassettes
				where cashCassette.Type.Equals(DispenserUnitType.BillCassette, StringComparison.OrdinalIgnoreCase)
				select cashCassette.Count * cashCassette.Value).Sum();

			return availableCashAmount;
		}

        public void CheckCassetteStatus()
        {
            var cassettes = GetMediaInfo();

            foreach (var cassette in cassettes)
            {
                Logger?.Info($"Cassette Status={cassette.Status} Cassette UnitStatus={cassette.UnitStatus} AndCassetteType:{cassette.Type} ");
            }

            //for (var i = 0; i < ax.NumberOfLogicalUnits; i++)
            //{

                Logger?.Info($"UnitStatus Status={ax.UnitStatus}");

                
            ///}

        }
        #endregion

        #region Private Functions & Events

        private void Ax_RejectComplete(object sender, EventArgs e)
		{
			Journal.CashDispenserNotesRejected();
		}

		private void Ax_TestCashUnitComplete(object sender, EventArgs e)
		{
			TestOperation.Stop(true);
		}

		private void Ax_RetractComplete(object sender, EventArgs e)
		{
			GuideLight?.TurnOff();

			Journal.CashDispenserNotesRetracted();
			RetractCashOperation.Stop(true);
		}

		private void Ax_ItemsTaken(object sender, EventArgs e)
		{
			OnUserAction();
			GuideLight?.TurnOff();

			Journal.CashDispenserNotesTaken();
			JournalCashCassetteStatuses();
			PresentCashAndWaitTakenOperation.Stop(true);
			RetractCashOperation.Stop(false);

			WriteDevicePropertiesToLog();
		}

		private void Ax_PresentComplete(object sender, EventArgs e)
		{
			Journal.CashDispenserNotesPresented();
		}

		private void Ax_DispenseComplete(object sender, EventArgs e)
		{
			DispenseCashOperation.Stop(true);
		}

		private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
		}

		private void Ax_Timeout(object sender, EventArgs e)
		{
			OnError(new DeviceTimeoutException(GetType().Name));

			WriteDevicePropertiesToLog();
		}

		private void Ax_NotDispensable(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_NotDispensable)));
		}

		private void Ax_CashUnitError(object sender, _DNXCashDispenserXEvents_CashUnitErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_CashUnitError), e.unitNumber));
		}

		private void Ax_FatalError(object sender, _DNXCashDispenserXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXCashDispenserXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_StackerStatusChanged(object sender, _DNXCashDispenserXEvents_StackerStatusChangedEvent e)
		{
			if (e.value == NotEmpty || e.value == NotEmptyWithCustomerAccess)
			{
				Journal.CashDispenserNotesStacked();
			}
		}

		private void Ax_PositionStatusChanged(object sender, _DNXCashDispenserXEvents_PositionStatusChangedEvent e)
		{
            //Logger.Info($"CashDispenser MediaChanged?.Invoked.");
            MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private void Ax_DevicePositionChanged(object sender, _DNXCashDispenserXEvents_DevicePositionChangedEvent e)
		{
			MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private void Ax_CashUnitChanged(object sender, _DNXCashDispenserXEvents_CashUnitChangedEvent e)
		{
            //ax.SetCashUnitInfo();
           // var logicalUnit = ax.get_LogicalUnit(e.unitNumber);
            MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private string GetDefaultCurrency()
		{
			MediaUnit[] cashCassettes = GetMediaInfo();

			MediaUnit firstCashOutCassette = cashCassettes.
				FirstOrDefault(c => c.Type.Equals(DispenserUnitType.BillCassette, StringComparison.OrdinalIgnoreCase));

			return firstCashOutCassette?.Currency ?? TerminalConfiguration.Section?.Currency;
		}

		private void JournalCashCassetteStatuses()
		{
			var cashCassetteStatuses = GetMediaInfo().Select(c => new CashCassetteStatusDto(c.Value, c.Currency, c.InitialCount, c.RejectedCount, c.Count));
			Journal.CashDispenserCassetteStatuses(cashCassetteStatuses);
		}

		private void WriteDevicePropertiesToLog()
		{
			Logger.Properties(new
			{
				ax.NumberOfCurrency,
				ax.NumberOfLogicalUnits,
				ax.NumberOfPhysicalUnits,
				ax.NumberOfMixAlgorithm,
				PhysicalCount = string.Join(",", (object[])ax.PhysicalCount),
				PhysicalDispensedCount = string.Join(",", (object[])ax.PhysicalDispensedCount),
				PhysicalInitialCount = string.Join(",", (object[])ax.PhysicalInitialCount),
				PhysicalPresentedCount = string.Join(",", (object[])ax.PhysicalPresentedCount),
				PhysicalRejectCount = string.Join(",", (object[])ax.PhysicalRejectCount),
				PhysicalRetractedCount = string.Join(",", (object[])ax.PhysicalRetractedCount),
				UnitCount = string.Join(",", (object[])ax.UnitCount),
				UnitDispensedCount = string.Join(",", (object[])ax.UnitDispensedCount),
				UnitRetractedCount = string.Join(",", (object[])ax.UnitRetractedCount),
				UnitRejectCount = string.Join(",", (object[])ax.UnitRejectCount),
				UnitNumber = string.Join(",", (object[])ax.UnitNumber),
				UnitPUNumber = string.Join(",", (object[])ax.UnitPUNumber)
			});
		}

		#endregion
	}
}