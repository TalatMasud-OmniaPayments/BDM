using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.ISO.UserManagement
{
    public class AccountTypes
    {
        public string acccountTypes { get; set; }

    }
    public class GetUserTypesResponse : ResponseBase<AccountTypes>
    {

        public List<string> UserTypes { get; set; }

    }
}
