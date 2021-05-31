using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Configurations;
using System.Collections.ObjectModel;
using Omnia.Pie.Supervisor.Shell.Configuration;
using System.Configuration;
using System.Linq;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
    public class UserRolesViewModel: PageWithPrintViewModel
    {
        //public override bool IsEnabled => Context.IsLoggedInMode;
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Roles == true ? true : false);

        private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();
        private readonly IUserRolesStore _userRolesStore = ServiceLocator.Instance.Resolve<IUserRolesStore>();


        private UserRoles _userRoles;
        public UserRoles UserRoles
        {
            get { return _userRoles; }
            set { SetProperty(ref _userRoles, value); }
        }

        private string _selectedUserName;
        public string SelectedUserName
        {
            get {
                LoadRoles();
                return _selectedUserName;
            }
            set {
                SetProperty(ref _selectedUserName, value);
            }
        }

        private bool _isDiagnostic;
        public bool IsDiagnostic
        {
            get { return _isDiagnostic; }
            set { SetProperty(ref _isDiagnostic, value); }
        }

        private List<UserRole> _users;
        public List<UserRole> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        public ICommand Apply { get; }

        public UserRolesViewModel()
        {
            SelectedUserName = UserNames?.FirstOrDefault();
            LoadRoles();

            Apply = new DelegateCommand(
                () =>
                {
                    Context.DisplayProgress = true;
                    try
                    {
                        _userRolesStore.Update(UserRoles);
                    }
                    finally
                    {
                        Context.DisplayProgress = false;
                        _channelManagementService.InsertEventAsync("User Roles", "True");
                    }
                });
        }

        public override void Load()
        {
            _users = UserRolesConfiguration.GetAllElements();
        }
        public void LoadRoles()
        {
            UserRoles = _userRolesStore.GetUserRole(_selectedUserName);
        }
        


        public ObservableCollection<string> UserNames => new ObservableCollection<string>((from SupervisoryConfigurationElement item in Roles select item.Id));
        public SupervisoryConfigurationRolesElementCollection Roles => ((SupervisoryConfigurationSection)ConfigurationManager.GetSection(SupervisoryConfigurationSection.Name))?.SupervisoryConfigurationRoles;

    }
}