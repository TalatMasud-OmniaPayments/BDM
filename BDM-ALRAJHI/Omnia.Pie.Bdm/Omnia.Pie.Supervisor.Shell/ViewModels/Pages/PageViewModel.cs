using Microsoft.Practices.Unity;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Services.Interface;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public abstract class PageViewModel : ViewModel
	{
		public SupervisorService Context { get; } = ServiceLocator.Instance.Resolve<SupervisorService>();
		public readonly IJournal _journal = ServiceLocator.Instance.Resolve<IJournal>();
		protected readonly IChannelManagementService _channelManagementService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
		public virtual string Id => GetType().Name.ToHumanString();

		public virtual bool IsEnabled => true;

		protected PageViewModel()
		{
			Context.DoorsOpenChanged += IsEnabledChanged;
            Context.SupervisoryStatusUpdated += Context_SupervisoryStatusUpdated;
            Context.LogoutEvent += IsEnabledChanged;
			Context.LoginEvent += IsEnabledChanged;
		}

        private void Context_SupervisoryStatusUpdated(object sender, Vtm.Devices.Interface.SensorsStatus e)
        {
            RaisePropertyChanged(nameof(IsEnabled));
        }

        private void IsEnabledChanged(object sender, System.EventArgs e)
		{
			RaisePropertyChanged(nameof(IsEnabled));
		}
	}
}