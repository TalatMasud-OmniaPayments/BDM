using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using System.Linq;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.Configuration;
using System.Configuration;
using System;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using System.Collections.Generic;
using Omnia.Pie.Vtm.Framework.Configurations;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class StandardCashViewModel : PageWithPrintViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.StandardCash == true ? true : false);
        //public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Diagnostic == true ? true : false);

        private readonly ICashDispenser _cashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
		private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();

		public ICommand StandardCash { get; }

		public StandardCashViewModel()
		{
			_cashDispenser.MediaChanged += (s, e) => Load();

			StandardCash = new DelegateCommand(
			async () =>
			{
				Context.DisplayProgress = true;

				try
				{
					SupervisoryConfiguration.SetCassetteElementValues(Cassettes.ToDictionary(
						c => c.Model.Value.ToString(),
						c => c.Added));

					await PrintAsync(new ClearAddCashReceipt
					{
						Units = Cassettes?.Select(
							i => new ClearAddCashUnit
							{
								Name = "CST " + i.Model.Id,
								Currency = i.Model.Currency,
								Denomination = i.Model.Value,
								Count = i.Model.Count
							}).ToList()
					});

					_cashDispenser?.SetMediaInfo(null, null);

					_cashDispenser?.SetMediaInfo(
						Cassettes?.Select(ii => ii.Model.Id).ToArray(),
						Cassettes?.Select(ii => ii.Model.Count + int.Parse(string.IsNullOrEmpty(ii.Added) ? "0" : ii.Added)).ToArray());

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

					Load();
				}
				finally
				{
					Context.DisplayProgress = false;
					await _channelManagementService.InsertEventAsync("Standard Cash", "True");
				}
			});
		}

		public override void Load()
		{
            /*
			Cassettes = _cashDispenser?.GetMediaInfo().
				Where(i =>
					i.Type != "REJECT" &&
					i.Type != "RETRACT" &&
					i.Type != "REJECTCASSETTE" &&
					i.Type != "RETRACTCASSETTE").
				Select(i => new MediaUnitViewModel
				{
					Model = i,
					Added = (from SupervisoryConfigurationElement c in ConfigCassettes
							 where c.Id == i.Value.ToString()
							 select c)
						.FirstOrDefault()?.Value
				}).ToArray();
            */

            MediaUnit m1 = new MediaUnit();
            m1.Id = 1;
            m1.InitialCount = 0;
            m1.MaxCount = 0;
            m1.PresentedCount = 0;
            m1.RejectedCount = 0;
            m1.RetractedCount = 0;
            m1.Status = "DIS";
            m1.TotalCount = 0;
            m1.Type = "DIS";
            m1.Value = 0;
            m1.Count = 0;
            m1.Currency = TerminalConfiguration.Section?.Currency;
            m1.DispensedCount = 0;
            

            IList<MediaUnit> iList = new List<MediaUnit>();
            iList.Add(m1);

            Cassettes = iList.ToArray().
                Where(i =>
                    i.Type != "DIS").
                Select(i => new MediaUnitViewModel
                {
                    Model = i,
                    Added = (from SupervisoryConfigurationElement c in ConfigCassettes
                             where c.Id == i.Value.ToString()
                             select c)
                        .FirstOrDefault()?.Value
                }).ToArray();
        }

		MediaUnitViewModel[] cassettes;
		public MediaUnitViewModel[] Cassettes
		{
			get { return cassettes; }
			set { SetProperty(ref cassettes, value); }
		}

		public SupervisoryConfigurationCassettesElementCollection ConfigCassettes => ((SupervisoryConfigurationSection)ConfigurationManager.GetSection(SupervisoryConfigurationSection.Name))?.SupervisoryConfigurationCassettes;
	}
}