using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;

namespace Omnia.PIE.VTA.Views
{
    /// <summary>
    /// Interaction logic for RemoteWindow.xaml
    /// </summary>
    public partial class RemoteWindow : Window
    {
        private IntPtr _handle;
        private static RemoteWindow remoteWindow;

        public static RemoteWindow Instance()
        {
            if (remoteWindow == null)
            {
                remoteWindow = new RemoteWindow();
            }

            return remoteWindow;
        }

        public IntPtr Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        public RemoteWindow()
        {
            InitializeComponent();
            _handle = videoPanel.Handle;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Owner = MainWindow.Instance;
            Handle = new WindowInteropHelper(this).Handle;
        }

		/// <summary>
		/// Handles the Click event of the BtnClose control. Close Window button
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the DplMainHead control. Drag the window head
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void DplMainHead_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Height == 500)
            {
                Top = 0;
                Left = 0;
                Height = SystemParameters.WorkArea.Height;
                Width = SystemParameters.WorkArea.Width;
            }
            else
            {
                Height = 500;
                Width = 500;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Height = MinHeight = 32;
            Width = MinWidth = 230;
            //this.WindowState = WindowState.Minimized;
        }

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}
