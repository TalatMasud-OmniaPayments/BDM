using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface ILoginViewModel : IExpirableBaseViewModel
    {
        Action TempButtonAction { get; set; }
        ICommand TempButtonCommand { get; }
        string SelectedUserName { get; set; }
        string Password { get; set; }
    }
}
