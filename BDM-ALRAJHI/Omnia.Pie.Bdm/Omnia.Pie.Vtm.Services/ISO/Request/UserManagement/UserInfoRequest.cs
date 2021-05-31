using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{
    public class UserInfoRequest : RequestBase
    {
        public string Username { get; set; }
        public UpdateAccount UserAccount { get; set; }
    }
}
