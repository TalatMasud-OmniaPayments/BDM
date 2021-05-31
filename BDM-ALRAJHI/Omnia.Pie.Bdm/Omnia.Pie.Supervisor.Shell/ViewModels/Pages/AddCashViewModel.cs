using System;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Services.Interface;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class AddCashViewModel : PageWithPrintViewModel
	{
        public override bool IsEnabled => Context.IsLoggedInMode;
        //public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.ClearCashIn == true ? true : false);


        private readonly ICashDispenser _cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
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

		public AddCashViewModel()
		{
			_cashDispenser.MediaChanged += (s, e) => Load();

			Clear = new DelegateCommand<MediaUnitViewModel>(
				async cassette =>
				{
					Context.DisplayProgress = true;
					try
					{
						var cassettes = Cassettes?.Where(i => cassette == null || cassette.Model.Id == i.Model.Id).ToArray();
						_cashDispenser.SetMediaInfo(cassette == null ? null : new[] { cassette.Model.Id }, null);
						await PrintAsync(new ClearAddCashReceipt
						{
							Units = cassettes?.Select(
								i => new ClearAddCashUnit
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
						await _channelManagementService.InsertEventAsync("Clear Add Cash", "True");
					}
				});

			ClearAll = new DelegateCommand(() => Clear.Execute(null));

			Print = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;
					try
					{
						await PrintAsync(new ViewAddCashReceipt
						{
							Units = Cassettes.Select(i =>
									new CashOutUnit
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
						await _channelManagementService.InsertEventAsync("View Add Cash", "True");
					}
				});

			Apply = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;

					try
					{
						_cashDispenser?.SetMediaInfo(
							Cassettes?.Select(ii => ii.Model.Id).ToArray(),
							Cassettes?.Select(ii => ii.Model.Count + int.Parse(ii.Added ?? "0")).ToArray()
						);

						await PrintAsync(new AddCashReceipt
						{
							Units = Cassettes?.Select(i => new AddCashUnit
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
						await _channelManagementService.InsertEventAsync("Add Cash", "True");

						var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
						var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
						await channelService.SendDeviceStatus(devStatus.GetDevicesStatus(), true, false);
					}
				});

			TestCash = new DelegateCommand(
				async () =>
				{
					Context.DisplayProgress = true;
					try
					{
						await _cashDispenser?.TestAsync();
						await PrintAsync(new TestCashReceipt { IsSuccess = true });
					}
					catch (Exception ex)
					{
						_logger.Exception(ex);
						await PrintAsync(new TestCashReceipt { IsSuccess = false });
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Test Cash", "True");
					}
				});
		}

		public override void Load()
		{
			Cassettes = _cashDispenser?.GetMediaInfo().
				Where(i =>
					i.Type != "REJECT" &&
					i.Type != "RETRACT" &&
					i.Type != "REJECTCASSETTE" &&
					i.Type != "RETRACTCASSETTE").
				Select(i => new MediaUnitViewModel { Model = i }).ToArray();
		}
	}
}
