namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	using Omnia.Pie.Supervisor.Shell.Service;
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Linq;
	using System.Windows.Input;
	using Omnia.Pie.Vtm.Framework.Interface.Receipts;
	using Omnia.Pie.Vtm.Services.Interface;

	public class AddCoinViewModel : PageWithPrintViewModel
	{
		public override bool IsEnabled => Context.IsLoggedInMode;

		private readonly ICoinDispenser _coinDispenser = ServiceLocator.Instance.Resolve<ICoinDispenser>();
		private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();

		private MediaUnitViewModel[] _cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return _cassettes; }
			set { SetProperty(ref _cassettes, value); }
		}

		public ICommand Clear { get; }
		public ICommand ClearAll { get; }
		public ICommand Print { get; }
		public ICommand Apply { get; }
		public ICommand TestCash { get; }

		public AddCoinViewModel()
		{
			_coinDispenser.MediaChanged += (s, e) => Load();

			Clear = new DelegateCommand<MediaUnitViewModel>(
				async cassette =>
				{
					Context.DisplayProgress = true;
					try
					{
						var cassettes = Cassettes?.Where(i => cassette == null || cassette.Model.Id == i.Model.Id).ToArray();
						_coinDispenser.SetMediaInfo(cassette == null ? null : new[] { cassette.Model.Id }, null);
						await PrintAsync(new ClearAddCoinReceipt
						{
							Units = cassettes?.Select(
								i => new ClearAddCoinUnit
								{
									Name = "CST " + i.Model.Id,
									Currency = i.Model.Currency,
									Denomination = i.Model.Value,
									Count = i.Model.Count
								}
							).ToList()
						});

						Load();
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Clear Add Coin", "True");
					}
				});

			ClearAll = new DelegateCommand(() => Clear.Execute(null));

			Print = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;
					try
					{
						await PrintAsync(new ViewAddCoinReceipt
						{
							Units = Cassettes.Select(i =>
									new CoinOutUnit
									{
										Index = i.Model.Id,
										Currency = i.Model.Currency,
										Denomination = i.Model.Value,
										Count = i.Model.Count,
										Rejected = i.Model.RejectedCount,
										Remaining = i.Model.RemainingCount,
										Dispensed = i.Model.DispensedCount,
										Total = i.Model.TotalCount
									}
							).ToList()
						});
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("View Add Coin", "True");
					}
				});

			Apply = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;

					try
					{
						_coinDispenser?.SetMediaInfo(
							Cassettes?.Select(ii => ii.Model.Id).ToArray(),
							Cassettes?.Select(ii => ii.Model.Count + int.Parse(ii.Added ?? "0")).ToArray()
						);

						await PrintAsync(new AddCoinReceipt
						{
							Units = Cassettes?.Select(i => new AddCoinUnit
							{
								Name = "CST " + i.Model.Id,
								Currency = i.Model.Currency,
								Denomination = i.Model.Value,
								Count = i.Model.Count
							}).ToList()
						});
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Add Coin", "True");

						var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
						var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
						await channelService.SendDeviceStatus(devStatus.GetDevicesStatus(), false, true);
					}
				});
		}

		public override void Load()
		{
			Cassettes = _coinDispenser?.GetMediaInfo().
				Where(i =>
					i.Type != "REJECT" &&
					i.Type != "RETRACT" &&
					i.Type != "REJECTCASSETTE" &&
					i.Type != "RETRACTCASSETTE").
				Select(i => new MediaUnitViewModel { Model = i }).ToArray();
		}
	}
}