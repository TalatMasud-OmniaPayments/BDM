namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.DelegateCommand;
    using System;
    using System.Timers;
    using System.Windows.Threading;

    public class ChangePasswordViewModel : ExpirableBaseViewModel, IChangePasswordViewModel
    {
        public String oldPassword { get; set; }
        public String newPassword { get; set; }

        public void Dispose()
        {

        }
    }
}
