using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Omnia.PIE.VTA.Views
{
    /// <summary>
    /// Interaction logic for DevicesPopup.xaml
    /// </summary>
    public partial class DevicesPopup : Window
    {
        private static DevicesPopup _DevicesPopupWindow;
        public static DevicesPopup Instance()
        {
            if (_DevicesPopupWindow == null)
            {
                _DevicesPopupWindow = new DevicesPopup();
            }

            return _DevicesPopupWindow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicesPopup"/> class.
        /// </summary>
        public DevicesPopup()
        {
            InitializeComponent();
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

        private void btnPopIn_Click(object sender, RoutedEventArgs e)
        {
            DevicesControl.PopIn();
            this.Close();
        }
    }
}
