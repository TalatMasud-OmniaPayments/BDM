namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels.LoginView
{
    using Omnia.Pie.Supervisor.Shell.Configuration;
    using Omnia.Pie.Supervisor.Shell.Utilities;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.DelegateCommand;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Linq;
    using System.Windows.Input;
    using Omnia.Pie.Vtm.Framework.Base;
    using Omnia.Pie.Vtm.Bootstrapper.Interface.LoginView;

    public class LoginViewViewModel : ILoginView
    {
        string ILoginView.UserName { get; set; }
        string ILoginView.Password { get; set; }
        public ICommand Login { get; }

        [Required(ErrorMessage = @"Required.")]
        public string SelectedUserName { get; set; }

        private string _password;

        [Required(ErrorMessage = @"Required.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _password; }
            //set { SetProperty(ref _password, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            //set { SetProperty(ref _errorMessage, value); }
        }
        LoginViewViewModel()
        {

        }
    }
}
