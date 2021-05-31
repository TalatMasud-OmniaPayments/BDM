namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.DelegateCommand;
    using System;
    using System.Timers;
    using System.Windows.Threading;

    public class BusinessUserMainMenuViewModel : ExpirableBaseViewModel, IBusinessUserMainMenuViewModel
    {
        public bool IsSelfModeCalling { get; set; } = false;
        public Action DepositAction { get; set; }


        private ICommand _depositCommand;
        public ICommand DepositCommand
        {
            get
            {
                if (_depositCommand == null)
                {
                    _depositCommand = new DelegateCommand(
                          () =>
                          {
                              DepositAction?.Invoke();
                          });
                }

                return _depositCommand;
            }

        }

        public Action ReprintAction { get; set; }

        private ICommand _reprintCommand { get; set; }
        public ICommand ReprintCommand
        {
            get
            {
                if (_reprintCommand == null)
                {
                    _reprintCommand = new DelegateCommand(
                          () =>
                          {
                              ReprintAction?.Invoke();
                          });
                }

                return _reprintCommand;
            }
        }

        public Action ChangePasswordAction { get; set; }

        private ICommand _changePasswordCommand { get; set; }
        public ICommand ChangePasswordCommand
        {
            get
            {
                if (_changePasswordCommand == null)
                {
                    _changePasswordCommand = new DelegateCommand(
                          () =>
                          {
                              ChangePasswordAction?.Invoke();
                          });
                }

                return _changePasswordCommand;
            }
        }
        public void Dispose()
        {

        }
    }
}
