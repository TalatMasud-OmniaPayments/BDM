using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.Authentication
{
    public class UserNamePasswordRequest : RequestBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}