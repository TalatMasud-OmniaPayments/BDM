using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels
{
    public interface ICreateUserViewModel : IBaseViewModel
    {
        String UserName { get; set; }
        String Name { get; set; }
        String Email { get; set; }
        String Mobile { get; set; }
        String Password { get; set; }
        //String UserType { get; set; }
        List<UserTypes> UserTypes { get; set; }
        UserTypes SelectedType { get; set; }
        ICommand UsertypeSelectedCommand { get; }
    }
}