using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface IChangePasswordViewModel : IExpirableBaseViewModel
    {
        String oldPassword { get; set; }
        String newPassword { get; set; }
    }
}
