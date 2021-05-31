using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Supervisor.Shell.Configuration;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public class ChangePasswordViewModel : PageViewModel
	{
		public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.ChangePassword == true ? true : false);

        public ChangePasswordViewModel()
		{
			SelectedUserName = UserNames?.FirstOrDefault();

			ChangePassword = new DelegateCommand(() =>
			{
				if (Validate() && Roles != null && NewPassword == ConfirmNewPassword)
				{
					var role = (from SupervisoryConfigurationElement r in Roles where r.Id == SelectedUserName select r).FirstOrDefault();
					if (role.Value == Crypto.EncryptStringAes(OldPassword, SelectedUserName))
					{
						SupervisoryConfiguration.SetRoleElementValue(role.Id, Crypto.EncryptStringAes(NewPassword, SelectedUserName));

						_journal.PasswordChanged(role.Id);
						_channelManagementService.InsertEventAsync("Password Changed", "True");
					}
				}
			});
		}

		public ICommand ChangePassword { get; }
		public string SelectedUserName { get; set; }

		private string _oldPassword;

		[Required(ErrorMessage = @"Required.")]
		[StringLength(10, ErrorMessage = @"Minimum length is {2}.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string OldPassword
		{
			get { return _oldPassword; }
			set { SetProperty(ref _oldPassword, value); }
		}

		private string _newPassword;

		[Required(ErrorMessage = @"Required.")]
		[StringLength(10, ErrorMessage = @"Minimum length is {2}.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string NewPassword
		{
			get { return _newPassword; }
			set { SetProperty(ref _newPassword, value); }
		}

		private string _confirmNewPassword;

		[Required(ErrorMessage = @"Required.")]
		[StringLength(10, ErrorMessage = @"Minimum length is {2}.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = @"Password mismatch.")]
		public string ConfirmNewPassword
		{
			get { return _confirmNewPassword; }
			set { SetProperty(ref _confirmNewPassword, value); }
		}

		public ObservableCollection<string> UserNames => new ObservableCollection<string>((from SupervisoryConfigurationElement item in Roles select item.Id));

		public SupervisoryConfigurationRolesElementCollection Roles => ((SupervisoryConfigurationSection)ConfigurationManager.GetSection(SupervisoryConfigurationSection.Name))?.SupervisoryConfigurationRoles;
	}
}