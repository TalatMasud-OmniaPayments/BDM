using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{

    public class UserInformation
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
        public string ResponseCode { get; set; }

    }

    public class UserInfoResponse : ResponseBase<UserInformation>
    {
        public UserInformation UserInfo { get; set; }
    }

 
}
