using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Omnia.PIE.VTA.Common;
using Omnia.PIE.VTA.Utilities;
using Omnia.PIE.VTA.Views.WF;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Omnia.PIE.VTA.Views
{
	public partial class LogOnWindow : Window
	{
		[Dependency]
		public IUnityContainer UnityContainer { get; set; }

		public eSpaceMediaOcx eSpaceMediaOcxApi
		{
			get
			{
				return eSpaceMediaOcx.Instance();
			}
		}

		private NetworkStatusObserver _NetworkStatusObserver;
		public NetworkStatusObserver NetworkStatusObserver
		{
			get
			{
				if (_NetworkStatusObserver == null)
					_NetworkStatusObserver = new NetworkStatusObserver();

				return _NetworkStatusObserver;
			}
			set
			{
				_NetworkStatusObserver = value;
			}
		}

		public LogOnWindow()
		{
			InitializeComponent();

			eSpaceMediaOcx.InitializeSuccessEvent += ESpaceMediaOcx_InitializeSuccessEvent;
			eSpaceMediaOcx.InitializeOcxFailedEvent += ESpaceMediaOcx_InitializeOcxFailedEvent;
			eSpaceMediaOcx.LoginSuccessEvent += ESpaceMediaOcx_LoginSuccessEvent;
			eSpaceMediaOcx.ChangeInitialPassword += ESpaceMediaOcx_ChangeInitialPassword;
		}

		private void ESpaceMediaOcx_ChangeInitialPassword(object sender, EventArgs e)
		{
			LoginGrid.Visibility = Visibility.Collapsed;
			ChangePasswordGrid.Visibility = Visibility.Visible;
		}

		private void ESpaceMediaOcx_InitializeOcxFailedEvent()
		{
			loader.Visibility = Visibility.Collapsed;
			loginMsg.Text = "Failed to initialize the Ocx.";
		}

		private void ESpaceMediaOcx_LoginSuccessEvent()
		{
			InitializeMainWindow();
		}

		private void ESpaceMediaOcx_InitializeSuccessEvent()
		{
			btnLogin.IsEnabled = true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			NetworkStatusObserver.VPNDropped += NetworkStatusObserver_VPNDropped;
			NetworkStatusObserver.InternetDropped += NetworkStatusObserver_InternetDropped;
			NetworkStatusObserver.InternetOn += NetworkStatusObserver_InternetOn;

			eSpaceMediaOcxApi.SetTellerCallReleaseEnterMode();
			txtTellerNumber.Focus();

			txtVersion.Text = ApplicationVersion.Value;
		}

		private void NetworkStatusObserver_InternetOn(object sender, EventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
			{
				InterNet.Background = Brushes.GreenYellow;
			}));
		}

		private void NetworkStatusObserver_InternetDropped(object sender, EventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
			{
				InterNet.Background = Brushes.Red;
			}));
		}

		private void NetworkStatusObserver_VPNDropped(object sender, EventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
			{
				if (VPN.Background == Brushes.Red)
				{
					VPN.Background = Brushes.GreenYellow;
				}
				else
				{
					VPN.Background = Brushes.Red;
				}
			}));
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			eSpaceMediaOcxApi.DisposeObject();
			this.Close();
			NetworkStatusObserver.StopObserver();
			Application.Current.Shutdown();
		}

		private void BtnLogin_Click(object sender, RoutedEventArgs e)
		{
			loader.Visibility = Visibility.Visible;

			if (!eSpaceMediaOcx.IsInternetAvailable())
			{
				loginMsg.Text = "Your Internet connection is not working.";
				loader.Visibility = Visibility.Collapsed;
				return;
			}

			var tellerId = txtTellerNumber.Text.ToString();

			if (string.IsNullOrEmpty(tellerId))
			{
				loginMsg.Text = "The employee ID is mandatory.";
				Logger.Writer.Info(GetType().ToString() + " Employee ID shoud not empty.");
				loader.Visibility = Visibility.Collapsed;
				txtTellerNumber.Focus();
				return;
			}

			var rgx = new Regex(@"^[0-9]+$");

			if (!rgx.IsMatch(tellerId))
			{
				loginMsg.Text = "Incorrect employee ID.";
				Logger.Writer.Info(GetType().ToString() + " tellerId not right.");
				loader.Visibility = Visibility.Collapsed;
				txtTellerNumber.Focus();
				return;
			}

			GlobalInfo.TellerId = tellerId;
			GlobalInfo.TellerName = System.Security.Principal.WindowsIdentity.GetCurrent()?.Name?.Split('\\')?.Last()?.Replace("_", " ") ?? "";
			GlobalInfo.TellerPassword = txtPassword.Password;
			GlobalInfo.PhoneNumber = txtPhoneNumber.Text;
			tellerId = string.Empty;

			Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
			{
				LogOnServerActive(txtTellerNumber.Text, txtPassword.Password, txtPhoneNumber.Text, txtPhonePassword.Password);
			}));
		}

		private void DplMainHead_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			DragMove();
		}

		public short LogOnServerActive(string tellerId, string password, string phoneNum, string phonePass)
		{
			short returnCode = -1;

			try
			{
				if (!string.IsNullOrEmpty(tellerId) && tellerId.Length < 1000)
				{
					Logger.Writer.Info(GetType().ToString() + " tellerId = " + tellerId);
				}

				returnCode = eSpaceMediaOcxApi.Login(tellerId, password, phoneNum, phonePass);

				if (returnCode > 0 && returnCode != 0)
				{
					eSpaceMediaOcxApi.Logout();
					var msg = string.Empty;

					if (returnCode == 3)
					{
						msg = "invalid VPN.";
					}
					else if (returnCode == 7)
					{
						msg = "invalid credentials.";
					}
					else if (returnCode == 8)
					{
						msg = "credentials already logged on.";
					}

					loginMsg.Text = string.Format("Logon failed {0}", msg);
					loader.Visibility = Visibility.Collapsed;
				}
				else
				{
					if (MainWindow.Instance != null)
						MainWindow.Instance.TellerStatus.BackGround = Brushes.GreenYellow;
					loader.Visibility = Visibility.Collapsed;
					loginMsg.Text = "";
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}

			return returnCode;
		}

		public void InitializeMainWindow()
		{
			try
			{
				var mainWindow = UnityContainer.Resolve<MainWindow>();
				((App)Application.Current).Shell = mainWindow;
				Application.Current.MainWindow = mainWindow;

				mainWindow.ShowInTaskbar = true;
				mainWindow.Show();
				mainWindow.Activate();

				GlobalInfo.TellerState = TellerState.Idle;

				NetworkStatusObserver.StopObserver();
				Close();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
				eSpaceMediaOcx.Instance().Logout();
			}
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
				e.Handled = true;
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == System.Windows.Input.Key.Enter)
				{
					BtnLogin_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnChangePassword_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtNewPassword.Password) || string.IsNullOrEmpty(txtConfirmNewPassword.Password))
			{
				loginMsg.Text = "New Password and Confirm New Password are required.";
				return;
			}

			if (txtNewPassword.Password == txtConfirmNewPassword.Password)
			{
				var result = eSpaceMediaOcxApi.ChangePassword(txtOldPassword.Password, txtNewPassword.Password);

				if (result == 0)
				{
					LoginGrid.Visibility = Visibility.Visible;
					ChangePasswordGrid.Visibility = Visibility.Collapsed;
					loginMsg.Text = "Password changed successfully.";

					InitializeMainWindow();
				}
				else if(result == 1)
				{
					loginMsg.Text = "Try a different password.";
				}
			}
			else
			{
				loginMsg.Text = "New Password and Confirm New Password does not match.";
			}
		}
	}
}