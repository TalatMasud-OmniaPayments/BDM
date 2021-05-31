using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CashWithdrawal11.xaml
    /// </summary>
    public partial class CashWithdrawal11 : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CashWithdrawal11"/> class.
        /// </summary>
        /// <param name="_ViewModel">The view model.</param>
        public CashWithdrawal11()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
