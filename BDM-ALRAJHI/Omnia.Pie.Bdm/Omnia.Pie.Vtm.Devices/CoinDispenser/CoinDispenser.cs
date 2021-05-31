namespace Omnia.Pie.Vtm.Devices.CoinDispenser
{
	using AxNXCoinDispenserXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Dto;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public class CoinDispenser : Device, ICoinDispenser
	{
		AxNXCoinDispenserX ax;
		private const string NotEmpty = "NOTEMPTY";
		private const string NotEmptyWithCustomerAccess = "NOTEMPTYCUST";

		internal readonly DeviceOperation<bool> DispenseCoinOperation;
		internal readonly DeviceOperation<bool> SetMediaInfoOperation;

		public event EventHandler MediaChanged;

		public CoinDispenser(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights, IJournal journal)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				DispenseCoinOperation = new DeviceOperation<bool>(nameof(DispenseCoinOperation), Logger, Journal),
				SetMediaInfoOperation = new DeviceOperation<bool>(nameof(SetMediaInfoOperation), Logger, Journal)
			});

			Logger.Info("CoinDispenser Initialized");
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
			ax.RejectComplete += Ax_RejectComplete;
		}

		protected override IGuideLight GuideLight => GuideLights.CoinDispenser;

		protected override AxHost CreateAx()
		{
			return ax = new AxNXCoinDispenserX();
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
			ax.RejectComplete -= Ax_RejectComplete;
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		#endregion

		#region Private Functions & Events

		private void Ax_RejectComplete(object sender, EventArgs e)
		{
			Journal.CoinDispenserCoinsRejected();
		}

		private void Ax_RetractComplete(object sender, EventArgs e)
		{
			GuideLight?.TurnOff();
			Journal.CoinDispenserCoinsRetracted();
			JournalCoinCassetteStatuses();
		}

		private void Ax_ItemsTaken(object sender, EventArgs e)
		{
			OnUserAction();
			GuideLight?.TurnOff();
			Journal.CoinDispenserCoinsTaken();
			JournalCoinCassetteStatuses();
		}

		private void Ax_PresentComplete(object sender, EventArgs e)
		{
			DispenseCoinOperation.Stop(true);
			GuideLight?.TurnOff();
			Journal.CoinDispenserCoinsPresented();
		}

		private void Ax_DispenseComplete(object sender, EventArgs e)
		{
			GuideLight?.TurnOff();
			DispenseCoinOperation.Stop(true);
			WriteDevicePropertiesToLog();
		}

		private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
		}

		private void Ax_Timeout(object sender, EventArgs e)
		{
			OnError(new DeviceTimeoutException(GetType().Name));
		}

		private void Ax_NotDispensable(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_NotDispensable)));
		}

		private void Ax_CashUnitError(object sender, _DNXCoinDispenserXEvents_CashUnitErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_CashUnitError), e.unitNumber));
		}

		private void Ax_FatalError(object sender, _DNXCoinDispenserXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXCoinDispenserXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_StackerStatusChanged(object sender, _DNXCoinDispenserXEvents_StackerStatusChangedEvent e)
		{
			if (e.value == NotEmpty || e.value == NotEmptyWithCustomerAccess)
			{
				//Journal.CashDispenserNotesStacked();
			}
		}

		private void Ax_PositionStatusChanged(object sender, _DNXCoinDispenserXEvents_PositionStatusChangedEvent e)
		{
			MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private void Ax_DevicePositionChanged(object sender, _DNXCoinDispenserXEvents_DevicePositionChangedEvent e)
		{
			MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private void Ax_CashUnitChanged(object sender, _DNXCoinDispenserXEvents_CashUnitChangedEvent e)
		{
			MediaChanged?.Invoke(this, EventArgs.Empty);
		}

		private string GetDefaultCurrency()
		{
			MediaUnit[] cashCassettes = GetMediaInfo();
			MediaUnit firstCashOutCassette = cashCassettes.
				FirstOrDefault(c => c.Type.Equals(DispenserUnitType.CoinCassette, StringComparison.OrdinalIgnoreCase));
			return firstCashOutCassette?.Currency ?? "SAR";
		}

		private void JournalCoinCassetteStatuses()
		{
			var coinCassetteStatuses = GetMediaInfo().Select(c => new CoinCassetteStatusDto(c.Value, c.Currency, c.InitialCount, c.RejectedCount, c.RemainingCount));
			Journal.CoinDispenserCassetteStatuses(coinCassetteStatuses);
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

			var count = 0;
			if (bool.Parse(ConfigurationManager.AppSettings["CoinDispenseDebug"].ToString()))
				count = 1;

			for (var i = 0; i < ax.NumberOfPhysicalUnits - count; i++)
			{
				var mediaUnit = new MediaUnit
				{
					MediaDevice = this,
					Id = (int)numbers[i],
					Type = (string)types[i],
					Currency = (string)currencies[i],
					Value = (int)values[i],
					Status = (string)statuses[i],
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
				if (mediaUnit.Type == DispenserUnitType.CoinCassette)
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

		public async Task PresentCoinAndWaitTakenAsync(int amount)
		{
			Logger.Info($"Dispensing {amount} coins.");
			GuideLight?.TurnOn();

			var defaultCurrency = GetDefaultCurrency();
			int[] coinsCount = new int[ax.NumberOfPhysicalUnits];
			ax.Denominate(MixAlgorithm.Mix, defaultCurrency, amount, coinsCount);

			try
			{
				await DispenseCoinOperation.StartAsync(() =>
				{
					object[] deviceCoinsCount = coinsCount.Cast<object>().ToArray();
					var res = ax.Dispense(MixAlgorithm.Mix, defaultCurrency, amount, deviceCoinsCount, 1, Timeout.Infinite);
					if (res == 0) { Journal.CoinDispenserCoinsDispensed(deviceCoinsCount); }

					return res;
				});
			}
			catch (DeviceTimeoutException)
			{
				Journal.CoinDispenserCoinsNotTaken();
				throw;
			}
		}

		public async Task PresentCoinsAsync(int amount)
		{
			Logger.Info($"Dispensing {amount} coins.");
			GuideLight.TurnOn();

			try
			{
				await DispenseCoinOperation.StartAsync(() =>
				{
					var defaultCurrency = GetDefaultCurrency();
					var denominateOperation = new DenominateOperationWithChange(GetMediaInfo(), defaultCurrency);
					int[] coinsCount = denominateOperation.Execute(amount);

					Logger.Info($"[{this}]: amount {amount} denominated as {string.Join(",", coinsCount)}.");

					object[] deviceCoinsCount = coinsCount.Cast<object>().ToArray();
					Logger.Info($"Dispense : {string.Join(", ", deviceCoinsCount)}");
					var res = ax.Dispense(MixAlgorithm.None, defaultCurrency, 0, deviceCoinsCount, 0, Timeout.Infinite);
					if (res == 0) { Journal.CoinDispenserCoinsDispensed(deviceCoinsCount); }

					return res;
				});
			}
			catch (DeviceTimeoutException)
			{
				Journal.CashDispenserNotesNotTaken();
				throw;
			}
		}

		public override Task ResetAsync()
		{
			return ResetOperation.StartAsync(() => ax.Reset((short)CashAcceptorRetractArea.Retract));
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

		public int GetAvailableCashAmount()
		{
			MediaUnit[] Cassettes = GetMediaInfo();

			int availableAmount = (
				from cassette in Cassettes
				where cassette.Type.Equals(DispenserUnitType.CoinCassette, StringComparison.OrdinalIgnoreCase)
				select cassette.Count * cassette.Value).Sum();

			return availableAmount;
		}

		#endregion
	}
}