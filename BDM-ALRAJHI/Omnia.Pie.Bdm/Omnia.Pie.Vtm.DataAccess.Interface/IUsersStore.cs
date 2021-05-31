using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface
{
    public interface IUsersStore
    {
        UserInfo ValidateUser(String username);
        int getLastUserId();
        void SaveNewUser(List<UserInfo> users);
    }
}
