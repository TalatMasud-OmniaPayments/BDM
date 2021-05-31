using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class ClearChecksViewModel : PageWithPrintViewModel
	{
		public override bool IsEnabled => Context.IsLoggedInMode;
        //public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.StandardCash == true ? true : false);

        private readonly IChequeAcceptor _checkAcceptor = ServiceLocator.Instance.Resolve<IChequeAcceptor>();

		private MediaUnitViewModel[] _cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return _cassettes; }
			set { SetProperty(ref _cassettes, value); }
		}

		public ICommand Clear { get; }
		public ICommand ClearAll { get; }
		public ICommand Print { get; }

		public ClearChecksViewModel()
		{
			_checkAcceptor.MediaChanged += (s, e) => Load();

			Clear = new DelegateCommand<MediaUnitViewModel>(
				async cassette =>
				{
					Context.DisplayProgress = true;
					try
					{
						var cassettes = Cassettes?.Where(i => cassette == null || cassette.Model.Id == i.Model.Id).ToArray();
						_checkAcceptor.SetMediaInfo(cassette == null ? null : new[] { cassette.Model.Id }, null);
						await PrintReceiptAsync(cassettes);
						Load();
					}
					finally
					{
						Context.DisplayProgress = false;
						await _channelManagementService.InsertEventAsync("Clear Checks", "True");
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
						await _channelManagementService.InsertEventAsync("Clear Checks", "True");
					}
				});
		}

		private async Task PrintReceiptAsync(MediaUnitViewModel[] cassettes, bool isView = false)
		{
			await PrintAsync(new ClearChequesReceipt
			{
				IsView = isView,
				Units = cassettes?.Select(
					c => new ChequeUnit
					{
						Name = c.Model.Type,
						Count = c.Model.Count
					}
				).ToList()
			});
		}

		public override void Load()
		{
			Cassettes = _checkAcceptor?.GetMediaInfo().
				Select(i => new MediaUnitViewModel { Model = i }).ToArray();
		}
	}
}