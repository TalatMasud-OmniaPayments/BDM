using Omnia.PIE.VTA.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CashWithdrawal10.xaml
    /// </summary>
    public partial class CashWithdrawal10 : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CashWithdrawal10"/> class.
        /// </summary>
        public CashWithdrawal10(WorkFlowViewModel _ViewModel)
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
