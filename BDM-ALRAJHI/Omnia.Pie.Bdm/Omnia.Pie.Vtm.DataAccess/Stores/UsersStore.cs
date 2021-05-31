using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Stores
{
    internal class UsersStore : StoreBase, IUsersStore
    {


        public UsersStore(IResolver container) : base(container)
        {
            ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [Users] (
					[id]                        INTEGER      NOT NULL UNIQUE PRIMARY KEY,
                    [username]                  NVARCHAR(64) NOT NULL UNIQUE,
					[password]                  NVARCHAR(64),
                    [password_date]             TEXT,
                    [salt]                      NVARCHAR(64),
                    [UserType]                  NVARCHAR(64),
                    [user_group_id]             INTEGER,
                    [name]                      NVARCHAR(150),
                    [mobile]                    NVARCHAR(64),
                    [email]                     NVARCHAR(50),
                    [last_login]                TEXT,
                    [password_blocked_until]    TEXT,
                    [password_blocked]          BOOLEAN,
                    [activated]                 TEXT,
                    [activation_code]           NVARCHAR(50),
                    [created]                   TEXT,
                    [updated]                   TEXT
                    )");

            //SaveAdminUser();
            //SaveuserUser();

        }
        private Task Save(Users user)
        {
            Task isSaved = ExecuteCommand(@"
				INSERT INTO [Users] (
					[id],
                    [username],
                    [password],
                    [salt],
                    [UserType],
                    [user_group_id],
                    [name],
                    [mobile],
                    [email],
                    [last_login],
                    [password_date],
                    [password_blocked_until],
                    [password_blocked],
                    [activated],
                    [activation_code],
                    [created],
                    [updated]

				) VALUES (
                    @id,
					@username,
                    @password,
                    @salt,
                    @userType,                    
                    @user_group_id,
                    @name,
                    @mobile,
                    @email,
                    @last_login,
                    @password_date,
                    @password_blocked_until,
                    @password_blocked,
                    @activated,
                    @activation_code,
                    @created,
                    @updated
				)", user);

            return isSaved;
        }
        //1	admin	0945BDC74427733DA2ABA3C4EAB61743842DBAC3308C57C317DD7CF0BF7D802F	1	Talat	123213	admin@geidea.net	2020-07-22 12:07:56.0000000	2020-03-17 11:35:27.0000000	NULL	NULL	True	028233	2020-03-11 15:20:27.6785130	2020-07-22 12:07:56.3290000
        //12 user	951465E20EBBC92C46677301F80F2FC003F69925E833318C2E2C6F0C8C6CF30D	2	Business User	20000000000	user @geidea.net 2020-07-27 13:47:19.0000000	2020-07-22 13:51:41.0000000	NULL NULL    True    141462	2020-03-11 15:40:48.5530750	2020-07-27 13:47:18.7110000
        public void SaveNewUser(List<UserInfo> users)
        {

            foreach (UserInfo newUser in users)
            {
                var user = ToUsers(newUser);
                Save(user);
            }

            //ToUsers()
            //Save(user);

        }
        /*public void SaveAdminUser()
        {


            Save(new Users
            {
                id = 1,
                username = "cit",
                password = "0945BDC74427733DA2ABA3C4EAB61743842DBAC3308C57C317DD7CF0BF7D802F",
                user_group_id = 3,
                name = "Talat",
                mobile = "123213",
                email = "admin@geidea.net",
                last_login = "2020-07-22 12:07:56.0000000",
                password_date = "2020-03-17 11:35:27.0000000",
                password_blocked_until = null,
                password_blocked = false,
                activated = true,
                activation_code = "028233",
                created = "2020-03-11 15:20:27.6785130",
                updated = "2020-07-22 12:07:56.3290000",
            });

        }*/

        private Users ToUsers(UserInfo user) => new Users
        {
            id = Convert.ToInt32(user.UserId) ,
            username = user.Username,
            password = user.Password,
            password_date = user.PasswordDate,
            userType = user.UserType,
            
            name = user.Name,
            mobile = user.Mobile,
            email = user.Email,
            salt = user.Salt,
            user_group_id = 0,
            last_login = "2020-07-22 12:07:56.0000000",
            
            password_blocked_until = null,
            password_blocked = false,
            activated = user.Activated,
            activation_code = "028233",
            created = "2020-03-11 15:20:27.6785130",
            updated = "2020-07-22 12:07:56.3290000",
        };

        public UserInfo ValidateUser(String username)
        {
            var user = getUser(username);

            return ToUserInfo(user);

        }

        private UserInfo ToUserInfo(Task<Users> user) => new UserInfo
        {
            Name = user?.Result?.name,
            Username = user?.Result?.username,
            Email = user?.Result?.email,
            Mobile = user?.Result?.mobile,
            UserType = user?.Result?.userType,
            ResponseCode = getResponseCode(user?.Result?.username ?? ""),
        };

        private string getUserType(int? groupId) {

            if (groupId == 1)
            {
                return "Supervisory";
            }
            else if (groupId == 2)
            {
                return "Business";
            }
            else if (groupId == 3)
            {
                return "CIT";
            }
            else {
                return "SLM";
            }
        }
        private string getResponseCode(string username)
        {

            if (username.Length > 0)
            {
                return "000";
            }
            else
            {
                return "";
            }
        }
        private async Task<Users> getUser(String username)
        {
            var list = await ExecuteQuery<Users>($@"
				SELECT
					 *
				FROM [Users]
				WHERE 
					[username] == '{username}'");



            return list?.FirstOrDefault();
        }

        public int getLastUserId()
        {
            var user = getLastUserIdFromLocal();

            return user?.Result?.id ?? 0;

        }
        private async Task<Users> getLastUserIdFromLocal()
        {
            var list = await ExecuteQuery<Users>($@"
				SELECT
					 id
				FROM [Users]
				ORDER BY id DESC LIMIT 1");

            /*var list = await ExecuteQuery<Users>($@"
				SELECT
					 MAX([id])
				FROM [Users]");*/



            return list?.FirstOrDefault();
        }


        /*public Task Update(Users user)
        {
            ClearUserRole(userRoles.UserName);
            return Save(userRoles);
        }*/

    }
}
