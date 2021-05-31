using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for SelectedAccountControl.xaml
    /// </summary>
    public partial class SelectedAccountControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedAccountControl"/> class.
        /// </summary>
        public SelectedAccountControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel.SelectedAccountNumber;
        }
    }
}
