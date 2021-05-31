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
    internal class AccountsStore : StoreBase
    {
        public AccountsStore(IResolver container) : base(container)
        {
            ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [Terminal_Accounts] (
					[id]                        INTEGER      NOT NULL UNIQUE PRIMARY KEY,
                    [terminalId]                  TEXT,
					[AccountNumber]                  TEXT
                    )");

            //SaveAdminUser();
            //SaveuserUser();

        }
        private Task Save(TerminalAccount terminalAccount)
        {
            Task isSaved = ExecuteCommand(@"
				INSERT INTO [Terminal_Accounts] (
					[id],
                    [terminalId],
                    [AccountNumber]

				) VALUES (
                    @id,
					@terminalId,
                    @AccountNumber
				)", terminalAccount);

            return isSaved;
        }
        //1	admin	0945BDC74427733DA2ABA3C4EAB61743842DBAC3308C57C317DD7CF0BF7D802F	1	Talat	123213	admin@geidea.net	2020-07-22 12:07:56.0000000	2020-03-17 11:35:27.0000000	NULL	NULL	True	028233	2020-03-11 15:20:27.6785130	2020-07-22 12:07:56.3290000
        //12 user	951465E20EBBC92C46677301F80F2FC003F69925E833318C2E2C6F0C8C6CF30D	2	Business User	20000000000	user @geidea.net 2020-07-27 13:47:19.0000000	2020-07-22 13:51:41.0000000	NULL NULL    True    141462	2020-03-11 15:40:48.5530750	2020-07-27 13:47:18.7110000
        public void SaveNewUser(List<TerminalAccountInfo> tAccountInfo)
        {

            foreach (TerminalAccountInfo newAccount in tAccountInfo)
            {
                var account = ToAccount(newAccount);
                Save(account);
            }

            //ToUsers()
            //Save(user);

        }
        

        private TerminalAccount ToAccount(TerminalAccountInfo tAccount) => new TerminalAccount
        {
            id = Convert.ToInt32(tAccount.id),
            AccountNumber = tAccount.AccountNumber,
            terminalId = tAccount.terminalId,
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

        private string getUserType(int? groupId)
        {

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
            else
            {
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