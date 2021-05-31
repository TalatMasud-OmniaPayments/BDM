using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	public class TermsAndConditionsViewModel : BaseViewModel, ITermsAndConditionsViewModel
	{
		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		public bool IsChecked { get ; set ; }

		public Action AcceptAction { get; set; }

		private ICommand _acceptCommand;
		public ICommand AcceptCommand
		{
			get
			{
				if (_acceptCommand == null)
				{
					_acceptCommand = new DelegateCommand
					(
						() =>
						AcceptAction?.Invoke(),
						() => IsChecked == true
					);
				}

				return _acceptCommand;
			}
		}
		public string CustomerName { get; set; }
		public void Dispose()
		{

		}
	}
}