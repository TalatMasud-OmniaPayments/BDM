using Omnia.PIE.VTA.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views
{
    /// <summary>
    /// Interaction logic for AccountHolderInfo.xaml
    /// </summary>
    public partial class AccountHolderInfo : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountHolderInfo"/> class.
        /// </summary>
        public AccountHolderInfo()
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
            this.DataContext = MainWindow.Instance.CustomerViewModel;
        }
    }
}
