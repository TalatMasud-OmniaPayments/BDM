using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{
    public class GetNewUsersResponse : ResponseBase<UserInfo>
    {
        public List<UserInfo> NewUsers { get; set; }
    }
}
