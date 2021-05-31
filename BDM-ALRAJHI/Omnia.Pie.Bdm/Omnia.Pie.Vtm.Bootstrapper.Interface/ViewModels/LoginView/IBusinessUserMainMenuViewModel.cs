using System;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface IBusinessUserMainMenuViewModel : IExpirableBaseViewModel
    {
        bool IsSelfModeCalling { get; set; }

        Action DepositAction { get; set; }
        ICommand DepositCommand { get; }

        Action ReprintAction { get; set; }
        ICommand ReprintCommand { get; }

        Action ChangePasswordAction { get; set; }


        ICommand ChangePasswordCommand { get; }     //this is a test change1
    }
}
