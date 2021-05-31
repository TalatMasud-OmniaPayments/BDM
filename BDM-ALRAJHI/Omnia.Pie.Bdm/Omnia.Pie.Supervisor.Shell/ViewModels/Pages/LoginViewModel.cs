using Omnia.Pie.Supervisor.Shell.Configuration;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class LoginViewModel : PageViewModel
	{
        
        public override bool IsEnabled => !Context.IsLoggedInMode && Context.DoorsOpen;

        public LoginViewModel()
		{
			SelectedUserName = UserNames?.FirstOrDefault();

			Login = new DelegateCommand(() =>
			{
				if (!Validate())
					return;

				var role = (from SupervisoryConfigurationElement r in Roles where r.Id == SelectedUserName select r).FirstOrDefault();
				if (role?.Value != Crypto.EncryptStringAes(Password, SelectedUserName))
				{
					ErrorMessage = "Wrong User Name/ Password.";
					return;
				}
				ErrorMessage = string.Empty;

				var isSupervisor = SelectedUserName.ToLower().Contains("supervisor");
				Context.Login(isSupervisor, SelectedUserName);
				Password = null;
			});

		}

        

        public ICommand Login { get; }

        

        [Required(ErrorMessage = @"Required.")]
		public string SelectedUserName { get; set; }

		private string _password;

		[Required(ErrorMessage = @"Required.")]
		[DataType(DataType.Password)]
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		private string _errorMessage;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { SetProperty(ref _errorMessage, value); }
		}

		public ObservableCollection<string> UserNames => new ObservableCollection<string>((from SupervisoryConfigurationElement item in Roles select item.Id));

		public SupervisoryConfigurationRolesElementCollection Roles => ((SupervisoryConfigurationSection)ConfigurationManager.GetSection(SupervisoryConfigurationSection.Name))?.SupervisoryConfigurationRoles;

	}
}
