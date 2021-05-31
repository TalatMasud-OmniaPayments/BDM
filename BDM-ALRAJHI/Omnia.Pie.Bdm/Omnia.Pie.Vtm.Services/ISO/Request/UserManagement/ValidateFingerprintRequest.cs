using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{
    public class ValidateFingerprintRequest : RequestBase
    {
        public string Username { get; set; }
        public string Fingerprint { get; set; }
    }
}
