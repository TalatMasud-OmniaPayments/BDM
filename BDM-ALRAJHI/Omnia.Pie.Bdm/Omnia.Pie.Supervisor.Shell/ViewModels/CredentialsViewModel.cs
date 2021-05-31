using System.Windows.Controls;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	public class CredentialsViewModel
	{
		public CredentialsViewModel()
		{
			Login = new DelegateCommand<PasswordBox>(x => 
			{
				if (x.Password == "12345") { }
			});
		}

		public string UserName { get; set; }
		public string Password { get; set; }

		public DelegateCommand<PasswordBox> Login { get; }
	}
}