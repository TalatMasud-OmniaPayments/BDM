using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{


    public interface ILoginView : IExpirableBaseViewModel
    {

        string SelectedUserName { get; set; }
        string Password { get; set; }

        

    }
}
