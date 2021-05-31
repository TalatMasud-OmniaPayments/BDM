using Microsoft.Practices.EnterpriseLibrary.Logging;
using Omnia.Pie.Supervisor.Shell.Configuration;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class CopyLogsViewModel : PageViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.CopyLogs == true ? true : false);
        private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();

		public CopyLogsViewModel()
		{
			CopyLogs = new DelegateCommand<object>(
				x =>
				{
					try
					{
						string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
						string logsListenerName = SupervisoryConfiguration.GetConfigValueWithXPath("//loggingConfiguration//categorySources//add[@name='General']//listeners//add", "name");
						string logsDirectory = SupervisoryConfiguration.GetConfigValueWithXPath($"//loggingConfiguration//listeners//add[@name='{logsListenerName}']", "fileName");
						logsDirectory = Path.GetDirectoryName(Path.GetFullPath(logsDirectory));
						string ejListenerName = SupervisoryConfiguration.GetConfigValueWithXPath("//loggingConfiguration//categorySources//add[@name='Journal']//listeners//add", "name");
						string ejDirectory = SupervisoryConfiguration.GetConfigValueWithXPath($"//loggingConfiguration//listeners//add[@name='{ejListenerName}']", "fileName");
						ejDirectory = Path.GetDirectoryName(Path.GetFullPath(ejDirectory));
						string bsTraceLocation = SupervisoryConfiguration.GetValue($"bsTraceDirectory");
						string destinationDirectory = $"{ SelectedDrive }\\Logs{ DateTime.Now.Year}{ DateTime.Now.Month}{ DateTime.Now.Day}\\";
						Directory.CreateDirectory(destinationDirectory);
						DateTime startDate;
						DateTime.TryParse(StartDate, out startDate);
						DateTime endDate;
						DateTime.TryParse(EndDate, out endDate);
						string terminalID = SupervisoryConfiguration.GetCommonSettings().Item1;
						if (startDate != null && endDate != null)
						{
							while (startDate < endDate)
							{
								try
								{
									startDate = startDate.AddDays(1);
									string month = startDate.Month <= 9 ? "0" + startDate.Month : startDate.Month.ToString();
									string day = startDate.Day <= 9 ? "0" + startDate.Day : startDate.Day.ToString();
									string fileName = $"{startDate.Year}{month}{day}";
									string ejFileFormat = $"{ejDirectory}EJ_{terminalID}_{fileName}.log";
									string logFileFormat = $"{logsDirectory}General{fileName}.log";
									if (File.Exists(logFileFormat))
										File.Copy(logFileFormat, $"{destinationDirectory}General{fileName}.log", true);
									if (File.Exists(ejFileFormat))
										File.Copy(ejFileFormat, $"{destinationDirectory}EJ_{terminalID}_{fileName}.log", true);
								}
								catch (Exception ex)
								{
									_logger.Error($"Log file cannot be copied");
									_logger.Exception(ex);
								}
							}
						}

						if (File.Exists(bsTraceLocation))
							File.Copy(bsTraceLocation, $"{destinationDirectory}BSTrace.nwlog", true);

						string logMonth = DateTime.Now.Month <= 9 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString();
						string logDay = DateTime.Now.Day <= 9 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString();

						string logZipFile = $"{SelectedDrive}{terminalID}_{DateTime.Now.Year}{logMonth}{logDay}.zip";
						if (File.Exists(logZipFile))
							File.Delete(logZipFile);

						ZipFile.CreateFromDirectory(destinationDirectory, logZipFile);
						string destinationLocation = SelectedDrive;
					}
					catch (Exception ex)
					{
						_logger.Error($"Log files cannot be copied");
						_logger.Exception(ex);
					}

				}, x => Drives.Count > 0);
		}

		public ICommand CopyLogs { get; }

		private string _startDate;
		[Required(ErrorMessage = "Required")]
		public string StartDate
		{
			get { return _startDate; }
			set { SetProperty(ref _startDate, value); }
		}

		private string _endDate;
		[Required(ErrorMessage = "Required")]
		public string EndDate
		{
			get { return _endDate; }
			set { SetProperty(ref _endDate, value); }
		}

		private string _selectedDrive;
		[Required(ErrorMessage = "Required")]
		public string SelectedDrive
		{
			get { return _selectedDrive; }
			set { SetProperty(ref _selectedDrive, value); }
		}

		public ObservableCollection<string> Drives { get; set; }

		public override void Load()
		{
			Drives = new ObservableCollection<string>();
			DriveInfo[] allDrives = DriveInfo.GetDrives();
			foreach (var item in allDrives)
				if (item.DriveType == DriveType.Removable)
					Drives.Add(item.Name);
			SelectedDrive = Drives.Count > 0 ? Drives[0] : "";
			StartDate = DateTime.Now.ToString("MM/dd/yyyy");
			EndDate = DateTime.Now.ToString("MM/dd/yyyy");
		}
	}
}
