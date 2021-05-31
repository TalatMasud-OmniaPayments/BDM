using Microsoft.Practices.EnterpriseLibrary.Logging;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.PIE.VTA.Common;
using Omnia.PIE.VTA.Core.Model;
using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Omnia.PIE.VTA.Views
{
	public partial class Devices : UserControl
	{
		public DevicesViewModel ViewModel = new DevicesViewModel();

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

		public Devices()
		{
			InitializeComponent();

			NetworkStatusObserver.VPNDropped += NetworkStatusObserver_VPNDropped;
			NetworkStatusObserver.InternetDropped += NetworkStatusObserver_InternetDropped;
			NetworkStatusObserver.InternetOn += NetworkStatusObserver_InternetOn;
		}

		private void NetworkStatusObserver_InternetOn(object sender, EventArgs e)
		{
			try
			{
				Dispatcher.Invoke(() =>
				{
					InterNet.Background = Brushes.GreenYellow;
				});
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void NetworkStatusObserver_InternetDropped(object sender, EventArgs e)
		{
			try
			{
				Dispatcher.Invoke(() =>
				{
					InterNet.Background = Brushes.Red;
				});
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void NetworkStatusObserver_VPNDropped(object sender, EventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				if (VPN.Background == Brushes.Red)
				{
					VPN.Background = Brushes.GreenYellow;
				}
				else
				{
					VPN.Background = Brushes.Red;
				}
			});
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.DataContext = ViewModel;
		}

		public void SetDevicesInitializeState()
		{
			try
			{
				ViewModel.CallDuration.Duration = new TimeSpan(0);
				ViewModel.CashDispenser.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.CardReader.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Scanner.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.ReceiptPrinter.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.StatementPrinter.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.PinPad.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Doors.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Sensors.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Camera.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.RFIDReader.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Auxiliaries.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.SignPad.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.Indicators.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.DVCSignal.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.VDM.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.VFD.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.TMD.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.ChequeScanner.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.JournalPrinter.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.CashAcceptor.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.IDScanner.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.A4Printer.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.A4Scanner.DeviceStatus = nameof(RTDeviceStatus.Offline);
				ViewModel.CashCassettes = new ObservableCollection<Cassette>();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		public void StopNetworObserver()
		{
			NetworkStatusObserver.StopObserver();
		}

		private void btnPopout_Click(object sender, RoutedEventArgs e)
		{
			btnPopout.Visibility = Visibility.Collapsed;
			MainWindow.Instance.btnRightGridColumn.Visibility = Visibility.Collapsed;
			MainWindow.Instance.btnRightGridColumn_Click(sender, e);
			DevicesPopup devPopup = new DevicesPopup();
			devPopup.Show();
		}

		public void PopIn()
		{
			btnPopout.Visibility = Visibility.Visible;
			MainWindow.Instance.btnRightGridColumn.Visibility = Visibility.Visible;
			MainWindow.Instance.btnRightGridColumn_Click(null, null);
			//devPopup.Hide();
		}
	}
}