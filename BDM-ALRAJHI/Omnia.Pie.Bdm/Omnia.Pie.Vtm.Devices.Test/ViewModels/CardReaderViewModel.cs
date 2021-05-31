using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class CardReaderViewModel : DeviceViewModel
	{
		public CardReaderViewModel()
		{
			ReadTracks = new OperationViewModel<Card>(() => Model.ReadCardAsync(false))
			{
				Id = nameof(Model.ReadCardAsync)
			};
			ReadTracksAndChip = new OperationViewModel<Card>(() => Model.ReadCardAsync(true))
			{
				Id = nameof(Model.ReadCardAsync)
			};
			ReadTracksAndChipTwice = new OperationViewModel<Card>( async () =>
			{
				await Model.ReadCardAsync(true);
				return await Model.ReadCardAsync(true);
			})
			{
				Id = nameof(Model.ReadCardAsync)
			};
			Cancel = new OperationViewModel(() => Model.CancelReadCard())
			{
				Id = nameof(Model.CancelReadCard)
			};
			EjectCard = new OperationViewModel(() => Model.EjectCardAndWaitTakenAsync())
			{
				Id = nameof(Model.EjectCardAndWaitTakenAsync)
			};
			RetainCard = new OperationViewModel(() => Model.RetainCardAsync())
			{
				Id = nameof(Model.RetainCardAsync)
			};
		}

		new ICardReader Model => (ICardReader)base.Model;
		public bool HasPendingMediaIn => Model.HasMediaInserted;

		public OperationViewModel ReadTracks { get; private set; }
		public OperationViewModel ReadChip { get; private set; }
		public OperationViewModel Cancel { get; private set; }
		public OperationViewModel EjectCard { get; private set; }
		public OperationViewModel RetainCard { get; private set; }
		public OperationViewModel<Card> ReadTracksAndChip { get; private set; }
		public OperationViewModel<Card> ReadTracksAndChipTwice { get; private set; }

		public override void Load()
		{
			base.Load();
			RaisePropertyChanged(nameof(HasPendingMediaIn));
		}
	}
}