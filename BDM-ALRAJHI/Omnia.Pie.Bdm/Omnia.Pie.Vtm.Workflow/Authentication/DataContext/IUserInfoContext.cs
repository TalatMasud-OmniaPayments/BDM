using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Vtm.Devices.Interface.Entities;

    public interface IUserInfoContext
    {
        string Name { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string Mobile { get; set; }
        string UserType { get; set; }
    }
}
