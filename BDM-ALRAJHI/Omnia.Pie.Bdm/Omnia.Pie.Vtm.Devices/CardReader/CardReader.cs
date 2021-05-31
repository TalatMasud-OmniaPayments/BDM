namespace Omnia.Pie.Vtm.Devices.CardReader
{
	using AxNXCardReaderXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Enum;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;

	public sealed class CardReader : Device, ICardReader
	{
		private Emv.Emv emv;
		readonly IRetractedCardStore retractedCardStore;
		readonly ICardTypeResolver cardTypeResolver;
		AxNXCardReaderX ax;
		bool awaitTaken;
        public event EventHandler<string> CardReaderStatusChanged;

        public CardReader(IGuideLights guideLights, IDeviceErrorStore deviceErrorStore,
							IRetractedCardStore retractedCardStore, ICardTypeResolver cardTypeResolver,
							IJournal journal, ILogger logger) :
			base(deviceErrorStore, logger, journal, guideLights)
		{
			this.retractedCardStore = retractedCardStore;
			this.cardTypeResolver = cardTypeResolver;
			Operations.AddRange(new DeviceOperation[] {
				ReadCardOperation = new DeviceOperation<Card>(nameof(ReadCardOperation), Logger, Journal),
				EjectCardAndWaitTakenOperation = new DeviceOperation<bool>(nameof(EjectCardAndWaitTakenOperation), Logger, Journal),
				RetainCardOperation = new DeviceOperation<bool>(nameof(RetainCardOperation), Logger, Journal),
				ReadChipDataOperation = new ReadChipDataOperation(Logger, Journal, this),
				ChipIOOperation = new DeviceOperation<byte[]>(nameof(ChipIOOperation), Logger, Journal),
				CancelReadCardOperation = new DeviceOperation<bool>(nameof(CancelReadCardOperation), Logger, Journal)
			});

			Logger.Info("CardReader Initialized");
		}

		internal readonly DeviceOperation<Card> ReadCardOperation;
		internal readonly ReadChipDataOperation ReadChipDataOperation;
		internal readonly DeviceOperation<bool> CancelReadCardOperation;
		internal readonly DeviceOperation<bool> EjectCardAndWaitTakenOperation;
		internal readonly DeviceOperation<bool> RetainCardOperation;
		internal readonly DeviceOperation<byte[]> ChipIOOperation;

		public event EventHandler MediaChanged;
		public bool HasMediaInserted { get; private set; }
		public Card Card { get; private set; }
        public bool HasEMVChip { get; private set; }

        #region Overridden Functions

        protected override IGuideLight GuideLight => GuideLights.CardReader;

		protected override AxHost CreateAx()
		{
            Logger.Info("CardReader CreateAx");
            ax = new AxNXCardReaderX();
			emv = new Emv.Emv(DeviceErrorStore, Logger, Journal, GuideLights, ax);
			emv.Initialize();
			return ax;
		}

		protected override void OnInitialized()
		{
            Logger.Info("CardReader OnInitialized");
            ax.MediaInserted += Ax_MediaInserted;
			ax.DeviceError += Ax_DeviceError;
            ax.DeviceStatusChanged += Ax_DeviceStatusChanged;
			ax.FatalError += Ax_FatalError;
			ax.MediaRemoved += Ax_MediaRemoved;
			ax.EjectComplete += Ax_EjectComplete;
			ax.Timeout += Ax_Timeout;
			ax.ReadComplete += Ax_ReadComplete;
			ax.AcceptCancelled += Ax_AcceptCancelled;
			ax.RetainComplete += Ax_RetainComplete;
			ax.ResetComplete += Ax_ResetComplete;
			ax.ChipIOComplete += Ax_ChipIOComplete;
			ax.ChipIOFailure += Ax_ChipIOFailure;
			ax.InvalidMedia += Ax_InvalidMedia;
			ax.InvalidTrackData += Ax_InvalidTrackData;
		}

        

        protected override int OpenSessionSync(int timeout)
		{
            Logger.Info("CardReader OpenSessionSync");
            return ax.OpenSessionSync(timeout);
		}

		protected override int CloseSessionSync()
		{
			return ax.CloseSessionSync();
		}

		protected override string GetDeviceStatus()
		{
			return ax.DeviceStatus;
		}

		protected override void OnDisposing()
		{
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.MediaInserted -= Ax_MediaInserted;
			ax.MediaRemoved -= Ax_MediaRemoved;
			ax.EjectComplete -= Ax_EjectComplete;
			ax.Timeout -= Ax_Timeout;
			ax.ReadComplete -= Ax_ReadComplete;
			ax.AcceptCancelled -= Ax_AcceptCancelled;
			ax.RetainComplete -= Ax_RetainComplete;
			ax.ResetComplete -= Ax_ResetComplete;
			ax.ChipIOComplete -= Ax_ChipIOComplete;
			ax.ChipIOFailure -= Ax_ChipIOFailure;
			ax.InvalidMedia -= Ax_InvalidMedia;
			ax.InvalidTrackData -= Ax_InvalidTrackData;
		}

		#endregion

		#region Public Functions

		public void CancelReadCard()
		{
			if (ReadCardOperation.IsRunning)
				CancelReadCardOperation.Start(() => ax.CancelAccept());
		}

		public async Task<Card> ReadCardAsync(bool readChip)
		{
            Logger.Info($"Execute Task: Read Card");

            GuideLight?.TurnOn();
            await PlayBeepAsync();

            Logger.Info(string.Format("Will Read Chip? : {0}", readChip));

            try
            {
                Card = await ReadCardOperation.StartAsync(() =>
                        ax.ReadAvailableRawData(string.Join(",", CardDataType.ISO1, CardDataType.ISO2,
                                    CardDataType.ISO3, CardDataType.CHIP), Timeout._30Sec)
                );

                HasEMVChip = false;

                if (readChip)
                {
                    try
                    {

                        Logger.Info("Reading the CHIP...");

                        await ReadCardOperation.StartAsync(() =>
                            ax.ReadAvailableRawData(CardDataType.CHIP.ToString(), Timeout.Operation)
                        );

                        Logger.Info("Getting EID Data from the CHIP...");
                        Card.EmiratesId = await ReadChipDataOperation.StartAsync();

                        HasEMVChip = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(new DeviceMalfunctionException($"{GetType().Name}.{nameof(ReadCardAsync)}:", ex));
                    }

                    if (!string.IsNullOrEmpty(Card?.EmiratesId?.Id))
                        Card.CardType = CardType.EmiratesIdCard;


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await StopBeepAsync();
            }

            if (Card.CardType == CardType.EmiratesIdCard)
            {
                Journal.EmiratesIdCardRead(Card?.EmiratesId?.Id);
            }
            else
            {
                Journal.CardReaderCardRead(Card.CardNumber);
            }

            return Card;
        }

		public bool IsCardInside
		{
			get
			{
				return HasMediaInserted;
			}
		}

		public Task EjectCardAndWaitTakenAsync() => EjectCardAsync(true);

		public async Task RetainCardAsync()
		{
			await RetainCardOperation.StartAsync(() =>
			{
				return ax.RetainMedia();
			});
			HasMediaInserted = false;
			Journal.CardReaderCardCaptured();

			await retractedCardStore.Save(new RetractedCard
			{
				Retracted = DateTime.Now,
				MaskedNumber = Card?.CardNumber?.ToMaskedCardNumber() ?? "N/A"
			});

			Card = null;
		}

		public override async Task ResetAsync()
		{
			await ResetOperation.StartAsync(() => ax.Reset(CardReaderResetAction.RETAIN.ToString()));
		}

		public MediaUnit[] GetMediaInfo()
		{
			var x = new[] {new MediaUnit {
				MediaDevice = this,
				Id = 1,
				Type = "RETRACT",
				Count = ax.RetainCount,
				TotalCount = ax.RetainCount
			}};

			foreach (var i in x)
			{
				Logger.Properties(i);
			}

			return x;
		}

		public void SetMediaInfo(int[] ids, int[] counts)
		{
			try
			{
				ax.ResetRetainCount();
			}
			catch (Exception ex)
			{
				Logger.Exception(new DeviceMalfunctionException($"{GetType().Name}.{nameof(SetMediaInfo)}:", ex));
				throw;
			}
		}

		public short GetMaxRetainCount()
		{
			return ax.MaxRetainCount;
		}

		public short GetRetainCount()
		{
			return ax.RetainCount;
		}

		public Task<IEmvData> GetEmvDataAsync(int amount, string transactionType)
		{
			return emv.GetEmvDataAsync(amount, transactionType);
		}

		#endregion

		#region Private Functions & Events

		private void Ax_ChipIOComplete(object sender, _DNXCardReaderXEvents_ChipIOCompleteEvent e)
		{
			//Logger.Info($"e.token={e.token}");
			//Logger.Info($"e.data={e.data}");
			var data = (dynamic)e.data;
			var result = new byte[data.Length];
			for (int i = 0; i < data.Length; ++i)
				result[i] = (byte)data[i];
			ChipIOOperation.Stop(result);
			//Logger.Info(" Received response from the eida card chip: " + BitConverter.ToString(result));
		}

		private void Ax_ChipIOFailure(object sender, _DNXCardReaderXEvents_ChipIOFailureEvent e)
		{
			OnError(new DeviceMalfunctionException(e.token));
		}

		private void Ax_InvalidTrackData(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_InvalidTrackData)));
		}

		private void Ax_InvalidMedia(object sender, EventArgs e)
		{
			OnError(new DeviceMalfunctionException(nameof(Ax_InvalidMedia)));
		}

		private void Ax_FatalError(object sender, _DNXCardReaderXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXCardReaderXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}
        private void Ax_DeviceStatusChanged(object sender, _DNXCardReaderXEvents_DeviceStatusChangedEvent e)
        {
            Logger.Info($"CardReader status changed{sender.ToString()} AndEvent: {e.value} => Ax_StatusChanged");
            CardReaderStatusChanged?.Invoke(sender, e.value);
        }

        private void Ax_Timeout(object sender, EventArgs e)
		{
			Journal.CardNotTaken();
			OnError(new DeviceTimeoutException(nameof(CardReader)));
		}

		private void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);

		private void Ax_AcceptCancelled(object sender, EventArgs e)
		{
			try
			{
				ReadCardOperation.Stop(new DeviceOperationCanceledException(nameof(ReadCardOperation)));
			}
			finally
			{
				GuideLight?.TurnOff();
			}
		}

		private void Ax_ReadComplete(object sender, EventArgs e)
		{
			Card card = ParseIso();
			ReadCardOperation.Stop(card);
		}

		private void Ax_RetainComplete(object sender, EventArgs e)
		{
			RetainCardOperation.Stop(true);
            Logger.Info($"{GetType()} => Card Retained");
        }

		private void Ax_EjectComplete(object sender, EventArgs e)
		{
			if (!awaitTaken)
			{
				try
				{
					Journal.CardReaderCardEjected();
					EjectCardAndWaitTakenOperation.Stop(true);
                    Logger.Info($"{GetType()} => Card Ejected");
                }
				finally
				{
					awaitTaken = false;
				}
			}
		}

		private void Ax_MediaRemoved(object sender, EventArgs e)
		{
			GuideLight.TurnOff();
			OnUserAction();
			EjectCardAndWaitTakenOperation.Stop(true);
			RetainCardOperation.Stop(false);
			Journal.CardReaderCardTaken();
            Logger.Info($"{GetType()} => Card Removed");
        }

		private void Ax_MediaInserted(object sender, EventArgs e)
		{
			GuideLight.TurnOff();
			OnUserAction();
			HasMediaInserted = true;
			Journal.CardReaderCardInserted();
            Logger.Info($"{GetType()} => Card Inserted");
        }

		private async Task EjectCardAsync(bool awaitTaken)
		{
            Logger.Info($"{GetType()} => Ejecting Card");

            GuideLight?.TurnOn();
			await PlayBeepAsync();

			try
			{
				await EjectCardAndWaitTakenOperation.StartAsync(() =>
				{
					this.awaitTaken = awaitTaken;
					return ax.EjectMedia(Timeout.AwaitTaken);
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				await StopBeepAsync();
			}

			HasMediaInserted = false;
			Card = null;
		}

		private Card ParseIso()
		{
			var card = new Card
			{
				Track1 = ax.Track1Data, // Card Name
				Track2 = ax.Track2Data, // Card Number
				Track3 = ax.Track3Data  // kuch nahi
			};
			if (card.Track1 != null)
			{
				string[] s = card.Track1.Split('^');
				if (s.Length >= 2)
					card.AccountName = s[1];
			}
			if (card.Track2 != null)
			{
				// Some cards are using 'D' instead of '=' as separator
				card.CardNumber = card.Track2.Replace('D', '=').Split('=')[0];
				if (!string.IsNullOrEmpty(card.CardNumber))
					card.CardType = cardTypeResolver.GetCardType(card.CardNumber);
			}
			// Track3 is optional, no need to parse it
			return card;
		}

		#endregion
	}
}