using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{
    public class GetUserTypesRequest : RequestBase
    {
        public string Username { get; set; }
    }
}
