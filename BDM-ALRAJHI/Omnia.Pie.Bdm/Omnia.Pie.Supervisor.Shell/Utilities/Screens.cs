namespace Omnia.Pie.Supervisor.Shell.Utilities
{
    using Microsoft.Practices.Unity;
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Supervisor.Shell.ViewModels;
    using Omnia.Pie.Supervisor.Shell.Views;
    using Omnia.Pie.Vtm.Devices.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using HWND = System.IntPtr;

    public static class Screens
    {
        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());
        //private static IDeviceSensors _deviceSensors = ServiceLocator.Instance.Resolve<IDeviceSensors>();

        private static Window _mainWnd;
        public static SupervisorViewModel supervisoryWindow;

        private static readonly List<OutOfServiceView> OutOfServiceViews = new List<OutOfServiceView>();

        public delegate void dgEventRaiser();
        public static event dgEventRaiser OnLoggoutSupervisoryUser;
        public static event dgEventRaiser OnDoorAccessed;

        public static Screen GetScreen(this Window wnd) =>
            Screen.FromHandle(new WindowInteropHelper(wnd).Handle);

        public static void SetScreen(this Window wnd, Screen screen)
        {
            if (screen == null)
                return;
            wnd.WindowStartupLocation = WindowStartupLocation.Manual;
            wnd.Left = screen.WorkingArea.Left;
            wnd.Top = screen.WorkingArea.Top;
            wnd.Width = screen.WorkingArea.Width;
            wnd.Height = screen.WorkingArea.Height;
        }

        public static Screen NextScreen(this Window wnd, Screen screen = null)
        {
            if (screen == null)
                screen = wnd.GetScreen();

            int i;
            for (i = 0; i < Screen.AllScreens.Length; i++)
                if (Screen.AllScreens[i].Bounds == screen.Bounds)
                    break;

            return Screen.AllScreens.Length == 0 ? null : Screen.AllScreens[(i + 1) % Screen.AllScreens.Length];
        }

        public static void SetDoorAccessed()
        {
            OnDoorAccessed();
        }
        public static void SetLogin(bool isSupervisorMode, string username)
        {
            //supervisoryWindow.Context.DoorsOpen = true;
            supervisoryWindow.Context.Login(isSupervisorMode, username);

            Console.WriteLine("Logged in");
        }

        public static void SetLogoutFromMainApp()
        {
            supervisoryWindow.Context.Logout();
            Screens.OutOfServiceViewsHide();
            //_channelManagementService.InsertEventAsync("InService", "True");

            //_channelManagementService.InsertEventAsync("Logged out without maintenance", "True");
            Screens.UpdateToMainApp(true);
        }
        public static void SetLogout()
        {
            supervisoryWindow.Context.steps.isCitStarted = false;
            OnLoggoutSupervisoryUser();
        }

        public static void SetMainScreen(this Window wnd)
        {
            _mainWnd = wnd;
            if (Screen.AllScreens.Length > 1)
                _mainWnd.Topmost = true;

            //UpdateToMainApp(true);
            _mainWnd.SetScreen(Screen.AllScreens.LastOrDefault());
            //UpdateToMainApp(true);
        }

        public static void OutOfServiceViewsShow()
        {
            if (OutOfServiceViews.Count == 0)
            {
                //Get all screens except last - where we show supervisor and VDM
                var screens = Screen.AllScreens.Take(Screen.AllScreens.Length - 1);

                foreach (var screen in screens)
                {
                    Logger.Info($"screen {screen.Bounds} Name: {screen.DeviceName}");

                    var outOfServiceView = new OutOfServiceView();
                    OutOfServiceViews.Add(outOfServiceView);
                    outOfServiceView.SetScreen(screen);

                    Logger.Info($"outOfServiceView.X={outOfServiceView.Left}");
                    Logger.Info($"outOfServiceView.Y={outOfServiceView.Top}");
                }
            }

            foreach (var outOfServiceView in OutOfServiceViews)
                outOfServiceView.Show();

            _mainWnd.Focus();
        }

        public static void OutOfServiceViewsHide()
        {
            foreach (var outOfServiceView in OutOfServiceViews)
                outOfServiceView.Hide();

            _mainWnd.Focus();
        }

        public static void UpdateToMainApp(bool mainApp)
        {

           /* var opStatus = _deviceSensors.GetOperatorStatus();

            if (opStatus == SensorsStatus.Supervisor)
            {
                mainApp = false;
            }*/

            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                //IntPtr HWND_TOPMOST = new IntPtr(0);
                //const UInt32 SWP_NOSIZE = 0x0001;
                //const UInt32 SWP_NOMOVE = 0x0002;
                //const UInt32 SWP_SHOWWINDOW = 0x0040;
                IntPtr handle = window.Key;
                string title = window.Value;

                Console.WriteLine("{0}: {1}", handle, title);

                
                if (mainApp && title == "MainWindow") {
                    //SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                    //Window window1 = (Window)HwndSource.FromHwnd((IntPtr)handle).RootVisual;
                    //window1.Topmost = true;
                    SwitchToThisWindow(handle, true);
                    break;
                }
                else if (!mainApp && title == "supervisory")
                {

                    SwitchToThisWindow(handle, true);
                    //SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                    //Window window1 = (Window)HwndSource.FromHwnd((IntPtr)handle).RootVisual;
                    //window1.Topmost = true;
                    break;
                }

            }
        }

        [DllImport("User32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    }


    
/// <summary>Contains functionality to get all the open windows.</summary>
    public static class OpenWindowGetter
    {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {

         

            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        

    }
}