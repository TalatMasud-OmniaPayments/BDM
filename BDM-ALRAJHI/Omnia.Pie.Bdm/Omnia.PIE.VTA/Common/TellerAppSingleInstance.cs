using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Omnia.PIE.VTA.Common
{
	public static class TellerAppSingleInstance
	{
		/// <summary>
		/// Processing single instance in <see cref="SingleInstanceModes"/> <see cref="SingleInstanceModes.ForEveryUser"/> mode.
		/// </summary>
		internal static void Make()
		{
			Make(SingleInstanceModes.ForEveryUser);
		}

		/// <summary>
		/// Processing single instance.
		/// </summary>
		/// <param name="singleInstanceModes"></param>
		internal static void Make(SingleInstanceModes singleInstanceModes)
		{
			var appName = Application.Current.GetType().Assembly.ManifestModule.ScopeName;
			var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
			var keyUserName = windowsIdentity != null ? windowsIdentity.User.ToString() : string.Empty;

			// Be careful! Max 260 chars!
			var eventWaitHandleName = string.Format("{0}{1}", appName, singleInstanceModes == SingleInstanceModes.ForEveryUser ? keyUserName : string.Empty);

			try
			{
				using (var eventWaitHandle = EventWaitHandle.OpenExisting(eventWaitHandleName))
				{
					// It informs first instance about other startup attempting.
					eventWaitHandle.Set();
				}

				// Let's terminate this posterior startup. For that exit no interceptions.
				Environment.Exit(0);
			}
			catch
			{
				// It's first instance. Register EventWaitHandle.
				using (var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventWaitHandleName))
				{
					ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, OtherInstanceAttemptedToStart, null, Timeout.Infinite, false);
				}

				RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
			}
		}

		private static void OtherInstanceAttemptedToStart(object state, bool timedOut)
		{
			RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
			Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				try
				{
					Application.Current.MainWindow.Topmost = true;
					Application.Current.MainWindow.Activate();
					Application.Current.MainWindow.Focus();
					Application.Current.MainWindow.Topmost = false;
				}
				catch { }
			}));
		}

		internal static DispatcherTimer AutoExitAplicationIfStartupDeadlock;

		public static void RemoveApplicationsStartupDeadlockForStartupCrushedWindows()
		{
			Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				AutoExitAplicationIfStartupDeadlock =
					new DispatcherTimer(
						TimeSpan.FromSeconds(6),
						DispatcherPriority.ApplicationIdle,
						(o, args) =>
						{
							if (Application.Current.Windows.Cast<Window>().Count(window => !Double.IsNaN(window.Left)) == 0)
							{
								Environment.Exit(0); // For that exit no interceptions.
							}
						},
						Application.Current.Dispatcher
					);
			}), DispatcherPriority.ApplicationIdle);
		}
	}

	public enum SingleInstanceModes
	{
		NotInited = 0,
		ForEveryUser,
	}
}