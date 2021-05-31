namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Client.Journal.Interface.Dto;
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Supervisor.Shell.Service;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;

	public class ClearCardsViewModel : PageWithPrintViewModel
	{
		//public override bool IsEnabled => Context.IsLoggedInMode;
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.ClearCards == true ? true : false);


        private readonly ICardReader _cardReader = ServiceLocator.Instance.Resolve<ICardReader>();
		private readonly IRetractedCardStore _retractedCardStore = ServiceLocator.Instance.Resolve<IRetractedCardStore>();

		private MediaUnitViewModel[] _cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return _cassettes; }
			set { SetProperty(ref _cassettes, value); }
		}

		public ICommand Clear { get; }
		public ICommand ClearAll { get; }
		public ICommand Print { get; }

		public ClearCardsViewModel()
		{
			_cardReader.MediaChanged += (s, e) => Load();

			Clear = new DelegateCommand<MediaUnitViewModel>(
				async cassette =>
				{
					Context.DisplayProgress = true;
					try
					{
						var cassettes = Cassettes?.Where(i => cassette == null || cassette.Model.Id == i.Model.Id).ToArray();
						_cardReader.SetMediaInfo(cassette == null ? null : new[] { cassette.Model.Id }, null);
						await PrintReceiptAsync(cassettes);
						JournalPrintRetractedCardsCleared();
						Load();
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Clear Cards", "True");
					}
				});

			ClearAll = new DelegateCommand(() => Clear.Execute(null));

			Print = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;
					try
					{
						await PrintReceiptAsync(Cassettes, true);
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Clear Cards", "True");
					}
				});
		}

		private async Task PrintReceiptAsync(MediaUnitViewModel[] cassettes, bool isView = false)
		{
			await PrintAsync(new ClearCardsReceipt
			{
				IsView = isView,
				Units = cassettes?.Select(
					c => new CardUnit
					{
						Name = c.Model.Type,
						Count = c.Model.TotalCount
					}
				).ToList()
			});
		}

		protected async void JournalPrintRetractedCardsCleared()
		{
			var retractedCards = await _retractedCardStore.GetList();
			_journal.RetractedCardsCleared(retractedCards.ConvertAll(c => new RetractedCardDto(c.Retracted, c.MaskedNumber)));
			await _retractedCardStore.ClearAll();
		}

		public override void Load()
		{
			Cassettes = _cardReader?.GetMediaInfo().
				Select(i => new MediaUnitViewModel { Model = i }).ToArray();
		}
	}
}