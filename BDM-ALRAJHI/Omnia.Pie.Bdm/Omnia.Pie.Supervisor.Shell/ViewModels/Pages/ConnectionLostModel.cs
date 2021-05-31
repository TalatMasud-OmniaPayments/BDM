using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
    public class ConnectionLostModel : PageViewModel
    {
        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.Dashboard == false ? false : false);
    }
}
