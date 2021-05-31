using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using PIE.ITM.Engine.Stages.Helper;

namespace Omnia.Pie.Vtm.Devices.Console.Devices {
	public class CardReader : Device {
		public CardReader() {
			ReadCardTracks = new Command(
				async () => Card = await cardReader.ReadCardAsync(CardDataType.ISO1 | CardDataType.ISO2 | CardDataType.ISO3),
				() => initialized);
			ReadCardChip = new Command(
				async () => Card = await cardReader.ReadCardAsync(CardDataType.CHIP),
				() => initialized);
			EjectCard = new Command(
				async () => await cardReader.EjectCardAsync(false),
				() => initialized);
		}

		public override IDevice Model => cardReader;
		readonly ICardReader cardReader = new Vtm.Devices.CardReader.CardReader(new CardTypeHelper(), new GuideLights.GuideLights());

		public Command ReadCardTracks { get; }
		public Command ReadCardChip { get; }
		public Command EjectCard { get; }

		Card card;
		public Card Card {
			get {
				return card;
			}
			set {
				card = value;
				OnPropertyChanged();
			}
		}
	}
}
