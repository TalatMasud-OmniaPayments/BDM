using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Interface;

namespace Omnia.Pie.Vtm.DataAccess.Stores
{
    internal class UserRolesStore : StoreBase, IUserRolesStore
    {

        public UserRolesStore(IResolver container) : base(container)
        {
            ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [UserRoles] (
					[Id]                    INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [UserName]              TEXT         NOT NULL,
					[Dashboard]             BOOLEAN     NOT NULL,
                    [Diagnostic]            BOOLEAN     NOT NULL,
                    [ClearCashIn]           BOOLEAN     NOT NULL,
                    [ClearCards]            BOOLEAN     NOT NULL,
                    [StandardCash]          BOOLEAN     NOT NULL,
                    [DeviceConfiguration]   BOOLEAN     NOT NULL,
                    [SystemParameters]      BOOLEAN     NOT NULL,
                    [Configuration]         BOOLEAN     NOT NULL,
                    [CopyLogs]              BOOLEAN     NOT NULL,
                    [ChangePassword]        BOOLEAN     NOT NULL,
                    [VDM]                   BOOLEAN     NOT NULL,
                    [Reboot]                BOOLEAN     NOT NULL,
                    [Roles]                 BOOLEAN     NOT NULL
                    
                    )");

            //var role = new Role() { task = "Dashboard"};
            //Save(role);
            LoadRoles("Supervisor");
            LoadCITRoles("CIT");
            LoadSLMRoles("SLM");
        }

        private void LoadRoles(String username) {


            Save(new UserRoles
            {
                UserName = username,
                Dashboard = true,
                Diagnostic = true,
                ClearCashIn = true,
                ClearCards = true,
                StandardCash = true,
                DeviceConfiguration = true,
                SystemParameters = true,
                Configuration = true,
                CopyLogs = true,
                ChangePassword = true,
                VDM = true,
                Reboot = true,
                Roles = true,
            });

        }
        private void LoadCITRoles(String username)
        {


            Save(new UserRoles
            {
                UserName = username,
                Dashboard = false,
                Diagnostic = false,
                ClearCashIn = true,
                ClearCards = false,
                StandardCash = false,
                DeviceConfiguration = false,
                SystemParameters = false,
                Configuration = false,
                CopyLogs = false,
                ChangePassword = false,
                VDM = false,
                Reboot = false,
                Roles = false
            });

        }
        private void LoadSLMRoles(String username)
        {


            Save(new UserRoles
            {
                UserName = username,
                Dashboard = true,
                Diagnostic = true,
                ClearCashIn = false,
                ClearCards = false,
                StandardCash = false,
                DeviceConfiguration = true,
                SystemParameters = false,
                Configuration = false,
                CopyLogs = false,
                ChangePassword = false,
                VDM = false,
                Reboot = true,
                Roles = false
            });

        }
        public UserRoles GetUserRole(String username) {
            var userRole = UserRole(username);
            return new UserRoles
            {
                UserName = userRole?.Result?.UserName ?? username,
                Dashboard = userRole?.Result?.Dashboard ?? true,
                Diagnostic = userRole?.Result?.Diagnostic ?? true,
                ClearCashIn = userRole?.Result?.ClearCashIn ?? false,
                ClearCards = userRole?.Result?.ClearCards ?? false,
                StandardCash = userRole?.Result?.StandardCash ?? false,
                DeviceConfiguration = userRole?.Result?.DeviceConfiguration ?? false,
                SystemParameters = userRole?.Result?.SystemParameters ?? false,
                Configuration = userRole?.Result?.Configuration ?? false,
                CopyLogs = userRole?.Result?.CopyLogs ?? false,
                ChangePassword = userRole?.Result?.ChangePassword ?? false,
                VDM = userRole?.Result?.VDM ?? false,
                Reboot = userRole?.Result?.Reboot ?? false,
                Roles = userRole?.Result?.Roles ?? false
            };
        }
        public Task ClearAll()
        {
            return ExecuteCommand(@"
				DELETE FROM [UserRoles]");
        }
        public Task ClearUserRole(String username)
        {
            return ExecuteCommand($@"
				DELETE
                FROM [UserRoles]
                WHERE
                    [UserName] == '{username}'");
        }

        private async Task<UserRoles> UserRole(String username)
        {
            var list = await ExecuteQuery<UserRoles>($@"
				SELECT
					 *
				FROM [UserRoles]
				WHERE 
					[UserName] == '{username}'");


            
            return list?.FirstOrDefault();
        }

        private Task Save(UserRoles userRoles)
        {
            return ExecuteCommand(@"
				INSERT INTO [UserRoles] (
					[UserName],
                    [Dashboard],
                    [Diagnostic],
                    [ClearCashIn],
                    [ClearCards],
                    [StandardCash],
                    [DeviceConfiguration],
                    [SystemParameters],
                    [Configuration],
                    [CopyLogs],
                    [ChangePassword],
                    [VDM],
                    [Reboot],
                    [Roles]

				) VALUES (
					@UserName,
                    @Dashboard,
                    @Diagnostic,
                    @ClearCashIn,
                    @ClearCards,
                    @StandardCash,
                    @DeviceConfiguration,
                    @SystemParameters,
                    @Configuration,
                    @CopyLogs,
                    @ChangePassword,
                    @VDM,
                    @Reboot,
                    @Roles
				)", userRoles);
        }

        public Task Update(UserRoles userRoles)
        {
            ClearUserRole(userRoles.UserName);
            return Save(userRoles);
        }
    }
}
