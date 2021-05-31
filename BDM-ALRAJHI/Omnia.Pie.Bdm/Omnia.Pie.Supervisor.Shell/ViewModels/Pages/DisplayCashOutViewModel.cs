
using Microsoft.Practices.Unity;
using System.Windows.Input;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class DisplayCashOutViewModel : PageWithPrintViewModel
	{
		public override bool IsEnabled => Context.IsLoggedInMode;

		private readonly ICashDispenser _cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();


		private MediaUnitViewModel[] cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return cassettes; }
			set { SetProperty(ref cassettes, value); }
		}

		public ICommand Print { get; }


		public DisplayCashOutViewModel()
		{
			_cashDispenser.MediaChanged += (s, e) => Load();

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
						await _channelManagementService.InsertEventAsync("View Cash", "True");
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
