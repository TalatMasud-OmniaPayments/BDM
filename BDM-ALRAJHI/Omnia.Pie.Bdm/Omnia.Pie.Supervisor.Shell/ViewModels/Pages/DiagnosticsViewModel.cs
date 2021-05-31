namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
    using Microsoft.Practices.Unity;
    using Microsoft.Win32;
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Supervisor.Shell.ViewModels.Devices;
    using Omnia.Pie.Supervisor.UI.Themes.Controls;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.Devices.Interface.Entities;
    using Omnia.Pie.Vtm.Framework.DelegateCommand;
    using Omnia.Pie.Vtm.Framework.Extensions;
    using Omnia.Pie.Vtm.Framework.Interface;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Windows;
    using System.Windows.Input;
    using DataAccessEntities = Vtm.DataAccess.Interface.Entities;

    public class DiagnosticsViewModel : PageViewModel
    {
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Diagnostic == true ? true : false);

        private readonly IDeviceErrorStore _deviceErrorStore = ServiceLocator.Instance.Resolve<IDeviceErrorStore>();
        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());

        DeviceViewModel device;

        public DeviceViewModel SelectedDevice
        {
            get { return device; }
            set
            {
                SetProperty(ref device, value);
            }
        }

        public ObservableCollection<DeviceError> Errors { get; } = new ObservableCollection<DeviceError>();

        public ObservableCollection<DataAccessEntities.DeviceError> DeviceErrors { get; } = new ObservableCollection<DataAccessEntities.DeviceError>();

        public ICommand ShowErrorHistory { get; }


        /* private string _startDate;
         [Required(ErrorMessage = @"Valid date required")]
         public string StartDate
         {
             get {
                 return _startDate;
             }
             set {
                 SetProperty(ref _startDate, value);
             }
         }*/
        private DateTime _startDate;
        // [Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
        public DateTime StartDate
        {
            get
            {
                if (_startDate == DateTime.MinValue)
                    _startDate = DateTime.Today.AddMonths(-1);

                return _startDate;
            }
            set
            {
                _startDate = value;

                RaisePropertyChanged("StartDate");
            }
        }

        /*private string _endDate;

		[Required(ErrorMessage = @"Valid date required")]
		public string EndDate
		{
			get {
                return _endDate;
            }
			set {
                SetProperty(ref _endDate, value);
            }
		}*/

        private DateTime _endDate;
        //[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
        //[Required(ErrorMessage = @"Valid date required")]
        public DateTime EndDate
        {
            get
            {
                if (_endDate == DateTime.MinValue)
                    _endDate = DateTime.Today;

                return _endDate;
            }
            set
            {
                _endDate = value;
                RaisePropertyChanged("EndDate");
            }
        }

        public DiagnosticsViewModel()
        {
            /*StartDate = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
			EndDate = DateTime.Now.ToString("MM/dd/yyyy");*/

            ShowErrorHistory = new DelegateCommand(async () =>
            {
                if (!Validate())
                    return;

                /*DateTime startDate;
				DateTime endDate;

				if (!DateTime.TryParse(StartDate, out startDate) || !DateTime.TryParse(EndDate, out endDate))
					return;*/

                DeviceErrors.Clear();

                var deviceErrors = await _deviceErrorStore.GetList(StartDate, EndDate);
                if (deviceErrors == null || deviceErrors.Count == 0)
                    return;

                deviceErrors.ForEach(deviceError => DeviceErrors.Add(deviceError));
            });
        }

        public override void Load()
        {
            Errors.Clear();

            var path = @"SOFTWARE\ATM\ErrorCode";

            if (EnvironmentHelper.Is64BitOperatingSystem())
            {
                path = @"SOFTWARE\WOW6432Node\ATM\ErrorCode";
            }

            using (var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                foreach (var i in reg.OpenSubKey(path).GetValues())
                {
                    if (string.IsNullOrWhiteSpace(i.Key))
                        continue;

                    var s = i.Value as string;
                    if (string.IsNullOrWhiteSpace(s))
                        continue;

                    int x;
                    if (int.TryParse(s, out x) && x == 0)
                        continue;

                    Errors.Add(new DeviceError
                    {
                        Message = i.Key,
                        Code = s
                    });
                    //await _channelManagementService.InsertEventAsync("CIT: Clear Cash", "Step not followed");
                    //_channelManagementService.InsertEventAsync("DEVICE_ERROR_CODE", s);
                    Logger.Info("DEVICE_ERROR_CODE: " + s);
                }
            }
        }
    }
}