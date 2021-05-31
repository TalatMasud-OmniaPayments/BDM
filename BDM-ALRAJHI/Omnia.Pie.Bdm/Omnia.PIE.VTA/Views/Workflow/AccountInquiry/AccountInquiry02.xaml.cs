using Omnia.PIE.VTA.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AccountInquiry02.xaml
    /// </summary>
    public partial class AccountInquiry02 : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInquiry02"/> class.
        /// </summary>
        public AccountInquiry02()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }

        /// <summary>
        /// Sets the initialize stage.
        /// </summary>
        public void SetInitializeStage()
        {
            grdAccounts.Visibility = Visibility.Collapsed;
            grdMessage.Visibility = Visibility.Visible;
        }
    }
}
