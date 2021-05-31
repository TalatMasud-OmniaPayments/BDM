

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.DelegateCommand;
    using System;
    using System.Timers;
    using System.Windows.Threading;
    public class LoginViewModel : ExpirableBaseViewModel, ILoginViewModel
    {
        public ICommand Login { get; }

        [Required(ErrorMessage = @"Required.")]
        public string SelectedUserName { get; set; }

        private string _password;

        [Required(ErrorMessage = @"Required.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public Action TempButtonAction { get; set; }
        public ICommand _tempButtonCommand { get; set; }

        public ICommand TempButtonCommand
        {
            get
            {
                if (_tempButtonCommand == null)
                {
                    _tempButtonCommand = new DelegateCommand(
                          () =>
                          {
                              TempButtonAction?.Invoke();
                          });
                }

                return _tempButtonCommand;
            }
        }
        public void Dispose()
        {
            
        }
    }
}
