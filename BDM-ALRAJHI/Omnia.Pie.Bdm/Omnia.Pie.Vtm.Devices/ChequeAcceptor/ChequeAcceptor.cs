namespace Omnia.Pie.Vtm.Devices.ChequeAcceptor
{
	using AxNXItemProcessorXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Logger;
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public class ChequeAcceptor : Device, IChequeAcceptor
	{
		AxNXItemProcessorX ax;
		readonly string scannedChequeLocation = ConfigurationManager.AppSettings["ScannedChequeLocation"];
		readonly int maxChequesAllowed = 20;
		readonly List<Cheque> _cheques = new List<Cheque>();

		internal readonly DeviceOperation<Cheque[]> AcceptChequesOperation;
		internal readonly DeviceOperation<bool> RollbackChequesOperation;
		internal readonly DeviceOperation<bool> CancelAcceptChequesOperation;
		internal readonly DeviceOperation<bool> RetractChequesOperation;
		internal readonly DeviceOperation<bool> SetMediaInfoOperation;
		internal readonly DeviceOperation<bool> SetDestinationOperation;
		internal readonly DeviceOperation<bool> MediaInEndOperation;
		internal readonly DeviceOperation<bool> PrintTextOperation;

		public Cheque[] Cheques { get; private set; }
		public event EventHandler MediaChanged;
		public bool HasMediaInserted { get; private set; }

		public ChequeAcceptor(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				AcceptChequesOperation = new DeviceOperation<Cheque[]>(nameof(AcceptChequesOperation), logger, journal),
				CancelAcceptChequesOperation = new DeviceOperation<bool>(nameof(CancelAcceptChequesOperation), logger, journal),
				RollbackChequesOperation = new DeviceOperation<bool>(nameof(RollbackChequesOperation), logger, journal),
				RetractChequesOperation = new DeviceOperation<bool>(nameof(RetractChequesOperation), logger, journal),
				SetMediaInfoOperation = new DeviceOperation<bool>(nameof(SetMediaInfoOperation), logger, journal),
				SetDestinationOperation = new DeviceOperation<bool>(nameof(SetDestinationOperation), logger, journal),
				MediaInEndOperation = new DeviceOperation<bool>(nameof(MediaInEndOperation), logger, journal),
				PrintTextOperation = new DeviceOperation<bool>(nameof(PrintTextOperation), logger, journal)
			});

			Logger.Info("ChequeAcceptor Initialized");
		}

		#region Overridden Functions

		protected override IGuideLight GuideLight
		{
			get
			{
				return GuideLights.ChequeAcceptor;
			}
		}

		protected override AxHost CreateAx()
		{
			return ax = new AxNXItemProcessorX();
		}

		protected override void OnInitialized()
		{
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.Timeout += Ax_Timeout;
			ax.MediaData += Ax_MediaData;
			ax.MediaRefused += Ax_MediaRefused;
			ax.MediaTaken += Ax_MediaTaken;
			ax.MediaInComplete += Ax_MediaInComplete;
			ax.ResetComplete += Ax_ResetComplete;
			ax.RetractMediaComplete += Ax_RetractMediaComplete;
			ax.MediaInRollbackComplete += Ax_MediaInRollbackComplete;
			ax.MediaInEndComplete += Ax_MediaInEndComplete;
			ax.SetMediaBinInfoComplete += Ax_SetMediaBinInfoComplete;
			ax.NoMedia += Ax_NoMedia;
			ax.MediaInserted += Ax_MediaInserted;
			ax.MediaRejected += Ax_MediaRejected;
			ax.ScannerThreshold += Ax_ScannerThreshold;
			ax.MICRThreshold += Ax_MICRThreshold;
			ax.SetDestinationComplete += Ax_SetDestinationComplete;
			ax.MediaBinFull += Ax_MediaBinFull;
			ax.InvalidBin += Ax_InvalidBin;
			ax.InvalidMediaID += Ax_InvalidMediaID;
			ax.NoMediaPresent += Ax_NoMediaPresent;
			ax.SequenceInvalid += Ax_SequenceInvalid;
			ax.NoBin += Ax_NoBin;
			ax.PrintTextComplete += Ax_PrintTextComplete;
			ax.MediaBinThreshold += Ax_MediaBinThreshold;
			ax.TonerThreshold += Ax_TonerThreshold;
			ax.InkThreshold += Ax_InkThreshold;
			ax.MediaBinError += Ax_MediaBinError;
			ax.MediaInCancelled += Ax_MediaInCancelled;

			if (string.IsNullOrEmpty(scannedChequeLocation))
			{
				Logger.Error($"ScannedChequeLocation is not set in the configuration : {scannedChequeLocation}");
				throw new InvalidOperationException($"ChequeAcceptor => Scanned cheque location is not set.");
			}
			else
			{
				Logger.Info($"ScannedChequeLocation existance check : {scannedChequeLocation}");

				if (!Directory.Exists(scannedChequeLocation))
				{
					Logger.Info($"ScannedChequeLocation does not exist so create it. : {scannedChequeLocation}");
					Directory.CreateDirectory(scannedChequeLocation);
				}
			}

			new EventsLogger(Logger, ax).StartEventsLogging();
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
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.MediaData -= Ax_MediaData;
			ax.MediaRefused -= Ax_MediaRefused;
			ax.MediaInComplete -= Ax_MediaInComplete;
			ax.MediaTaken -= Ax_MediaTaken;
			ax.ResetComplete -= Ax_ResetComplete;
			ax.RetractMediaComplete -= Ax_RetractMediaComplete;
			ax.MediaInRollbackComplete -= Ax_MediaInRollbackComplete;
			ax.MediaInEndComplete -= Ax_MediaInEndComplete;
			ax.SetMediaBinInfoComplete -= Ax_SetMediaBinInfoComplete;
			ax.NoMedia -= Ax_NoMedia;
			ax.MediaInserted -= Ax_MediaInserted;
			ax.MediaRejected -= Ax_MediaRejected;
			ax.ScannerThreshold -= Ax_ScannerThreshold;
			ax.MICRThreshold -= Ax_MICRThreshold;
			ax.SetDestinationComplete -= Ax_SetDestinationComplete;
			ax.MediaBinFull -= Ax_MediaBinFull;
			ax.InvalidBin -= Ax_InvalidBin;
			ax.InvalidMediaID -= Ax_InvalidMediaID;
			ax.NoMediaPresent -= Ax_NoMediaPresent;
			ax.SequenceInvalid -= Ax_SequenceInvalid;
			ax.NoBin -= Ax_NoBin;
			ax.PrintTextComplete -= Ax_PrintTextComplete;
			ax.MediaBinThreshold -= Ax_MediaBinThreshold;
			ax.TonerThreshold -= Ax_TonerThreshold;
			ax.InkThreshold -= Ax_InkThreshold;
			ax.MediaBinError -= Ax_MediaBinError;
			ax.MediaInCancelled -= Ax_MediaInCancelled;
		}

		#endregion

		#region Public Functions

		public async Task<Cheque[]> AcceptChequesAsync()
		{
			GuideLight.TurnOn();
			return await AcceptChequesOperation.StartAsync(() =>
			{
				_cheques.Clear();
				ax.ClearMediaInImageRequest();

				ax.AddMediaInImageRequest("FRONTIMAGE", "JPG", "GRAYSCALE", "DEFAULT", scannedChequeLocation);
				ax.AddMediaInImageRequest("BACKIMAGE", "JPG", "GRAYSCALE", "DEFAULT", scannedChequeLocation);

				return ax.MediaIn("E13B", (short)maxChequesAllowed, 0, Timeout.Insert);
			});
		}

		public Task CancelAcceptChequesAsync()
		{
			GuideLight.TurnOff();
			return CancelAcceptChequesOperation.StartAsync(() => ax.CancelMediaIn());
		}

		public async Task StoreChequesAsync()
		{
			Logger.Info($"StoreChequeOperation => Execute() cheques count : {_cheques.Count}");

			foreach (var i in Cheques)
			{
				Logger.Info($"StoreChequeOperation => Execute() cheque.MICR : {i.Micr}");

				var bin = GetBinByType(i.MicrAvailable ? ChequeAcceptorBinType.MEDIAIN : ChequeAcceptorBinType.RETRACT);

				Logger.Info($"StoreChequeOperation => Execute() cheque.MediaId : {i.MediaId} cheque.MICR : {i.Micr} set destination {bin.Number}.");

				await SetDestinationOperation.StartAsync(() => ax.SetDestination((short)i.MediaId, (short)bin.Number));

				try
				{
					await PrintTextOperation.StartAsync(() => ax.PrintText((short)i.MediaId, 0, i.TextToPrint));
				}
				catch (Exception ex)
				{
					Journal.Write(ex.GetBaseException().Message);
					Logger.Info(ex.GetBaseException().Message);
				}
			}

			await MediaInEndOperation.StartAsync(() => ax.MediaInEnd(Timeout.AwaitTaken));
		}

		public async Task RollbackChequesAsync()
		{
			GuideLight.TurnOn();
			await RollbackChequesOperation.StartAsync(() => ax.MediaInRollback(Timeout.AwaitTaken));
			HasMediaInserted = false;
		}

		public async Task<bool> RetractChequesAsync()
		{
			bool chequesWereRetracted = await RetractChequesOperation.StartAsync(() => ax.RetractMedia(
				ChequeRetractArea.BIN.ToString(),
				(short)GetBinByType(ChequeAcceptorBinType.RETRACT).Number));
			HasMediaInserted = false;
			return chequesWereRetracted;
		}

		public override async Task ResetAsync()
		{
			GuideLight.TurnOff();
			await ResetOperation.StartAsync(() => ax.Reset(
				ChequeAcceptorResetAction.RETRACTTOBIN.ToString(),
				(short)GetBinByType(ChequeAcceptorBinType.RETRACT).Number,
				Timeout.AwaitTaken));
			HasMediaInserted = false;
		}

		public MediaUnit[] GetMediaInfo()
		{
			var x = new List<MediaUnit>();
			var numbers = (dynamic)ax.BinNumber;
			var types = (dynamic)ax.BinType;
			var counts = (dynamic)ax.BinCount;

			for (var i = 0; i < ax.NumOfBinCount; i++)
			{
				x.Add(new MediaUnit
				{
					MediaDevice = this,
					Id = (int)numbers[i],
					Type = types[i],
					Count = counts[i],
					TotalCount = counts[i]
				});
			}

			foreach (var i in x)
				Logger.Properties(i);

			return x.ToArray();
		}

		public async void SetMediaInfo(int[] ids, int[] counts) => await SetMediaInfoOperation.StartAsync(() =>
		{
			if (ids != null)
			{
				foreach (var item in ids)
				{
					var bin = ax.BinNumber as object[];
					for (int i = 0; i < bin?.Length; i++)
					{
						if ((short)bin[i] == item)
						{
							object[] count = (object[])ax.BinCount;
							count[i] = (short)0;

							ax.BinMediaInCount = count;
							ax.BinCount = count;
							ax.BinRetractOperations = count;
						}
					}
				}
			}
			else
			{
				object[] x = new object[ax.NumOfBinCount];

				for (int i = 0; i < x.Length; i++)
				{
					x[i] = (short)0;
				}

				ax.BinMediaInCount = x as object;
				ax.BinCount = x as object;
				ax.BinRetractOperations = x as object;
			}

			return ax.SetMediaBinInfo();
		});

		public int GetMediaCount(string binID, OperationType opType)
		{
			int mediaCount = 0;
			for (int i = 0; i < (ax.BinID as object[]).Length; i++)
			{
				if ((ax.BinID as object[])[i].ToString() == binID)
				{
					if (opType == OperationType.MaxCount)
						mediaCount = (int)(ax.BinMaxItems as object[])[i];
					else
						mediaCount = (int)(ax.BinMediaInCount as object[])[i];
				}
			}

			return mediaCount;
		}

		public int GetMediaCount(OperationType opType)
		{
			int mediaCount = 0;
			for (int i = 0; i < (ax.BinID as object[]).Length; i++)
			{
				if (opType == OperationType.MaxCount)
					mediaCount = (int)(ax.BinMaxItems as object[])[i];
				else
					mediaCount = (int)(ax.BinMediaInCount as object[])[i];
			}

			return mediaCount;
		}

		#endregion

		#region Private Functions & Events

		private void Ax_MediaInCancelled(object sender, EventArgs e)
		{
			AcceptChequesOperation.Stop(new DeviceOperationCanceledException(nameof(AcceptChequesOperation)));
			CancelAcceptChequesOperation.Stop(true);
		}

		private void Ax_MediaInserted(object sender, EventArgs e)
		{
			GuideLight.TurnOff();
			OnUserAction();
			HasMediaInserted = true;
			Journal.ChequeAcceptorChequesInserted();
		}

		private void Ax_NoMedia(object sender, EventArgs e)
		{
			HasMediaInserted = false;
		}

		private void Ax_SetMediaBinInfoComplete(object sender, EventArgs e)
		{
			SetMediaInfoOperation.Stop(true);
		}

		private void Ax_PrintTextComplete(object sender, EventArgs e)
		{
			PrintTextOperation.Stop(true);
		}

		private void Ax_MediaInEndComplete(object sender, EventArgs e)
		{
			MediaInEndOperation.Stop(true);
			Cheques = null;
			HasMediaInserted = false;
			Journal.ChequeAcceptorChequesDeposited();
		}

		private void Ax_MediaInRollbackComplete(object sender, EventArgs e)
		{
			Journal.ChequeAcceptorDepositCanceled();
			if (!HasMediaInserted)
			{
				// If there're no cheques, rollback operation should not wait until customer collects them, so rollback operation should be stopped on rollback complete event
				RollbackChequesOperation.Stop(false);
			}
		}

		private void Ax_MediaTaken(object sender, _DNXItemProcessorXEvents_MediaTakenEvent e)
		{
			GuideLight.TurnOff();
			OnUserAction();
			RollbackChequesOperation.Stop(true);
			RetractChequesOperation.Stop(false);
			Journal.ChequeAcceptorChequesTaken();
		}

		private void Ax_MediaInComplete(object sender, EventArgs e)
		{
			Cheques = _cheques.ToArray();
			AcceptChequesOperation.Stop(Cheques);
			Journal.ChequeAcceptorInsertCompleted();
		}

		private void Ax_MediaData(object sender, _DNXItemProcessorXEvents_MediaDataEvent e)
		{
			_cheques.AddRange(GetCheques(e.mediaID));
		}

		private void Ax_RetractMediaComplete(object sender, _DNXItemProcessorXEvents_RetractMediaCompleteEvent e)
		{
			GuideLight.TurnOff();
			Journal.ChequeAcceptorChequesRetracted();
			RetractChequesOperation.Stop(true);
		}

		private void Ax_ResetComplete(object sender, EventArgs e)
		{
			ResetOperation.Stop(true);
		}

		private void Ax_SetDestinationComplete(object sender, EventArgs e)
		{
			SetDestinationOperation.Stop(true);
		}

		private void Ax_MediaRefused(object sender, _DNXItemProcessorXEvents_MediaRefusedEvent e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_MediaRefused)));
		}

		private void Ax_MediaBinError(object sender, _DNXItemProcessorXEvents_MediaBinErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_MediaBinError)));
		}

		private void Ax_InkThreshold(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_InkThreshold)));
		}

		private void Ax_TonerThreshold(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_TonerThreshold)));
		}

		private void Ax_MediaBinThreshold(object sender, _DNXItemProcessorXEvents_MediaBinThresholdEvent e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_MediaBinThreshold)));
		}

		private void Ax_NoBin(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_NoBin)));
		}

		private void Ax_SequenceInvalid(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_SequenceInvalid)));
		}

		private void Ax_NoMediaPresent(object sender, EventArgs e)
		{
			OnError(new DeviceTimeoutException(nameof(Ax_NoMediaPresent)));
		}

		private void Ax_InvalidMediaID(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_InvalidMediaID)));
		}

		private void Ax_InvalidBin(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_InvalidBin)));
		}

		private void Ax_MediaBinFull(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_MediaBinFull)));
		}

		private void Ax_FatalError(object sender, _DNXItemProcessorXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXItemProcessorXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_Timeout(object sender, EventArgs e)
		{
			OnError(new DeviceTimeoutException(nameof(Ax_Timeout)));
		}

		private void Ax_MediaRejected(object sender, _DNXItemProcessorXEvents_MediaRejectedEvent e)
		{
			OnError(new DeviceMalfunctionException(e.reason));
		}

		private void Ax_MICRThreshold(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_MICRThreshold)));
		}

		private void Ax_ScannerThreshold(object sender, _DNXItemProcessorXEvents_ScannerThresholdEvent e)
		{
			OnError(new DeviceMalfunctionException(e.scanner));
		}

		/// <summary>
		/// Extracts cheque number from MICR string (which in general includes cheque number, routung number and account number).
		/// </summary>
		/// <param name="micr">MICR string.</param>
		/// <returns>Cheque number parsed from the MICR string. If parsing didn't succeed, returns null.</returns>
		private string GetChequeNumber(string micr)
		{
			// Cheque number is preceded by "c" character
			// However, some MICRs include "dc" characters in routing number, so we use rule that "c" is not preseded by "d" to distinguish cheque number from such routing numbers
			var chequeNumberMatch = Regex.Match(micr, @"([^d]|^)c(?<chequeNumber>\d+)");
			if (chequeNumberMatch.Groups["chequeNumber"].Success)
			{
				string chequeNumber = chequeNumberMatch.Groups["chequeNumber"].Value;
				Logger.Info($"[{this}]: parsed cheque number [{chequeNumber}] from MICR [{micr}].");
				return chequeNumber;
			}
			else
			{
				Logger.Info($"[{this}]: can't find cheque number in MICR text [{micr}].");
				return null;
			}
		}

		private string GetFromAccountNumber(string micr)
		{
			Logger.Info($"[{this}]: Micr : [{micr}].");

			var chequeMatch = Regex.Matches(micr, @"([\d])(\d+)");

			Logger.Info($"[{this}]: Regex.Matches Count : [{chequeMatch.Count}].");

			if (chequeMatch.Count == 3)
			{
				Logger.Info($"[{this}]: Parsed from account [{chequeMatch[2].Value}].");
				return chequeMatch[2].Value;
			}
			else
			{
				Logger.Info($"[{this}]: can't find From account number in MICR text [{micr}].");
				return null;
			}
		}

		private string GetRoutingCodeNumber(string micr)
		{
			Logger.Info($"[{this}]: Micr : [{micr}].");

			var chequeMatch = Regex.Matches(micr, @"([\d])(\d+)");

			Logger.Info($"[{this}]: Regex.Matches Count : [{chequeMatch.Count}].");

			if (chequeMatch.Count == 3)
			{
				Logger.Info($"[{this}]: Parsed routing code [{chequeMatch[1].Value}].");
				return chequeMatch[1].Value;
			}
			else
			{
				Logger.Info($"[{this}]: can't find routing code in MICR text [{micr}].");
				return null;
			}
		}

		private ChequeAcceptorBin GetBinByType(ChequeAcceptorBinType binType)
		{
			var bin = ax.GetBins().FirstOrDefault(ii => ii.Type == binType);
			if (bin == null)
			{
				throw new DeviceMalfunctionException($"No cheque acceptor bin: {binType}");
			}

			return bin;
		}

		private IEnumerable<Cheque> GetCheques(int mediaId)
		{
			ax.GetMediaInfo((short)mediaId);

			Logger.Info($"{nameof(ax.MediaMagneticReadIndicator)} : {ax.MediaMagneticReadIndicator}");
			Logger.Info($"{nameof(ax.MediaCodeLineData)} : {ax.MediaCodeLineData}");

			if (ax.MediaMagneticReadIndicator == "MICR")
			{
				Logger.Info($"AcceptChequeOperation => _itemProcessor_MediaInComplete() media {mediaId} is MICR so it'll be stored to Bin.");
			}
			else if (ax.MediaMagneticReadIndicator == "NO_MICR")
			{
				Logger.Info($"AcceptChequeOperation => _itemProcessor_MediaInComplete() media {mediaId} is NO_MICR so it'll be ejected when store operation will be called.");
				throw new DeviceDataValidationExeption("NO_MICR");
			}

			var mediaImages = ax.MediaImageFile as object[];
			if (mediaImages != null)
			{
				for (int i = 0; i < mediaImages.Length; i++)
				{
					if (i % 2 == 0)
					{
						var micr = MicrExtender.GetMicr(ax.MediaCodeLineData);
						var cheque = new Cheque()
						{
							MediaId = mediaId,
							FrontImage = BitmapExtender.LoadImageAsJpeg(mediaImages[i]?.ToString()),
							BackImage = BitmapExtender.LoadImageAsJpeg(mediaImages[i + 1]?.ToString()),
							ValidationImage = BitmapExtender.LoadImageAsJpeg($"{scannedChequeLocation}UV_IR_Image_{i + 1}.jpg"),
							Micr = micr,
							FromAccount = GetFromAccountNumber(micr),
							ChequeNumber = GetChequeNumber(micr),
							RoutingCode = GetRoutingCodeNumber(micr),
						};

						yield return cheque;
					}
				}
			}
		}

		#endregion
	}

	public static class ItemProcessorExtensions
	{
		public static IEnumerable<ChequeAcceptorBin> GetBins(this AxNXItemProcessorX ax)
		{
			var binNumber = (object[])ax.BinNumber;
			for (var i = 0; i < binNumber.Length; i++)
			{
				ChequeAcceptorBinType type;
				if (Enum.TryParse((string)((object[])ax.BinType)[i], out type))
					yield return new ChequeAcceptorBin
					{
						Number = (short)binNumber[i],
						Type = type
					};
			}
		}
	}
}