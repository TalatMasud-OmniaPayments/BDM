using Microsoft.Practices.EnterpriseLibrary.Logging;
using Omnia.PIE.VTA.Core.Model;
using Omnia.PIE.VTA.ViewModelUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Omnia.PIE.VTA.ViewModels
{
	public class MainWindowViewModel : PageViewModelBase, IMainWndViewModel
	{
		public MainWindowViewModel()
		{
			InitializeAccountHolder();
		}

		private void InitializeAccountHolder()
		{
			if (AccountHolder == null)
				AccountHolder = new AccountHolder();

			AccountHolder.AccountHolderName = "Robert Padbury";
			AccountHolder.AccountNumber = "0124584";
			AccountHolder.IBANNumber = "10000000124584";
			AccountHolder.PassportNumber = "CD12548";
			AccountHolder.PassportExpiryDate = DateTime.Now.AddYears(3);
			AccountHolder.VisaNumber = "254887";
			AccountHolder.VisaExpiryDate = DateTime.Now.AddYears(3);
			AccountHolder.EmiratesIdNumber = "12458745548";
			AccountHolder.EmiratesIdExpiryDate = DateTime.Now.AddYears(3);

		}

		private IPageViewModel _currentPage;
		private List<IPageViewModel> _pages;

		private ICommand _CmdLogout;
		public ICommand CmdLogout
		{
			get
			{
				if (_CmdLogout == null)
					_CmdLogout = new RelayCommand(param => OnCmdLogout());
				return _CmdLogout;
			}
		}

		private void OnCmdLogout()
		{
			try
			{
				//Logger.Info(GetType(), "OnCmdLogout Started", "Logging Out");

				//Application.Current.Shutdown();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		public AccountHolder AccountHolder { get; set; }

		#region "Page Management"

		public IPageViewModel CurrentPage
		{
			get { return _currentPage; }
			set
			{
				if (_currentPage != value)
				{
					_currentPage = value;
					OnPropertyChanged(() => CurrentPage);
				}
			}
		}

		public List<IPageViewModel> Pages
		{
			get
			{
				if (_pages == null)
					_pages = new List<IPageViewModel>();

				return _pages;
			}
		}

		public void ChangePage(string pageID)
		{
			CurrentPage = Pages.FirstOrDefault(vm => vm.PageID == pageID);
		}

		#endregion
	}
}
