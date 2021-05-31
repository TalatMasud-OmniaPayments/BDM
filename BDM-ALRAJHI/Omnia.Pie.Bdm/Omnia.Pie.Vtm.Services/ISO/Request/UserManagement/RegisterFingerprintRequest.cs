using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{

    public class UserAccount
    {
        public string Username { get; set; }
        public string Fingerprint { get; set; }
        public string FingerIndex { get; set; }
    }

    public class RegisterFingerprintRequest : RequestBase
    {
        public string Username { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
