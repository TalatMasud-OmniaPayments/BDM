namespace Omnia.Pie.Vtm.Workflow
{
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Authentication;
	using Omnia.Pie.Vtm.Workflow.Common;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.MainMenu;
    using Microsoft.Practices.Unity;
    using System;
	using System.Threading;
	using System.Threading.Tasks;

	public abstract class BaseFlow : IDisposable
	{
		protected readonly ILogger _logger;
		protected readonly IResolver _container;
		protected readonly INavigationObserver _navigator;
		protected readonly ITopVideoObserver _videoService;
		protected readonly IJournal _journal;
        protected readonly INetworkObserver _networkStatus;
        public IDataContext Context { get; set; }
        protected readonly IChannelManagementService _channelService;
        //public IDataContext UserContext { get; set; }

        public short InactivityTimer = 30;

		public BaseFlow(IResolver container)
		{
			_container = container;
			_navigator = container.Resolve<INavigationObserver>();
			_logger = container.Resolve<ILogger>();
			_videoService = container.Resolve<ITopVideoObserver>();
			_journal = container.Resolve<IJournal>();
            _channelService = container.Resolve<IChannelManagementService>();
            //IDataContext context2 = container.Resolve<IDataContext>();
            _networkStatus = container.Resolve<INetworkObserver>();
            //_networkStatus.co


            short.TryParse(SystemParametersConfiguration.GetElementValue("InactivityTimeLimit"), out InactivityTimer);
		}

		protected async Task<bool> LoadErrorScreenAsync(ErrorType type, bool cancel = true)
		{
           
            _logger.Info($"Loading Error Screen: {type.ToString()}");

            var cancellationToken = new CancellationTokenSource();
            var _taskSource = new TaskCompletionSource<bool>();

			_navigator.RequestNavigationTo<IErrorViewModel>((viewModel) =>
			{
				viewModel.Type(type);

				viewModel.BackVisibility = !cancel;
				viewModel.CancelVisibility = cancel;

				viewModel.CancelAction = viewModel.BackAction = () =>
				{
                    cancellationToken?.Cancel();
                    cancellationToken = null;
                    _taskSource.SetResult(true);
                    LoadSelfServiceMenu();
                };
                LoadTimeoutScreen(viewModel, cancellationToken);

                /*viewModel.StartUserActivityTimer(cancellationToken.Token);
                viewModel.ExpiredAction = () =>
                {
                    _navigator.Push<IActiveConfirmationViewModel>((vm) =>
                    {
                        vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                        vm.YesAction = () =>
                        {
                            viewModel.StartUserActivityTimer(cancellationToken.Token);
                            _navigator.Pop();
                        };
                        vm.NoAction = vm.ExpiredAction = () =>
                        {
                            cancellationToken?.Cancel();
                            cancellationToken = null;

                            LoadMainScreen();
                            //CancelAction?.Invoke();
                        };
                    });
                };*/
            });

			return await _taskSource.Task;
		}       


        protected async Task<bool> LoadErrorScreenAsync(ErrorType type, Action action, bool cancel = true)
		{
            
            _logger.Info($"Loading Error Screen: {type.ToString()}");

            var cancellationToken = new CancellationTokenSource();
            var _taskSource = new TaskCompletionSource<bool>();

			_navigator.RequestNavigationTo<IErrorViewModel>((viewModel) =>
			{
				viewModel.Type(type);

				viewModel.BackVisibility = !cancel;
				viewModel.CancelVisibility = cancel;

				viewModel.CancelAction = viewModel.BackAction = () =>
				{
                    cancellationToken?.Cancel();
                    cancellationToken = null;
                    action?.Invoke();
					_taskSource.SetResult(true);
				};
                LoadTimeoutScreen(viewModel, cancellationToken);
            });

			return await _taskSource.Task;
		}

        protected async Task<bool> LoadTimedErrorScreenAsync(ErrorType type, Action action, int timeout = 15)
        {
            _logger.Info($"Loading Error Screen: {type.ToString()}");

            var _taskSource = new TaskCompletionSource<bool>();

            _navigator.RequestNavigationTo<ITimedErrorViewModel>((viewModel) =>
            {
                viewModel.Type(type);

                viewModel.BackVisibility = false;
                viewModel.CancelVisibility = false;

                viewModel.StartTimer(new TimeSpan(0, 0, 15));
                viewModel.ExpiredAction = () => {
                    action?.Invoke();
                    _taskSource.SetResult(true);
                };

            });

            return await _taskSource.Task;
        }

        protected void LoadTimeoutScreen(IExpirableBaseViewModel viewModel, CancellationTokenSource cancellationToken)
        {
            //var cancellationToken = new CancellationTokenSource();

            viewModel.StartUserActivityTimer(cancellationToken.Token);
            viewModel.ExpiredAction = () =>
            {
                _navigator.Push<IActiveConfirmationViewModel>((vm) =>
                {
                    vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                    vm.YesAction = () =>
                    {
                        viewModel.StartUserActivityTimer(cancellationToken.Token);
                        _navigator.Pop();
                    };
                    vm.NoAction = vm.ExpiredAction = () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        LoadMainScreen();
                        //CancelAction?.Invoke();
                    };
                });
            };
        }
        protected void LoadTimeoutScreen(IExpirableBaseViewModel viewModel, CancellationTokenSource cancellationToken, TaskCompletionSource<bool> _workflowCompletionTask)
        {
            //var cancellationToken = new CancellationTokenSource();

            viewModel.StartUserActivityTimer(cancellationToken.Token);
            viewModel.ExpiredAction = () =>
            {
                _navigator.Push<IActiveConfirmationViewModel>((vm) =>
                {
                    vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                    vm.YesAction = () =>
                    {
                        viewModel.StartUserActivityTimer(cancellationToken.Token);
                        _navigator.Pop();
                    };
                    vm.NoAction = vm.ExpiredAction = () =>
                    {
                        cancellationToken?.Cancel();
                        cancellationToken = null;

                        LoadMainScreen();
                        _workflowCompletionTask.TrySetResult(true);
                    };
                });
            };
        }
        protected void LoadWaitScreen()
		{
            _logger.Info($"Loading Wait Screen");
            _navigator.RequestNavigationTo<IAnimationViewModel>((vm) => { vm.Type(AnimationType.Wait); });
		}

        protected async void MonitorDeviceStatus()
        {
            var devStatus = ServiceLocator.Instance.Resolve<ChannelManagementDeviceStatusService>();
            var channelService = ServiceLocator.Instance.Resolve<IChannelManagementService>();
            await channelService.SendDeviceStatus(devStatus.GetDevicesStatus(), true, false);
        }

		protected async void LoadMainScreen()
		{
            
            await EndSSCurrentSession();
			_videoService.FullVolume();
            Loagout();
            _logger.Info($"Loading Main Screen");
			var mainWorkflow = _container.Resolve<MainWorkflow>();
			mainWorkflow.Execute();

		}

        protected async void LoadMainBDMScreen()
        {

            await EndSSCurrentSession();
            _videoService.FullVolume();
            
            _logger.Info($"Loading Main Screen");
            var mainWorkflow = _container.Resolve<AuthenticationLoginWorkflow>();
            mainWorkflow.Execute();

        }
        protected void Loagout()
        {

            _container.Resolve<IAuthDataContext>().Username = string.Empty;
            _container.Resolve<IAuthDataContext>().loggedInUserInfo = null;
            _container.Resolve<IAuthDataContext>().Username = "";
            _container.Resolve<IAuthDataContext>().Password = "";
            _container.Resolve<IAuthDataContext>().Fingerprint = "";
            _container.Resolve<IAuthDataContext>().AuthenticatedByFingerprint = false;
        }
            private async Task EndSSCurrentSession()
		{
			await _container.Resolve<ReturnCardStep>().ReturnCard();

			_container.Resolve<ISessionContext>().Name = string.Empty;
			_container.Resolve<ISessionContext>().CustomerIdentifier = string.Empty;
			_container.Resolve<ISessionContext>().CardUsed = null;
			_container.Resolve<ISessionContext>().AccountNumber = string.Empty;
			_container.Resolve<ISessionContext>().BalanceAmount = 0.0;
			_container.Resolve<ISessionContext>().TransactionHistory = null;
			_container.Resolve<ISessionContext>().CifAuth = false;
			_container.Resolve<ISessionContext>().EIdNumber = string.Empty;
			_container.Resolve<ISessionContext>().CardFdk = string.Empty;
			_container.Resolve<ISessionContext>().InCall = false;
			SetCallMod(false);
		}

		protected async void EndRTCurrentSession()
		{
			await _container.Resolve<ReturnCardStep>().ReturnCard();

			_container.Resolve<ISessionContext>().Name = string.Empty;
			_container.Resolve<ISessionContext>().CustomerIdentifier = string.Empty;
			_container.Resolve<ISessionContext>().CardUsed = null;
			_container.Resolve<ISessionContext>().AccountNumber = string.Empty;
			_container.Resolve<ISessionContext>().BalanceAmount = 0.0;
			_container.Resolve<ISessionContext>().TransactionHistory = null;
			_container.Resolve<ISessionContext>().CifAuth = false;
			_container.Resolve<ISessionContext>().EIdNumber = string.Empty;
			_container.Resolve<ISessionContext>().CardFdk = string.Empty;

		}

	

		
		protected bool GetCallMod()
		{
			var result = false;
			try
			{
				result = _container.Resolve<ISessionContext>().SelfCallMod;
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}

			return result;
		}

		protected void SetCallMod(bool self)
		{
			_container.Resolve<ISessionContext>().SelfCallMod = self;
		}

		protected IDataContext CreateContext(Type t)
		{
			if (t == null)
				throw new ArgumentNullException(nameof(t));

			var c = (IDataContext)_container.Resolve(typeof(DataContext<>).MakeGenericType(t));
			return c;
		}

		protected bool IsWorkingHours()
		{
			return DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 22;
		}

        public void LoadSelfServiceMenu()
        {

            var businessMainMenuSelection = _container.Resolve<BusinessUserMainMenuWorkFlow>();
            businessMainMenuSelection.Execute();

        }

        public abstract void Dispose();
	}
}