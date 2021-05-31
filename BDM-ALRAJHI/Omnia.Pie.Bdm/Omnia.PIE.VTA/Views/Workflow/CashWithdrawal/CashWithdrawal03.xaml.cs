using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CashWithdrawal03.xaml
    /// </summary>
    public partial class CashWithdrawal03 : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CashWithdrawal03"/> class.
        /// </summary>
        public CashWithdrawal03()
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
    }
}
