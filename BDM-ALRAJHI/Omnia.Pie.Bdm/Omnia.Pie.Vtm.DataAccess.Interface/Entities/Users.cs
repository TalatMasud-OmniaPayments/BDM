using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
    public class Users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int user_group_id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string last_login { get; set; }
        public string password_date { get; set; }
        public string password_blocked_until { get; set; }
        public bool password_blocked { get; set; }
        public string activated { get; set; }
        public string activation_code { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string userType { get; set; }
        public string salt { get; set; }


    }
}
