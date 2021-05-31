using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
    public class UserRoles
    {
        public string UserName { get; set; }
        public bool Dashboard { get; set; }
        public bool Diagnostic { get; set; }
        public bool ClearCashIn { get; set; }
        public bool ClearCards { get; set; }
        public bool StandardCash { get; set; }
        public bool DeviceConfiguration { get; set; }
        public bool SystemParameters { get; set; }
        public bool Configuration { get; set; }
        public bool CopyLogs { get; set; }
        public bool ChangePassword { get; set; }
        public bool VDM { get; set; }
        public bool Reboot { get; set; }
        //public bool Roles { get; set; }
        //public bool role1 public int MyProperty { get; set; }

        private bool _roles;
        public bool Roles
        {
            get { return _roles; }
            set
            {
               
                    if (UserName.ToLower() == "supervisor")
                {
                    _roles = true;
                }
                else {
                    _roles = false;
                }
                    
            }
        }

    }
}
