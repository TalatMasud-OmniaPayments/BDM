using System.Windows.Input;
using Omnia.Pie.Supervisor.Shell.Utilities;
using System.Collections.ObjectModel;
using System;
using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Configuration;
using System.ComponentModel.DataAnnotations;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Client.Journal.Interface.Extension;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class ConfigurationViewModel : PageViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Configuration == true ? true : false);
        private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();


		#region Properties

		private string _machineDate;
		[Range(typeof(DateTime), "1/1/1900 00:00:00", "1/1/3000 23:59:59", ErrorMessage = "Invalid Date")]
		public string MachineDate
		{
			get { return _machineDate; }
			set { SetProperty(ref _machineDate, value); }
		}

		public ObservableCollection<string> TimeZones { get; set; }

		private string _selectedTimeZone;
		public string SelectedTimeZone
		{
			get { return _selectedTimeZone; }
			set { SetProperty(ref _selectedTimeZone, value); }
		}

		private string _localIpAddress1;
		[Required(ErrorMessage = "Required")]
		public string LocalIpAddress1
		{
			get { return _localIpAddress1; }
			set { SetProperty(ref _localIpAddress1, value); }
		}

		private string _localIpAddress2;
		[Required(ErrorMessage = "Required")]
		public string LocalIpAddress2
		{
			get { return _localIpAddress2; }
			set { SetProperty(ref _localIpAddress2, value); }
		}

		private string _localIpAddress3;
		[Required(ErrorMessage = "Required")]
		public string LocalIpAddress3
		{
			get { return _localIpAddress3; }
			set { SetProperty(ref _localIpAddress3, value); }
		}

		private string _localIpAddress4;
		[Required(ErrorMessage = "Required")]
		public string LocalIpAddress4
		{
			get { return _localIpAddress4; }
			set { SetProperty(ref _localIpAddress4, value); }
		}

		private ConfigurationItemViewModel[] _configurationItems;
		public ConfigurationItemViewModel[] ConfigurationItems
		{
			get { return _configurationItems; }
			set { SetProperty(ref _configurationItems, value); }
		}

		private string _serverIpAddress1;
		[Required(ErrorMessage = "Required")]
		public string ServerIpAddress1
		{
			get { return _serverIpAddress1; }
			set { SetProperty(ref _serverIpAddress1, value); }
		}

		private string _serverIpAddress2;
		[Required(ErrorMessage = "Required")]
		public string ServerIpAddress2
		{
			get { return _serverIpAddress2; }
			set { SetProperty(ref _serverIpAddress2, value); }
		}

		private string _serverIpAddress3;
		[Required(ErrorMessage = "Required")]
		public string ServerIpAddress3
		{
			get { return _serverIpAddress3; }
			set { SetProperty(ref _serverIpAddress3, value); }
		}

		private string _serverIpAddress4;
		[Required(ErrorMessage = "Required")]
		public string ServerIpAddress4
		{
			get { return _serverIpAddress4; }
			set { SetProperty(ref _serverIpAddress4, value); }
		}
		private string _serverPort;
		[Required(ErrorMessage = "Required")]
		[RegularExpression("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$", ErrorMessage = "Invalid port number")]
		public string ServerPort
		{
			get { return _serverPort; }
			set { SetProperty(ref _serverPort, value); }
		}

		private string _terminalID;
		[Required(ErrorMessage = "Required")]
		public string TerminalID
		{
			get { return _terminalID; }
			set { SetProperty(ref _terminalID, value); }
		}

		private string _branchID;
		[Required(ErrorMessage = "Required")]
		public string BranchID
		{
			get { return _branchID; }
			set { SetProperty(ref _branchID, value); }
		}

		#endregion

		#region Commands

		public ICommand Apply { get; }

		public ICommand ApplyServerSettings { get; }

		public ICommand ApplyMachineIP { get; }

		public ICommand ApplyMachineDateTime { get; }

		public ICommand ApplyMachineCommon { get; }

		#endregion

		public ConfigurationViewModel()
		{
			ApplyMachineCommon = new DelegateCommand<object>(
				x =>
				{
					if (Validate())
					{
						SupervisoryConfiguration.SetCommonSettings(TerminalID, BranchID);
						_journal.TerminalBranchIdChanged(TerminalID, BranchID);
						Load();
					}
				}, x => true);

			ApplyMachineDateTime = new DelegateCommand<object>(
				x =>
				{
					if (Validate())
					{
						var systemConfiguration = new SystemConfiguration();
						systemConfiguration.SetSystemTimeZone(SelectedTimeZone);
						DateTime selectedDate;
						DateTime.TryParse(MachineDate, out selectedDate);

						if (selectedDate != null)
							systemConfiguration.SetTime((ushort)selectedDate.Day, (ushort)selectedDate.Month, (ushort)selectedDate.Year, (ushort)selectedDate.Hour, (ushort)selectedDate.Minute, (ushort)selectedDate.Second);

						this.Load();
					}
				}, x => true);
			ApplyMachineIP = new DelegateCommand<object>(
				x =>
				{
					if (Validate())
					{
						var systemConfiguration = new SystemConfiguration();
						systemConfiguration.SetIP($"{LocalIpAddress1}.{LocalIpAddress2}.{LocalIpAddress3}.{LocalIpAddress4}");
						this.Load();
					}
				}, x => true);

			ApplyServerSettings = new DelegateCommand<object>(
				x =>
				{
					if (Validate())
					{
						try
						{
							if (new SystemConfiguration().PingHost($"{ServerIpAddress1}.{ServerIpAddress2}.{ServerIpAddress3}.{ServerIpAddress4}"))
							{
								SupervisoryConfiguration.SetServerAddress($"{ServerIpAddress1}.{ServerIpAddress2}.{ServerIpAddress3}.{ServerIpAddress4}", ServerPort);
							}
						}
						catch (Exception ex)
						{
							_logger.Error($"Server settings cannot be applied");
							_logger.Exception(ex);
						}

						Load();
					}
				},
				x => true);
		}

		public override void Load()
		{
			try
			{
				Tuple<string, string> tuple = SupervisoryConfiguration.GetCommonSettings();
				TerminalID = tuple.Item1;
				BranchID = tuple.Item2;

				Tuple<string, string> serverConfig = SupervisoryConfiguration.GetIPandPort();
				if (!string.IsNullOrEmpty(serverConfig?.Item1))
				{
					ServerIpAddress1 = serverConfig.Item1.Split('.')[0];
					ServerIpAddress2 = serverConfig.Item1.Split('.')[1];
					ServerIpAddress3 = serverConfig.Item1.Split('.')[2];
					ServerIpAddress4 = serverConfig.Item1.Split('.')[3];
				}

				ServerPort = serverConfig.Item2;
				LocalIpAddress1 = new SystemConfiguration().GetLocalIPAddress().Split('.')[0];
				LocalIpAddress2 = new SystemConfiguration().GetLocalIPAddress().Split('.')[1];
				LocalIpAddress3 = new SystemConfiguration().GetLocalIPAddress().Split('.')[2];
				LocalIpAddress4 = new SystemConfiguration().GetLocalIPAddress().Split('.')[3];
				MachineDate = DateTime.Now.ToString();

				TimeZones = new ObservableCollection<string>();
				ReadOnlyCollection<TimeZoneInfo> tz = TimeZoneInfo.GetSystemTimeZones();
				SelectedTimeZone = TimeZone.CurrentTimeZone.StandardName;
				foreach (var item in tz)
					TimeZones.Add(item.StandardName);
			}
			catch (Exception ex)
			{
				_logger.Error($"There was an exception while initializing data.");
				_logger.Exception(ex);
			}
		}

		private static ConfigurationItemViewModel CreateConfigurationItem(string title, string key)
		{
			return new ConfigurationItemViewModel
			{
				Title = title,
				Key = key,
				Value = SupervisoryConfiguration.GetValue(key)
			};
		}
	}
}
