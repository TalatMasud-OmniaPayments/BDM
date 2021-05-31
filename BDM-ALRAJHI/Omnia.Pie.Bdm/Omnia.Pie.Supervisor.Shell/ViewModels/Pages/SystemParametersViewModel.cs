using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Configurations;
using System.Collections.Generic;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class SystemParametersViewModel : PageWithPrintViewModel
	{
		//public override bool IsEnabled => Context.IsLoggedInMode;
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.SystemParameters == true ? true : false);

        private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();

		private List<SystemParameter> _systemParameters;
		public List<SystemParameter> SystemParameters
		{
			get { return _systemParameters; }
			set { SetProperty(ref _systemParameters, value); }
		}

		public ICommand Apply { get; }

		public SystemParametersViewModel()
		{
			Apply = new DelegateCommand(
				() =>
				{
					Context.DisplayProgress = true;

					try
					{
						foreach (var item in _systemParameters)
						{
							SystemParametersConfiguration.SetElementValue(item.Key, item.Value);
						}
					}
					finally
					{
						Context.DisplayProgress = false;
						_channelManagementService.InsertEventAsync("System Parameters", "True");
					}
				});
		}

		public override void Load()
		{
			_systemParameters = SystemParametersConfiguration.GetAllElements();
		}
	}
}
