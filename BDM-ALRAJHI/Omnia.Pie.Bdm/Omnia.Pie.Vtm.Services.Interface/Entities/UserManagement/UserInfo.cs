using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
    public class UserInfo
    {

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
        public string ResponseCode { get; set; }
        public string LastLogin { get; set; }
        public string Salt { get; set; }
        public string PasswordDate { get; set; }
        public string Activated { get; set; }


        /*public UserInfo(string userId, string name, string username, string password, string email, string mobile, string userType, string responseCode, string lastLogin, string salt, string passwordDate, string activated)
        {
            UserId = userId;
            Name = name;
            Username = username;
            Password = password;
            Email = email;
            Mobile = mobile;
            UserType = userType;
            ResponseCode = responseCode;
            LastLogin = lastLogin;
            Salt = salt;
            PasswordDate = passwordDate;
            Activated = activated;
        }*/
    }


    
}
